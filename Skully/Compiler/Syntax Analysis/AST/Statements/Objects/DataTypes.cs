using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements.Objects
{
    public struct DataType
    {
        public DataTypes Type = DataTypes.Unknown;
        public string Value = "";

        public DataType() { }

        public override string ToString()
        {
            if(this.Type == DataTypes.Other)
            {
                return this.Value;
            }
            else
            {
                return DataTypeInfo.ToString[this.Type];
            }
        }
    }

    public enum DataTypes
    {
        Unknown,
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
        public static new Dictionary<DataTypes, string> ToString = new Dictionary<DataTypes, string>()
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

        public static Dictionary<string, DataTypes> ToType = new Dictionary<string, DataTypes>()
        {
            { "bool", DataTypes.Boolean  },
            { "byte", DataTypes.Byte },
            {  "sbyte", DataTypes.Sbyte  },
            { "char", DataTypes.Char },
            { "decimal", DataTypes.Decimal },
            { "double", DataTypes.Double },
            { "float", DataTypes.Float },
            { "int", DataTypes.Int },
            { "uint", DataTypes.UInt },
            { "long", DataTypes.Long },
            { "ulong", DataTypes.ULong },
            { "object", DataTypes.Object },
            { "short", DataTypes.Short },
            { "ushort", DataTypes.UShort },
            { "string", DataTypes.String },
            { "var", DataTypes.Var },
            { "dynamic", DataTypes.Dynamic },
            { "void", DataTypes.Void },
        };
    }
}
