using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skully_Compiler.Compiler.Code_Generation.LLVM.AST.Expressions;

namespace Skully_Compiler.Compiler.Code_Generation.LLVM.AST.Statements
{
    /// <summary>
    /// LLVM variable declaration. %hi = add i8 4, 5
    /// </summary>
    class LLVMVariableStatement : LLVMStatement
    {
        public bool isLocal = false;
        public string Name = "";
        public LLVMExpression Assign = new LLVMExpression();

        public override string ToString()
        {
            return "variable " + this.Name;
        }
    }
}
