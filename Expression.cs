using LLVMSharp.Interop;

namespace Parser;
public abstract class Expression
{
    public abstract LLVMValueRef Accept(IExpressionVisitor visitor);
}