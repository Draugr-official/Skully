using Skully_Compiler.Compiler.Syntax_Analysis.AST.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    /// <summary>
    /// C# method call statement (e.g Console.WriteLine("Hello, World!"))
    /// </summary>
    internal class MethodCallStatement : Statement
    {
        public string Name = "";
        public List<Expression> Arguments = new List<Expression>();

        public MethodCallStatement(string name, List<Expression> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public MethodCallStatement() { }

        public override string ToString()
        {
            return $"{this.Name}({String.Join(", ", this.Arguments.Select(t => t.ToString()))});\n";
        }
    }
}
