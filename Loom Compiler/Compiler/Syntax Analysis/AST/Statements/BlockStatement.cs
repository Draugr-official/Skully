using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    /// <summary>
    /// C# block statement (e.g: { ... })
    /// </summary>
    internal class BlockStatement : Statement
    {
        public List<Statement> Statements = new List<Statement>();

        public BlockStatement(List<Statement> statements)
        {
            this.Statements = statements;
        }

        public BlockStatement() { }

        public override string ToString()
        {
            return String.Join("", this.Statements.Select(t => t.ToString()));
        }
    }
}
