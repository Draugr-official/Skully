using Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Code_Generation.LLVM.Objects
{
    public class LLVMDataTypes
    {
        public static string 
            i8 = "i8", 
            i16 = "i16",
            i32 = "i32",
            i64 = "i64";

        public static Dictionary<DataTypes, string> CsToLLVM = new Dictionary<DataTypes, string>()
        {
            { DataTypes.Int, i32 }
        };
    }
}
