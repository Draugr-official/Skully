using Skully_Compiler.Compiler.Code_Generation.LLVM.AST.Expressions;
using Skully_Compiler.Compiler.Code_Generation.LLVM.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Code_Generation.LLVM.AST.Statements
{
    /// <summary>
    /// LLVM function declaration. define i32 @main() { ... }
    /// </summary>
    class LLVMFunctionStatement : LLVMStatement
    {
        /// <summary>
        /// Determines wether a declaration is local or global, % for local, @ for global.
        /// </summary>
        public bool isLocal = false;

        /// <summary>
        /// LLVM return type, e.g i32
        /// </summary>
        public string ReturnType = LLVMDataTypes.i32;

        /// <summary>
        /// LLVM function name, e.g main
        /// </summary>
        public string Name = "";

        /// <summary>
        /// LLVM function parameters, e.g @main(...)
        /// </summary>
        public List<LLVMVariableExpression> Parameters = new List<LLVMVariableExpression>();

        /// <summary>
        /// LLVM function body, e.g { ... }
        /// </summary>
        public List<LLVMStatement> Body = new List<LLVMStatement>();

        public override string ToString()
        {
            return $"define {this.ReturnType} {(this.isLocal ? "%" : "@")}{this.Name}({this.Parameters.Select(t => t.ToString())})\n" + "{\n" + "\n}";
        }
    }
}