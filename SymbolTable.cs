using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Parser
{
    // this class defines the symbol table and its functions
    public class SymbolTable
    {
    // the symbol table is a list with a key refers to the variable name and the variable itself
        
        protected SortedList<string, Variable> symTable = new SortedList<string,Variable>(128);
        

        // this function adds a variable in the symbol table
        public void Add(string key, Variable variable)
        {
            symTable[key] = variable;
        }
       // this function checks the variable existance in the symbol table
        public Variable LookUp(string key)
        {
            if (symTable.ContainsKey(key))
                return symTable[key];
            else
            return null;
        }
        public string FindKeyOfVar(Variable var)
        {
            for (int i = 0; i < symTable.Values.Count; i++)
            {
                Variable v = symTable.Values[i];
                if (v == var)
                    return symTable.Keys[i];
            }
            return "";
        }
        // this variable returns float variable from the symbol table
        public Float_Var getFloat(string key)
        {
                Variable v = LookUp(key);
            if (v != null)
            {
                if (v.var_type == VarType.Float)
                {
                    Float_Var fv = (Float_Var)v;
                    return fv;
                }

            }
            return null;

        }
        // this variable returns bool variable from the symbol table
        public Bool_Var getBool(string key)
        {
            Variable v = LookUp(key);
            if (v != null)
            {
                if (v.var_type == VarType.Bool)
                {
                    Bool_Var bv = (Bool_Var)v;
                    return bv;
                }
                
            }
            return null;
            
        }

    }
}
