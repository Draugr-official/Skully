using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Expressions
{
    class NullExpression : Expression
    {
        public override string ToString()
        {
            return "null";
        }
    }
}
