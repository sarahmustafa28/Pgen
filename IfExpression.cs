using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLVMSharp.Interop;

namespace Parser
{
    public sealed class IfExpression(bool condition, Then expr_then, Else expr_else) : Expression
    {
        public bool Condition { get; } = condition;
        public Then Then { get; } = expr_then;
        public Else Else { get; } = expr_else;

        public override LLVMValueRef Accept(IExpressionVisitor visitor) => visitor.VisitIf(this);
    }
}
