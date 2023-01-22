using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Expressions;
using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements.Objects;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements
{
    /// <summary>
    /// C# variable statement (e.g string var1 = "Hello")
    /// </summary>
    internal class VariableStatement : Statement
    {
        public List<Modifiers> Modifiers = new List<Modifiers>();
        public DataType ReturnType = new DataType();
        public string Name = "";

        public Expression DefaultValue = new Expression();

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
            return "";
        }
    }
}
