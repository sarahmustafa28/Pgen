using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLVMSharp.Interop;

namespace Parser
{
    public sealed class Then : Expression
    {
        public Then(int if_ip, int else_ip)
        {
            If_ip = if_ip ;
            Else_ip = else_ip ;
        }
        public int If_ip { get; }
        public int Else_ip { get; }
        public override LLVMValueRef Accept(IExpressionVisitor visitor) => visitor.VisitThen(this);

    }
}


