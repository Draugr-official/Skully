using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Syntax_Analysis.AST.Expressions
{
    internal class EmptyExpression : Expression
    {
        public override string ToString()
        {
            return "";
        }
    }
}
