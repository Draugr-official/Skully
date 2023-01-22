using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    /// <summary>
    /// C# using statement (e.g 'using System')
    /// </summary>
    internal class UsingStatement : Statement
    {
        public List<string> Namespace = new List<string>();

        public UsingStatement(List<string> @namespace)
        {
            Namespace = @namespace;
        }

        public UsingStatement() { }

        public override string ToString()
        {
            return $"using {string.Join(" ", this.Namespace)};";
        }
    }
}
