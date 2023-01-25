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
        public List<VariableExpression> Parameters = new List<VariableExpression>(); // Variable expressions
        public BlockStatement Body = new BlockStatement();

        public MethodStatement(DataType returnType, List<Modifiers> modifiers, string name, List<VariableExpression> arguments, BlockStatement body)
        {
            this.ReturnType = returnType;
            this.Modifiers = modifiers;
            this.Name = name;
            this.Parameters = arguments;
            this.Body = body;
        }

        public MethodStatement() { }

        public override string ToString()
        {
            return $"{Objects.ModifierInfo.ListToString(this.Modifiers)} {(this.ReturnType.Type == DataTypes.Other ? this.ReturnType.Value : Objects.DataTypeInfo.ToString[this.ReturnType.Type])} {this.Name}(" 
                + String.Join(", ", this.Parameters.Select(t => t.ToString())) + ")\n{\n" + this.Body.ToString() + "}\n";
        }
    }
}
