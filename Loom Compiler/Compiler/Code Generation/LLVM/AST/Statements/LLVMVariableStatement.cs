using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirage_Compiler.Compiler.Code_Generation.LLVM.AST.Expressions;

namespace Mirage_Compiler.Compiler.Code_Generation.LLVM.AST.Statements
{
    /// <summary>
    /// LLVM variable declaration. %hi = add i8 4, 5
    /// </summary>
    struct LLVMVariableStatement : LLVMStatement
    {
        public bool isLocal { get; set; }
        public string Name { get; set; }
        public LLVMExpression Assign { get; set; }

        public override string ToString()
        {
            return (this.isLocal ? "%" : "@") + this.Name + " = " + this.Assign.ToString();
        }
    }
}
