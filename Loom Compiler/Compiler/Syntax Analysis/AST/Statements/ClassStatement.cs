using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    /// <summary>
    /// C# class statement (e.g 'class Program { ... })
    /// </summary>
    internal class ClassStatement : Statement
    {
        public string Name = "";
        public Expression Attribute = new Expression();
        public BlockStatement Body = new BlockStatement();

        public ClassStatement(string name, BlockStatement body)
        {
            this.Name = name;
            this.Body = body;
        }

        public ClassStatement() { }

        public override string ToString()
        {
            return "class " + this.Name + "\n{\n" + this.Body.ToString() + "}\n";
        }
    }
}
