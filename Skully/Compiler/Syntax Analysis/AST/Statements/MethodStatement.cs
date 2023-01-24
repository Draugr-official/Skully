using Skully_Compiler.Compiler.Syntax_Analysis.AST.Expressions;
using Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    internal class MethodStatement : Statement
    {
        public DataType ReturnType = new DataType();
        public List<Modifiers> Modifiers = new List<Modifiers>();
        public string Name = "";
        public List<Expression> Arguments = new List<Expression>(); // Variable expressions
        public BlockStatement Body = new BlockStatement();

        public MethodStatement(DataType returnType, List<Modifiers> modifiers, string name, List<Expression> arguments, BlockStatement body)
        {
            this.ReturnType = returnType;
            this.Modifiers = modifiers;
            this.Name = name;
            this.Arguments = arguments;
            this.Body = body;
        }

        public MethodStatement() { }

        public override string ToString()
        {
            return $"{Objects.ModifierInfo.ListToString(this.Modifiers)} {(this.ReturnType.Type == DataTypes.Other ? this.ReturnType.Value : Objects.DataTypeInfo.ToString[this.ReturnType.Type])} {this.Name}(" 
                + String.Join(", ", this.Arguments.Select(t => t.ToString())) + ")\n{\n" + this.Body.ToString() + "}\n";
        }
    }
}
