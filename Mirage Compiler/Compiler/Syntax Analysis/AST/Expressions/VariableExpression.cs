using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Expressions
{
    internal class VariableExpression : Expression
    {
        public string Name = "";

        public override string ToString()
        {
            return this.Name;
        }
    }
}
