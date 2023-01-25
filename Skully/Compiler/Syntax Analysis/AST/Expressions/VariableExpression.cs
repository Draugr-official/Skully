using Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Syntax_Analysis.AST.Expressions
{
    internal class VariableExpression : Expression
    {
        public string Name = "";
        public DataType DataType = new DataType();

        public override string ToString()
        {
            return $"{(this.DataType.Type == DataTypes.Unknown ? "" : $"{DataTypeInfo.ToString[this.DataType.Type]} ") }{this.Name}";
        }
    }
}
