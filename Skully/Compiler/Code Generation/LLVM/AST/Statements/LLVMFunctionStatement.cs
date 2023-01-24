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
    struct LLVMFunctionStatement : LLVMStatement
    {
        /// <summary>
        /// Determines wether a declaration is local or global, % for local, @ for global.
        /// </summary>
        public bool isLocal { get; set; }

        /// <summary>
        /// LLVM return type, e.g i32
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// LLVM function name, e.g main
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// LLVM function parameters, e.g @main(...)
        /// </summary>
        public List<LLVMVariableStatement> Parameters { get; set; }

        /// <summary>
        /// LLVM function body, e.g { ... }
        /// </summary>
        public List<LLVMStatement> Body { get; set; }

        public override string ToString()
        {
            return $"define {this.ReturnType} {this.Name}({string.Join(", ", this.Parameters.Select(t => t.ToString()))})\n" + "{\n" + string.Join("\n", this.Body.Select(t => t.ToString())) + "\n}";
        }
    }
}
