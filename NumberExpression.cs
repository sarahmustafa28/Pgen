using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLVMSharp.Interop;
using Parser;

namespace Parser
{
    public sealed class NumberExpression(double value) : Expression
    {
        public double Value { get; } = value;

        public override LLVMValueRef Accept(IExpressionVisitor visitor) => visitor.VisitNumber(this);
    }
}
