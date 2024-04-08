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
        // this functions edits float variable already exist in the symbol table
        public void editFloat(string key, float newvalue)
        {
            Variable v = symTable[key];
            if (v != null)
            {
                Float_Var fv = (Float_Var)v;
                fv.float_value = newvalue;
                symTable[key] = fv;
            }
        }
        // this functions edits bool variable already exist in the symbol table
        public void editBool(string key, bool newvalue)
        {
            Variable v = symTable[key];
            if (v != null)
            {
                Bool_Var bv = (Bool_Var)v;
                bv.bool_value = newvalue;
                symTable[key] = bv;
            }
        }
        // this variable returns float variable from the symbol table
            public Float_Var getFloat(string key)
        {
                Variable v = LookUp(key);
                v.var_type = VarType.Float;
                Float_Var fv = (Float_Var)v;
                return fv;
        }
        // this variable returns bool variable from the symbol table
        public Bool_Var getBool(string key)
        {
            Variable v = LookUp(key);
            v.var_type = VarType.Bool;
            Bool_Var bv = (Bool_Var)v;
                return bv;
            
        }


    }
}
