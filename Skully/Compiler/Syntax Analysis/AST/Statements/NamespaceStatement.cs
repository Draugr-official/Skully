using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    /// <summary>
    /// C# namespace statement (e.g namespace Application { ... })
    /// </summary>
    internal class NamespaceStatement : Statement
    {
        public string Name = "";
        public BlockStatement Body = new BlockStatement();

        public NamespaceStatement(string name, BlockStatement body)
        {
            this.Name = name;
            this.Body = body;
        }

        public NamespaceStatement() { }

        public override string ToString()
        {
            return "namespace " + this.Name + "\n{\n" + this.Body.ToString() + "}";
        }
    }
}
