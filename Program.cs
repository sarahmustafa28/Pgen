using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Numerics;

namespace Parser
{
    public class Program
    {
        private static readonly Interpreter Interpreter = new();
        public static void Main(string[] args)
        {
            var parser = new Parser();
            var env = new Environment();
            List<Instruction> instructions = new List<Instruction>();
            List<Expression> expressions = new List<Expression>();

            var filePath = @"C:\Users\MRCOMPUTER\Desktop\Kaleidoscope-main\test.txt";
            var inputFile = new FileStream(filePath, FileMode.Open);
            var reader = new StreamReader(inputFile);

            parser.Parse(reader, env);
            instructions = env.evaluate.Evaluate(env.control.instructions, env.sym);

            Instruction instruct = new Instruction();

            for (int i = 0; i < instructions.Count; i++)
                {
                    instruct = instructions[i];

                Then then = new Then(env.evaluate.if_ip, env.evaluate.else_ip);
                Else @else = new Else(env.evaluate.else_ip, env.evaluate.end_if);

                switch (instruct.InstrType)
                {
                    case InstrType.FloatDecl:
                        expressions.Add(new VariableExpression(instruct.Op1.Name));
                        break;
                    case InstrType.BoolDecl:
                        expressions.Add(new VariableExpression(instruct.Op1.Name));
                        break;
                    case InstrType.Add:
                        expressions.Add(new BinaryExpression(instruct,
                                        new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)), 
                                        new VarInExpression(instruct.Op2,
                                        new VariableExpression(instruct.Op2.Name))));
                        break;
                    case InstrType.Sub:
                        expressions.Add(new BinaryExpression(instruct, 
                                        new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)), 
                                        new VarInExpression(instruct.Op2, 
                                        new VariableExpression(instruct.Op2.Name))));
                        break;
                    case InstrType.Mul:
                        expressions.Add(new BinaryExpression(instruct, 
                                        new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)), 
                                        new VarInExpression(instruct.Op2, 
                                        new VariableExpression(instruct.Op2.Name))));
                        break;
                    case InstrType.Div:
                        expressions.Add(new BinaryExpression(instruct, 
                                        new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)), 
                                        new VarInExpression(instruct.Op2, 
                                        new VariableExpression(instruct.Op2.Name))));
                        break;
                    case InstrType.GE:
                        expressions.Add(new BinaryExpression(instruct, 
                                        new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)), 
                                        new VarInExpression(instruct.Op2, 
                                        new VariableExpression(instruct.Op2.Name))));
                        break;
                    case InstrType.LE:
                        expressions.Add(new BinaryExpression(instruct, 
                                        new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)), 
                                        new VarInExpression(instruct.Op2, 
                                        new VariableExpression(instruct.Op2.Name))));
                        break;
                    case InstrType.EQ:
                        expressions.Add(new BinaryExpression(instruct, 
                                        new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)), 
                                        new VarInExpression(instruct.Op2, 
                                        new VariableExpression(instruct.Op2.Name))));
                        break;
                    case InstrType.NE:
                        expressions.Add(new BinaryExpression(instruct, 
                                        new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)), 
                                        new VarInExpression(instruct.Op2, 
                                        new VariableExpression(instruct.Op2.Name))));
                        break;
                    case InstrType.FloatAssign:
                        expressions.Add(new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)));
                        break;
                    case InstrType.BoolAssign:
                        expressions.Add(new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)));
                        break;
                    case InstrType.Input:
                        expressions.Add(new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)));
                        break;
                    case InstrType.Output:
                        expressions.Add(new VarInExpression(instruct.Op1, 
                                        new VariableExpression(instruct.Op1.Name)));
                        break;
                        
                    case InstrType.IfGoto:
                        expressions.Add(new IfExpression(env.evaluate.cond_result,then,@else));
                        expressions.Add(then);
                        break;
                    case InstrType.NotGoto:
                        expressions.Add(@else);
                        break;
                    case InstrType.For:
                        expressions.Add(new ForExpression(env.evaluate.varname, 
                                        env.evaluate.start,
                                        env.evaluate.end, 
                                        new Body(env.evaluate.for_ip, env.evaluate.end_for)));
                        break;
                }

            }
            if (expressions is not null)
            {
                Interpreter.Run(expressions);
            }
        }
    }
}
