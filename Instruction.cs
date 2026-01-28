using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Instruction
    {
        public InstrType InstrType;
        public Variable Op1;
        public Variable Op2;
        public Variable Result;
    }
    public enum InstrType
    {
        Add, Sub, Mul, Div, GE, LE, EQ, NE, FloatDecl, BoolDecl, FloatAssign, BoolAssign, Input, Output,
        EndIf, IfGoto, NotGoto, For, Body, EndFor
    }
}
