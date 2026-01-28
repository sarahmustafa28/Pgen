using LLVMSharp.Interop;

namespace Parser
{
    public class VariableExpression : Expression
    {
        public VariableExpression(string name)
        {
            Name = name;
        }
      
        public string Name { get; set; }
        public double Value = 0;
        public override LLVMValueRef Accept(IExpressionVisitor visitor) => visitor.VisitVariable(this);
    }
}
