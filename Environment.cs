using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Environment:EnvironmentBase
    {
        public SymbolTable sym = new SymbolTable();
        public Controller control = new Controller();
        public Evaluator evaluate = new Evaluator();
        public void reportError(int lineNo,string message)
        {
            Console.WriteLine(""+lineNo+":"+message);
        }
        public override bool ErrorFlag { set; get; }
    }
}
