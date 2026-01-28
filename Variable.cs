using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Parser.VariableExpression;
using LLVMSharp;

namespace Parser
{
    // this class defines two types of variables float and bool
    public class Variable: VariableExpression
    {
        public VarType var_type;
        
        public string Name;
        public Variable(string name) : base(name)
        {
            Name = name;
        }
    }
    public enum VarType
    {
       Float,Bool,Address,String
    }
   // this class defines Float_Var and its value
    public class Float_Var : Variable
    {
        public string Name;
        public Float_Var(string name) : base(name)
        {
            Name = name;
            var_type = VarType.Float;
        }

        public float float_value;

    }
    // this class defines Bool_Var and its value
    public class Bool_Var : Variable
    {
        public Bool_Var(string name) : base(name)
        {
            Name = name;
            var_type = VarType.Bool;
        }
        public bool bool_value;
    }
    public class Address_Var : Variable
    {
        public Address_Var(string name) : base(name)
        {
            Name = name;
            var_type = VarType.Address;
        }
        public int address_value;
    }
    public class String_Var : Variable
    {
        public String_Var(string name) : base(name)
        {
            Name = name;
            var_type = VarType.String;
        }
        public string string_value;
    }
        
    }
