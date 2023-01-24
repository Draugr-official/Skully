using Skully_Compiler.Compiler.SyntaxAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Syntax_Analysis.AST.Expressions
{
    internal class LiteralExpression : Expression
    {
        public string Value = "";
        public LiteralTypes Type = LiteralTypes.Unknown;

        public LiteralExpression(string value, LiteralTypes type)
        {
            Value = value;
            Type = type;
        }

        public LiteralExpression() { }

        public override string ToString()
        {
            return (this.Type == LiteralTypes.String ? $"\"{this.Value}\"" : this.Value);
        }

        public static Dictionary<LexType, LiteralTypes> LiteralLookup = new Dictionary<LexType, LiteralTypes>()
        {
            { LexType.String, LiteralTypes.String },
            { LexType.Number, LiteralTypes.Number },
            { LexType.Boolean, LiteralTypes.Boolean },
        };
    }

    enum LiteralTypes
    {
        Unknown,
        String,
        Boolean,
        Number,
    }
}