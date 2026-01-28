using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLVMSharp.Interop;

namespace Parser
{
    public sealed class Else : Expression
    {
        public Else(int else_ip, int end_if)
        {
            Else_ip = else_ip;
            End_if = end_if;
        }
        public int Else_ip { get; }
        public int End_if { get; }

        public override LLVMValueRef Accept(IExpressionVisitor visitor) => visitor.VisitElse(this);

    }
}
