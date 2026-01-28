using LLVMSharp.Interop;
using System.Linq.Expressions;

namespace Parser;

public interface IExpressionVisitor
{
    LLVMValueRef VisitBinary(BinaryExpression expr);
    LLVMValueRef VisitFor(ForExpression expr);
    LLVMValueRef VisitIf(IfExpression expr);
    LLVMValueRef VisitNumber(NumberExpression expr);
    LLVMValueRef VisitVariable(VariableExpression expr);
    LLVMValueRef VisitVarInExpression(VarInExpression expr);
    LLVMValueRef VisitBody(Body expr);
    LLVMValueRef VisitThen(Then expr);
    LLVMValueRef VisitElse(Else expr);
}