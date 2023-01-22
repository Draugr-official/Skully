using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.Data
{
    internal class DataDirective
    {
        public string Label = "";
        public DefinedDirectives DataType = DefinedDirectives.Byte;
        public string Initial = "";

        public DataDirective(string label, DefinedDirectives dataType, string initial)
        {
            Label = label;
            DataType = dataType;
            Initial = initial;
        }

        public DataDirective() { }

        public override string ToString()
        {
            return $"{this.Label} {DDLookup[this.DataType] }";
        }

        Dictionary<DefinedDirectives, string> DDLookup = new Dictionary<DefinedDirectives, string>()
        {
            { DefinedDirectives.Unknown, "Unknown" },
            { DefinedDirectives.Byte, "db" },
            { DefinedDirectives.Word, "dw" },
            { DefinedDirectives.Dword, "dd" },
            { DefinedDirectives.Qword, "dq" },
            { DefinedDirectives.Tbyte, "dt" }
        };
    }

    /// <summary>
    /// Directives for initialized data
    /// </summary>
    enum DefinedDirectives
    {
        /// <summary>
        /// Unknown directive
        /// </summary>
        Unknown,

        /// <summary>
        /// One byte, 'db'
        /// </summary>
        Byte,

        /// <summary>
        /// Two bytes, 'dw'
        /// </summary>
        Word,

        /// <summary>
        /// Four bytes, 'dd'
        /// </summary>
        Dword,

        /// <summary>
        /// Eight bytes, 'dq'
        /// </summary>
        Qword,

        /// <summary>
        /// Ten bytes, 'dt'
        /// </summary>
        Tbyte,
    }
}
