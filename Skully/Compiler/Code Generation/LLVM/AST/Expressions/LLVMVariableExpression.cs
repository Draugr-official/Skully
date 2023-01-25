using Skully_Compiler.Compiler.Code_Generation.LLVM.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Code_Generation.LLVM.AST.Expressions
{
    internal class LLVMVariableExpression : LLVMExpression
    {
        public string Name = "";
        public string ReturnType = LLVMDataTypes.i32;
    }
}
