using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    // this class defines two types of variables float and bool
    public class Variable
    {
        public VarType var_type;
    }
    public enum VarType
    {
       Float,Bool
    }
   // this class defines Float_Var and its value
    public class Float_Var : Variable
    {
        public Float_Var()
        {
            var_type = VarType.Float;
        }
        public float float_value;
    }
    // this class defines Bool_Var and its value
    public class Bool_Var : Variable
    {
        public Bool_Var()
        {
            var_type = VarType.Bool;
        }
        public bool bool_value;
    }
}
