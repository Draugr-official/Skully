using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Expressions
{
    class BinaryExpression : Expression
    {
        public Expression Left = new EmptyExpression();
        public Expression Right = new EmptyExpression();
        public BinaryInfix Operator = BinaryInfix.Unknown;

        public override string ToString()
        {
            return $"{this.Left.ToString()} {(char)this.Operator} {this.Right.ToString()}";
        }
    }

    public enum BinaryInfix
    {
        Unknown,
        Add = '+',
        Sub = '-',
        Mul = '*',
        Div = '/',
        Mod = '%'
    }
}
