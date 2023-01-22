using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Expressions;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    /// <summary>
    /// C# while statement (e.g 'while( ... ) { ... }')
    /// </summary>
    internal class WhileStatement : Statement
    {
        public Expression Condition = new Expression();
        public BlockStatement Body = new BlockStatement();

        public WhileStatement(Expression condition, BlockStatement body)
        {
            this.Condition = condition;
            this.Body = body;
        }

        public WhileStatement() { }

        public override string ToString()
        {
            return "while(" + this.Condition.ToString() + ")\n{" + this.Body.ToString() + "\n}";
        }
    }
}
