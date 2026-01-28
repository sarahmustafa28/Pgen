

namespace Parser
{
    using System;
    using LLVMSharp.Interop;

    public class BinaryExpression : Expression
    {
        public BinaryExpression(Instruction i, VarInExpression lhs, VarInExpression rhs)
        {
            switch (i.InstrType)
            {
                case InstrType.Add:
                    NodeType = ExpressionType.Add;
                    OperatorValue = "+";
                    break;
                case InstrType.Sub:
                    NodeType = ExpressionType.Sub;
                    OperatorValue = "-";
                    break;
                case InstrType.Mul:
                    NodeType = ExpressionType.Mul;
                    OperatorValue = "*";
                    break;
                case InstrType.Div:
                    NodeType = ExpressionType.Div;
                    OperatorValue = "/";
                    break;
                case InstrType.GE:
                    NodeType = ExpressionType.GE;
                    OperatorValue = ">=";
                    break;
                case InstrType.LE:
                    NodeType = ExpressionType.LE;
                    OperatorValue = "<=";
                    break;
                case InstrType.EQ:
                    NodeType = ExpressionType.EQ;
                    OperatorValue = "==";
                    break;
                case InstrType.NE:
                    NodeType = ExpressionType.NE;
                    OperatorValue = "!=";
                    break;
                default:
                        throw new ArgumentException("op " + i + " is not a valid instruction");
                    break;
            }
            Lhs = lhs;
            Rhs = rhs;
            I = i;
        }

        public VarInExpression Lhs { get; }
        public VarInExpression Rhs { get; }
        public Instruction I { get; }
        public ExpressionType NodeType { get; }
        public string OperatorValue  { get; }

        public override LLVMValueRef Accept(IExpressionVisitor visitor)
       => visitor.VisitBinary(this);
    }
}