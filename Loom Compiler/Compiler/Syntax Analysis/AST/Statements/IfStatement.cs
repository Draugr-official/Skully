using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Expressions;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    /// <summary>
    /// C# if statement (e.g 'if(...) { ... }')
    /// </summary>
    internal class IfStatement : Statement
    {
        public Expression Condition = new Expression();
        public BlockStatement Body = new BlockStatement();

        public IfStatement(Expression condition, BlockStatement body)
        {
            this.Condition = condition;
            this.Body = body;
        }

        public IfStatement() { }

        public override string ToString()
        {
            return "if(" + this.Condition.ToString() + ")\n{\n" + this.Body.ToString() + "\n}";
        }
    }
}
