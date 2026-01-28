using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLVMSharp.Interop;

namespace Parser
{
    public sealed class Body(int for_ip, int end_ip) : Expression
    {
        public int For_ip { get; } = for_ip;
        public int End_ip { get; } = end_ip;
        public override LLVMValueRef Accept(IExpressionVisitor visitor) => visitor.VisitBody(this);

    }
}
