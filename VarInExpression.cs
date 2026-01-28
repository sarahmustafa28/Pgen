using LLVMSharp.Interop;
using Parser;

namespace Parser
{
    public sealed class VarInExpression : Expression
    {
        public VarInExpression(Variable v, VariableExpression ve)
        {
            VE = ve;
            if (VE != null)
            {
                if (v.var_type == VarType.Float)
                {
                    Float_Var fv = (Float_Var)v;
                    VE.Value = fv.float_value;
                }
                if (v.var_type == VarType.Bool)
                {
                    Bool_Var bv = (Bool_Var)v;
                    if (bv.bool_value == true)
                        VE.Value = 1;
                    else
                        VE.Value = 0;
                }
            }
        }


        public VariableExpression VE { get; }

        public override LLVMValueRef Accept(IExpressionVisitor visitor) => visitor.VisitVarInExpression(this);
    }
}
