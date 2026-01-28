using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Controller
    {
        public int ip;

        public List<Instruction> instructions = new List<Instruction>();


        public void AddAddress(SymbolTable sym)
        {
            ip++;
            Address_Var address = new Address_Var(ip.ToString());
            address.address_value = ip;
            sym.Add(ip.ToString(), address);

        }

        public void FloatDecl(string op1, SymbolTable sym)
        {
            if (sym.LookUp(op1) != null)
            {
                Console.WriteLine(op1 + "is already declared");
            }
            else
            {
                Instruction i = new Instruction();
                i.InstrType = InstrType.FloatDecl;
                Float_Var float_Var = new Float_Var(op1);
                float_Var.float_value = 0;
                sym.Add(op1, float_Var);
                i.Op1 = float_Var;
                instructions.Add(i);
            }
        }
        public void BoolDecl(string op1, SymbolTable sym)
        {
            if (sym.LookUp(op1) != null)
            {
                Console.WriteLine(op1 + "is already declared");
            }
            else
            {
                Instruction i = new Instruction();
                i.InstrType = InstrType.BoolDecl;
                Bool_Var bool_Var = new Bool_Var(op1);
                bool_Var.bool_value = false;
                sym.Add(op1, bool_Var);
                i.Op1 = bool_Var;
                instructions.Add(i);
            }
        }
        public void FloatAssign(string op1, float value, SymbolTable sym)
        {
            if (sym.LookUp(op1) != null)
            {
                Instruction i = new Instruction();
                i.InstrType = InstrType.FloatAssign;
                Float_Var floatvar = sym.getFloat(op1);
                if (floatvar != null)
                {
                    floatvar.float_value = value;
                    sym.Add(op1, floatvar);
                    i.Op1 = floatvar;
                    instructions.Add(i);
                }
                else
                {
                    Console.WriteLine(op1 + "is not declared");
                }

            }
        }
        public void BoolAssign(string op1, bool value, SymbolTable sym)
        {
            if (sym.LookUp(op1) != null)
            {
                Instruction i = new Instruction();
                i.InstrType = InstrType.BoolAssign;
                Bool_Var boolvar = sym.getBool(op1);
                if (boolvar != null)
                {
                    boolvar.bool_value = value;
                    sym.Add(op1, boolvar);
                    i.Op1 = boolvar;
                    instructions.Add(i);
                }
                else
                {
                    Console.WriteLine(op1 + "is not declared");
                }

            }
        }
        public float Add(float op1, float op2, SymbolTable sym)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.Add;
            Float_Var num1 = new Float_Var(op1.ToString());
            num1.float_value = op1;
            Float_Var num2 = new Float_Var(op2.ToString());
            num2.float_value = op2;
            string Result = "Result" + ip;
            Float_Var result = new Float_Var(Result);
            i.Op1 = num1;
            i.Op2 = num2;
            i.Result = result;
            sym.Add(Result, result);
            instructions.Add(i);
            return result.float_value;
        }
        public float Sub(float op1, float op2, SymbolTable sym)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.Sub;
            Float_Var num1 = new Float_Var(op1.ToString());
            num1.float_value = op1;
            Float_Var num2 = new Float_Var(op2.ToString());
            num2.float_value = op2;
            string Result = "Result" + ip;
            Float_Var result = new Float_Var(Result);
            i.Op1 = num1;
            i.Op2 = num2;
            i.Result = result;
            sym.Add(Result, result);
            instructions.Add(i);
            return result.float_value;
        }
        public float Mul(float op1, float op2, SymbolTable sym)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.Mul;
            Float_Var num1 = new Float_Var(op1.ToString());
            num1.float_value = op1;
            Float_Var num2 = new Float_Var(op2.ToString());
            num2.float_value = op2;
            string Result = "Result" + ip;
            Float_Var result = new Float_Var(Result);
            i.Op1 = num1;
            i.Op2 = num2;
            i.Result = result;
            sym.Add(Result, result);
            instructions.Add(i);
            return result.float_value;
        }
        public float Div(float op1, float op2, SymbolTable sym)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.Div;
            Float_Var num1 = new Float_Var(op1.ToString());
            num1.float_value = op1;
            Float_Var num2 = new Float_Var(op2.ToString());
            num2.float_value = op2;
            string Result = "Result" + ip;
            Float_Var result = new Float_Var(Result);
            i.Op1 = num1;
            i.Op2 = num2;
            i.Result = result;
            sym.Add(Result, result);
            instructions.Add(i);
            return result.float_value;
        }
        public void Input(string op1, SymbolTable sym)

        {
            if (sym.LookUp(op1) != null)
            {
                Instruction i = new Instruction();
                i.InstrType = InstrType.Input;
                Variable v = sym.LookUp(op1);
                if (v != null)
                {
                    if (v.var_type == VarType.Float)
                    {
                        Float_Var num = (Float_Var)v;
                        i.Op1 = num;
                        instructions.Add(i);
                    }
                    if(v.var_type == VarType.Bool)
                    {
                        Bool_Var boolean = (Bool_Var)v;
                        i.Op1 = boolean;
                        instructions.Add(i);
                    }
                }
            }
        }

        public void Output(string op1, SymbolTable sym)

        {
            if (sym.LookUp(op1) != null)
            {
                Variable v = sym.LookUp(op1);
                Instruction i = new Instruction();
                i.InstrType = InstrType.Output;
                if (v.var_type == VarType.Float)
                {
                    Float_Var num = (Float_Var)v;
                    i.Op1 = num;
                    instructions.Add(i);
                }
                if (v.var_type == VarType.Bool)
                {
                    Bool_Var boolean = (Bool_Var)v;
                    i.Op1 = boolean;
                    instructions.Add(i);
                }
            }
        }
        public void IfGoto(int address, bool cond_result)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.IfGoto;
            Address_Var jump_add = new Address_Var(ip.ToString());
            jump_add.address_value = address;
            Bool_Var boolean = new Bool_Var("condition");
            boolean.bool_value = cond_result;
            i.Op1 = boolean;
            i.Result = jump_add;
            instructions.Add(i);
        }
        public void Body(int start_address, int end_adress, int start, int end, string id)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.Body;
            Address_Var jump_add = new Address_Var(ip.ToString());
            jump_add.address_value = start_address;
            Float_Var Start = new Float_Var(id);
            Start.float_value = start;
            i.Op1 = Start; 
            Float_Var End = new Float_Var(id);
            End.float_value = end;
            i.Op2 = End;
            i.Result = jump_add;
            instructions.Add(i);
        }
        public void NotGoto(int address, bool cond_result)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.NotGoto;
            Address_Var jump_add = new Address_Var(ip.ToString());
            jump_add.address_value = address;
            Bool_Var boolean = new Bool_Var("condition");
            boolean.bool_value = cond_result;
            i.Op1 = boolean;
            i.Result = jump_add;
            instructions.Add(i);
        }
        public void EndIf()
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.EndIf;
            instructions.Add(i);
        }
        public void EndFor()
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.EndFor;
            instructions.Add(i);
        }
        public void For()
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.For;
            instructions.Add(i);
        }
        public bool GE(float op1, float op2, SymbolTable sym)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.GE;
            Float_Var num1 = new Float_Var(op1.ToString());
            num1.float_value = op1;
            Float_Var num2 = new Float_Var(op2.ToString());
            num2.float_value = op2;
            string Result = "Result" + ip;
            Bool_Var result = new Bool_Var(Result);
            i.Op1 = num1;
            i.Op2 = num2;
            i.Result = result;
            sym.Add(Result, result);
            instructions.Add(i);
            return result.bool_value;
        }
        public bool LE(float op1, float op2, SymbolTable sym)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.LE;
            Float_Var num1 = new Float_Var(op1.ToString());
            num1.float_value = op1;
            Float_Var num2 = new Float_Var(op2.ToString());
            num2.float_value = op2;
            string Result = "Result" + ip;
            Bool_Var result = new Bool_Var(Result);
            i.Op1 = num1;
            i.Op2 = num2;
            i.Result = result;
            sym.Add(Result, result);
            instructions.Add(i);
            return result.bool_value;
        }
        public bool EQ(float op1, float op2, SymbolTable sym)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.EQ;
            Float_Var num1 = new Float_Var(op1.ToString());
            num1.float_value = op1;
            Float_Var num2 = new Float_Var(op2.ToString());
            num2.float_value = op2;
            string Result = "Result" + ip;
            Bool_Var result = new Bool_Var(Result);
            i.Op1 = num1;
            i.Op2 = num2;
            i.Result = result;
            sym.Add(Result, result);
            instructions.Add(i);
            return result.bool_value;
        }
        public bool NE(float op1, float op2, SymbolTable sym)
        {
            Instruction i = new Instruction();
            i.InstrType = InstrType.NE;
            Float_Var num1 = new Float_Var(op1.ToString());
            num1.float_value = op1;
            Float_Var num2 = new Float_Var(op2.ToString());
            num2.float_value = op2;
            string Result = "Result" + ip;
            Bool_Var result = new Bool_Var(Result);
            i.Op1 = num1;
            i.Op2 = num2;
            i.Result = result;
            sym.Add(Result, result);
            instructions.Add(i);
            return result.bool_value;
        }
     
        public List<Instruction> getInstructions()
        {
            return instructions;
        }
    }
}
