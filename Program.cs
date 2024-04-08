using System;
using System.IO;

namespace Parser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = new Parser();
            var env = new Environment();


            var filePath = @"C:\Users\MRCOMPUTER\Desktop\Pgen2\test.txt";
            var inputFile = new FileStream(filePath, FileMode.Open);
            var reader = new StreamReader(inputFile);

            parser.Parse(reader, env);
        
        }
    }
}
