using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LLVMSharp.Interop;

namespace Parser
{
    public sealed class ForExpression(string varname, int start, int end, Body body) : Expression
    {
        public string VarName { get; } = varname;

        public int Start { get; } = start;

        public int End { get; } = end;

        public Body Body { get; } = body;

        public ExpressionType NodeType { get; } = ExpressionType.ForExpression;

        public override LLVMValueRef Accept(IExpressionVisitor visitor) => visitor.VisitFor(this);
    }
}

