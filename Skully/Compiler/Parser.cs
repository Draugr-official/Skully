using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CSharp;

namespace Skully.Compiler
{
    internal class Parser
    {
        string Source = "";
        public Parser(string source)
        {
            this.Source = source;
        }
           
        public SyntaxTree Parse()
        {
            return CSharpSyntaxTree.ParseText(this.Source);
        }
    }
}
