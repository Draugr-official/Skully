using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skully_Compiler.Compiler.Syntax_Analysis.AST.Expressions;
using Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements.Objects;

namespace Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    /// <summary>
    /// C# variable statement (e.g string var1 = "Hello")
    /// </summary>
    internal class VariableStatement : Statement
    {
        public List<Modifiers> Modifiers = new List<Modifiers>();
        public DataType ReturnType = new DataType();
        public string Name = "";

        public Expression DefaultValue = new EmptyExpression();

        public VariableStatement(List<Modifiers> modifiers, DataType returnType, string name, Expression defaultValue)
        {
            Modifiers = modifiers;
            ReturnType = returnType;
            Name = name;
            DefaultValue = defaultValue;
        }

        public VariableStatement() { }

        public override string ToString()
        {
            return $"{this.ReturnType.Value} {this.Name} {(DefaultValue is EmptyExpression ? "" : "= " + this.DefaultValue.ToString())};";
        }
    }
}
