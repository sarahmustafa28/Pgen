using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using Parser;
using LLVMSharp;
using LLVMSharp.Interop;
using parser;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Timers;
using System.Diagnostics;

namespace Parser
{
    public delegate void Print(double d); 
    public delegate double RunDoubleFunction();

    public unsafe class Interpreter : IExpressionVisitor
    {
        unsafe public LLVMModuleRef _module;
        unsafe public LLVMBuilderRef _builder;
        unsafe public LLVMExecutionEngineRef _engine;
        unsafe public LLVMOpaquePassBuilderOptions* _passBuilderOptions;
        unsafe public Dictionary<string, Expression> _functions;
        unsafe public Context _context;

        unsafe public List<LLVMValueRef> toRun = new List<LLVMValueRef>();
        unsafe public List<Expression> expressions = new List<Expression>();


        unsafe public int start;
        unsafe public int end;
        unsafe public int then_index;
        unsafe public int else_index;
        public static class LLVMCAPI
        {
            // Define the DllImports for the necessary functions
            [DllImport("libLLVM.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr LLVMPrintValueToString(LLVMValueRef Val);

            [DllImport("libLLVM.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void LLVMDisposeMessage(IntPtr Message);

            // ... potentially other DllImports for your LLVM operations
        }
        public void PrintMyValue(LLVMValueRef valueRef)
        {
            // Get the string representation as an unmanaged pointer
            IntPtr messagePtr = LLVMCAPI.LLVMPrintValueToString(valueRef);

            // Convert the unmanaged pointer to a managed C# string
            string valueAsString = Marshal.PtrToStringAnsi(messagePtr); // Use PtrToStringUTF8 if using UTF8 bindings

            // Print the string
            Console.WriteLine(valueAsString);

            // Free the memory allocated by the LLVM function
            LLVMCAPI.LLVMDisposeMessage(messagePtr);
        }


        public void PutChard(double x)
        {
            try
            {
                Console.Write((char)x);
            }
            catch
            {
            }
        }

        public Interpreter()
        {
            LLVM.InitializeNativeTarget();
            LLVM.InitializeNativeAsmPrinter();
            LLVM.InitializeNativeAsmParser();
            _functions = new Dictionary<string, Expression>();
            _context = Context.Empty;
        }

        public void InitializeModule()
        {
            _module = LLVMModuleRef.CreateWithName("Kaleidoscope Module");
            _builder = _module.Context.CreateBuilder();
            _passBuilderOptions = LLVM.CreatePassBuilderOptions();

            _engine = _module.CreateMCJITCompiler();

            var ft = LLVMTypeRef.CreateFunction(LLVMTypeRef.Double, [LLVMTypeRef.Double]);
            var write = _module.AddFunction("putchard", ft);
            write.Linkage = LLVMLinkage.LLVMExternalLinkage;
            Delegate d = new Print(PutChard);
            var p = Marshal.GetFunctionPointerForDelegate(d);
            _engine.AddGlobalMapping(write, p);
        }

        public void Run(List<Expression> exprs)
        {
            // If we modify the module after we already executed some function with
            // _engine.RunFunction it will break, so for each run we instantiate the module again
            // any previous defined function will be emitted again in the current module
            expressions = exprs;
            InitializeModule();
            for (int i = 0; i < exprs.Count; i++)
            {
                _context = Context.Empty;
                var v = Visit(expressions[i]);
                toRun.Add(v);
            }

            var passes = new MarshaledString("mem2reg,instcombine,reassociate,gvn,simplifycfg");
            var passesError = LLVM.RunPasses(_module, passes, _engine.TargetMachine, _passBuilderOptions);

            if (passesError != null)
            {
                sbyte* errorMessage = LLVM.GetErrorMessage(passesError);
                var span = MemoryMarshal.CreateReadOnlySpanFromNullTerminated((byte*)errorMessage);
                Console.WriteLine(span.AsString());
                return;
            }


            foreach (var v in toRun)
            {
                if (v != null)
                {
                    PrintMyValue(v);
                }
            }
            // to get exe file we follow these steps
            // 1. Get the output bit file

            _module.WriteBitcodeToFile("output.bc");

            using (var path = new MarshaledString("output.bc"))
            {
                LLVM.WriteBitcodeToFile(_module, path);
            }
            unsafe
            {
                // 2. Prepare the null-terminated path
                byte[] pathBytes = System.Text.Encoding.UTF8.GetBytes("output.ll\0");

                fixed (byte* pPath = pathBytes)
                {
                    // 3. Declare a pointer to receive error messages from LLVM
                    sbyte* errorMessagePtr = null;

                    // 4. Call the function (returns 0 for success)
                    int result = LLVM.PrintModuleToFile(_module, (sbyte*)pPath, &errorMessagePtr);

                    if (result != 0)
                    {
                        // 5. Handle error if the return value is non-zero
                        string error = Marshal.PtrToStringAnsi((IntPtr)errorMessagePtr);

                        // 6. CRITICAL: LLVM allocates the error string; you must free it to avoid leaks
                        LLVM.DisposeMessage(errorMessagePtr);

                        throw new Exception($"LLVM Error: {error}");
                    }
                }

            }
            // 7. Emit the module to an object file
            // Note: PrintToFile/WriteBitcodeToFile only save IR; EmitToFile creates machine code
            // 8. Setup the target machine (Windows x64)
            var triple = "x86_64-pc-windows-msvc";
            var target = LLVMTargetRef.GetTargetFromTriple(triple);
            var machine = target.CreateTargetMachine(triple, "generic", "",
                            LLVMCodeGenOptLevel.LLVMCodeGenLevelDefault,
                            LLVMRelocMode.LLVMRelocDefault,
                            LLVMCodeModel.LLVMCodeModelDefault);

            machine.TryEmitToFile(_module, "output.obj", LLVMCodeGenFileType.LLVMObjectFile, out string Error);
           
            // 9. Dispose module and builder
            LLVM.DisposePassBuilderOptions(_passBuilderOptions);
            _builder.Dispose();
            _module.Dispose();

            // 10. Use clang as a driver to handle linking system libraries automatically
            var processInfo = new ProcessStartInfo
            {
                FileName = "clang",
                Arguments = "output.obj -o my_program.exe",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(processInfo);
            process.WaitForExit();
        }
        public LLVMValueRef Visit(Expression body) => body.Accept(this);
        public LLVMValueRef BinaryVal(LLVMValueRef lhsVal, LLVMValueRef rhsVal, ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Add:
                    return _builder.BuildFAdd(lhsVal, rhsVal, "addtmp");
                case ExpressionType.Sub:
                    return _builder.BuildFSub(lhsVal, rhsVal, "addtmp");
                case ExpressionType.Mul:
                    return _builder.BuildFMul(lhsVal, rhsVal, "addtmp");
                case ExpressionType.Div:
                    return _builder.BuildFDiv(lhsVal, rhsVal, "addtmp");
                case ExpressionType.LE:
                    var i = _builder.BuildFCmp(LLVMRealPredicate.LLVMRealOLE, lhsVal, rhsVal, "cmptmp");
                    return _builder.BuildUIToFP(i, LLVMTypeRef.Double, "booltmp");
                case ExpressionType.GE:
                    var j = _builder.BuildFCmp(LLVMRealPredicate.LLVMRealOGE, lhsVal, rhsVal, "cmptmp");
                    return _builder.BuildUIToFP(j, LLVMTypeRef.Double, "booltmp");
                case ExpressionType.EQ:
                    var k = _builder.BuildFCmp(LLVMRealPredicate.LLVMRealOEQ, lhsVal, rhsVal, "cmptmp");
                    return _builder.BuildUIToFP(k, LLVMTypeRef.Double, "booltmp");
                case ExpressionType.NE:
                    var l = _builder.BuildFCmp(LLVMRealPredicate.LLVMRealONE, lhsVal, rhsVal, "cmptmp");
                    return _builder.BuildUIToFP(l, LLVMTypeRef.Double, "booltmp");
                default:
                    throw new InvalidOperationException();

            }
        }

        public LLVMValueRef VisitBinary(BinaryExpression expr)
        {
            VarInExpression lhs = expr.Lhs;
            VarInExpression rhs = expr.Rhs;

            var lhsVal = VisitVarInExpression(lhs);
            var rhsVal = VisitVarInExpression(rhs);
            return BinaryVal(lhsVal, rhsVal, expr.NodeType);
        }


        public LLVMValueRef VisitNumber(NumberExpression expr) => LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, expr.Value);


        public LLVMValueRef VisitVariable(VariableExpression expr)
        {

            // --- Setup for the Function/Block ---

            // We need a function to allocate the variable inside.
            LLVMTypeRef dty = _module.Context.DoubleType;
            LLVMTypeRef funcType = LLVMTypeRef.CreateFunction(dty, new LLVMTypeRef[] { }, false);
            LLVMValueRef function = _module.AddFunction("allocatefunction", funcType);
            LLVMBasicBlockRef entryBlock = function.AppendBasicBlock("entry");

            // The Builder must be positioned at the start of the block
            _builder.PositionAtEnd(entryBlock);

            LLVMValueRef allocated_Ptr = _builder.BuildAlloca(dty, expr.Name);

            LLVM.BuildStore(_builder, LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, expr.Value), allocated_Ptr);

            var value = _builder.BuildLoad2(LLVMTypeRef.Double, allocated_Ptr, expr.Name);

            _builder.BuildRet(value);

            return LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, expr.Value);


        }


        public LLVMValueRef VisitVarInExpression(VarInExpression expr)
        {
            var value = VisitVariable(expr.VE);
            return value;
        }

        public LLVMValueRef VisitFor(ForExpression expr)
        {
            start = expr.Start;
            end = expr.End;
            VisitBody(expr.Body);

            var zero = LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, 0);
            return zero;
        }
        public LLVMValueRef VisitBody(Body body)
        {
            for (int i = 0; i < body.End_ip - body.For_ip; i++)
                expressions.Remove(expressions[body.For_ip -1]);
            
            var zero = LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, 0);
            return zero;
        }
        public LLVMValueRef VisitThen(Then then)
        {
                expressions.RemoveRange(then_index, then.Else_ip - then.If_ip );

            var zero = LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, 0);
            return zero;
        }
        public LLVMValueRef VisitElse(Else @else)
        {
                expressions.RemoveRange(else_index, @else.End_if - @else.Else_ip);

            var zero = LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, 0);
            return zero;
        }
        public LLVMValueRef VisitIf(IfExpression expr)
        {
            foreach (Expression ex in expressions)
            {
                if (ex is Then)
                    then_index = expressions.IndexOf(ex);
                if (ex is Else)
                    else_index = expressions.IndexOf(ex);
            }
            var exprThen = expr.Then;
            var exprElse = expr.Else;
            var cond = expr.Condition;
            if (cond)
            {
                VisitElse(exprElse);
                expressions.Remove(expressions[then_index]);
            }
            else
            {
                VisitThen(exprThen);
                expressions.Remove(expressions[then_index]);
            }

                var zero = LLVMValueRef.CreateConstReal(LLVMTypeRef.Double, 0);
            
            return zero;

        }
      
    }
}

    
