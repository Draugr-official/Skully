using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements.Objects
{
    public struct DataType
    {
        public DataTypes Type;
        public string Value;

        public override string ToString()
        {
            if(this.Type == DataTypes.Other)
            {
                return this.Value;
            }
            else
            {
                return DataTypeInfo.Names[this.Type];
            }
        }
    }

    public enum DataTypes
    {
        Boolean,
        Byte,
        Sbyte,
        Char,
        Decimal,
        Double,
        Float,
        Int,
        UInt,
        Long,
        ULong,
        Object,
        Short,
        UShort,
        String,
        Var,
        Dynamic,
        Void,
        Other
    }

    public class DataTypeInfo
    {
        public static Dictionary<DataTypes, string> Names = new Dictionary<DataTypes, string>()
        {
            { DataTypes.Boolean, "bool" },
            { DataTypes.Byte, "byte" },
            { DataTypes.Sbyte, "sbyte" },
            { DataTypes.Char, "char" },
            { DataTypes.Decimal, "decimal" },
            { DataTypes.Double, "double" },
            { DataTypes.Float, "float" },
            { DataTypes.Int, "int" },
            { DataTypes.UInt, "uint" },
            { DataTypes.Long, "long" },
            { DataTypes.ULong, "ulong" },
            { DataTypes.Object, "object" },
            { DataTypes.Short, "short" },
            { DataTypes.UShort, "ushort" },
            { DataTypes.String, "string" },
            { DataTypes.Var, "var" },
            { DataTypes.Dynamic, "dynamic" },
            { DataTypes.Void, "void" },
        };
    }
}
