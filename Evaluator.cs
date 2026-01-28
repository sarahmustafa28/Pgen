using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Evaluator
    {
        public bool if_flage;
        public bool for_flage;
        public bool cond_result;
        public int for_ip;
        public int end_for;
        public int if_ip;
        public int else_ip;
        public int end_if;
        public int start;
        public int end;
        public string varname;

        public List<Instruction> Evaluate(List<Instruction> instructions, SymbolTable sym)
        {
            Instruction instruct = new Instruction();
            Float_Var num1;
            Float_Var num2;
            Float_Var n_result;
            Bool_Var bool1;
            Bool_Var bool2;
            Bool_Var b_result;
            Address_Var jump_add;

            for (int i = 0; i < instructions.Count; i++)
            {
                instruct = instructions[i];
                switch (instruct.InstrType)
                {
                    case InstrType.FloatDecl:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                System.Console.WriteLine("Done");
                            }
                        }
                        break;
                    case InstrType.BoolDecl:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                bool1 = (Bool_Var)instruct.Op1;
                                System.Console.WriteLine("Done");
                            }
                        }
                        break;
                    case InstrType.FloatAssign:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                System.Console.WriteLine(num1.float_value);
                            }
                        }
                        break;
                    case InstrType.BoolAssign:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                bool1 = (Bool_Var)instruct.Op1;
                                System.Console.WriteLine(bool1.bool_value);
                            }
                        }
                        break;
                    case InstrType.Add:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                num2 = (Float_Var)instruct.Op2;
                                n_result = (Float_Var)instruct.Result;
                                n_result.float_value = num1.float_value + num2.float_value;
                                Console.WriteLine(n_result.float_value);
                            }
                        }
                        break;
                    case InstrType.Sub:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                num2 = (Float_Var)instruct.Op2;
                                n_result = (Float_Var)instruct.Result;
                                n_result.float_value = num1.float_value - num2.float_value;
                                Console.WriteLine(n_result.float_value);
                            }
                        }
                        break;
                    case InstrType.Mul:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                num2 = (Float_Var)instruct.Op2;
                                n_result = (Float_Var)instruct.Result;
                                n_result.float_value = num1.float_value * num2.float_value;
                                Console.WriteLine(n_result.float_value);
                            }
                        }
                        break;
                    case InstrType.Div:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                num2 = (Float_Var)instruct.Op2;
                                n_result = (Float_Var)instruct.Result;
                                n_result.float_value = num1.float_value / num2.float_value;
                                Console.WriteLine(n_result.float_value);
                            }
                        }
                        break;
                    case InstrType.GE:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                num2 = (Float_Var)instruct.Op2;
                                b_result = (Bool_Var)instruct.Result;
                                b_result.bool_value = num1.float_value >= num2.float_value;
                                Console.WriteLine(b_result.bool_value);
                            }
                        } 
                        break;
                    case InstrType.LE:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                num2 = (Float_Var)instruct.Op2;
                                b_result = (Bool_Var)instruct.Result;
                                b_result.bool_value = num1.float_value <= num2.float_value;
                                Console.WriteLine(b_result.bool_value);
                            }
                        }
                        break;
                    case InstrType.EQ:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                num2 = (Float_Var)instruct.Op2;
                                b_result = (Bool_Var)instruct.Result;
                                b_result.bool_value = num1.float_value == num2.float_value;
                                Console.WriteLine(b_result.bool_value);
                            }
                        }
                        break;
                    case InstrType.NE:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                num2 = (Float_Var)instruct.Op2;
                                b_result = (Bool_Var)instruct.Result;
                                b_result.bool_value = num1.float_value != num2.float_value;
                                Console.WriteLine(b_result.bool_value);
                            }
                        }
                        break;
                    case InstrType.Input:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                string key = sym.FindKeyOfVar(instruct.Op1);
                                System.Console.WriteLine("Enter a value of variable(" + key + ")");
                                string input = System.Console.ReadLine();

                                if (input != null)
                                {
                                    Float_Var floatvar = sym.getFloat(key);
                                    if (floatvar != null)
                                    {
                                        floatvar.float_value = float.Parse(input);
                                        sym.Add(key, floatvar);
                                        Console.WriteLine("Done");
                                    }
                                    else
                                    {
                                        Bool_Var boolvar = sym.getBool(key);
                                        if (boolvar != null)
                                        {
                                            boolvar.bool_value = bool.Parse(input);
                                            sym.Add(key, boolvar);
                                            Console.WriteLine("Done");
                                        }
                                        else
                                            System.Console.WriteLine("The variable does not exist");

                                    }
                                }
                                else
                                    System.Console.WriteLine("Please type the value");

                            }
                        }
                        break;
                    case InstrType.Output:
                        if (!if_flage)
                        {
                            if (!for_flage)
                            {
                                string output = sym.FindKeyOfVar(instruct.Op1);

                                Variable v1 = sym.LookUp(output);
                                if (v1.var_type == VarType.Float)
                                {
                                    Float_Var floatvar1 = sym.getFloat(output);
                                    Console.WriteLine(floatvar1.float_value);
                                }
                                if (v1.var_type == VarType.Bool)
                                {
                                    Bool_Var boolvar1 = sym.getBool(output);
                                    Console.WriteLine(boolvar1.bool_value);
                                }
                            }
                        }
                        break;

                    case InstrType.IfGoto:
                            bool2 = (Bool_Var)instruct.Op1;
                            if (bool2.bool_value)
                            {
                                if_flage = false;
                                i = i++;
                            }
                            else
                            {
                                if_flage = true;
                            }
                        break;
                    case InstrType.NotGoto:
                            bool2 = (Bool_Var)instruct.Op1;
                            if (!bool2.bool_value)
                            {
                                if_flage = false;
                                i = i++;
                            }
                            else
                            {
                                if_flage = true;
                            }
                        break;
                    case InstrType.For:
                            for_flage = true;
                        break;
                    case InstrType.Body:
                        if (!if_flage)
                        {
                            if (for_flage)
                            {
                                num1 = (Float_Var)instruct.Op1;
                                num2 = (Float_Var)instruct.Op2;
                                jump_add = (Address_Var)instruct.Result;

                                for (int j = (int)num1.float_value; j <= num2.float_value; j++)
                                {
                                    List<Instruction> body = new List<Instruction>();
                                    for (int k = 0; k <=  end_for - for_ip; k++)
                                    {
                                        i = jump_add.address_value + k;
                                        body.Add(instructions[i]);
                                    }
                                    instructions.InsertRange(jump_add.address_value, body);
                                }
                                for_flage = false;
                            }
                        }
                        break;
                    case InstrType.EndIf:
                                if_flage = false;
                                break;

                    case InstrType.EndFor:
                                for_flage = false;
                                break;
                            }
                        }
                        return instructions;
                }
            }
        }
