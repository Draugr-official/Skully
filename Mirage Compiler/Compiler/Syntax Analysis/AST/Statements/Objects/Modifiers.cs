using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements.Objects
{
    public enum Modifiers
    {
        Public,
        Private,
        Protected,
        Internal,
        Static,
        Readonly,
        Const,
        New,
        Abstract,
        Sealed,
        Virtual,
        Override,
        Extern,
        Unsafe,
        Volatile,
        Async,
    }

    public class ModifierInfo
    {
        public static Dictionary<Modifiers, string> Names = new Dictionary<Modifiers, string>() 
        {
            { Modifiers.Public, "public" },
            { Modifiers.Private, "private" },
            { Modifiers.Protected, "protected" },
            { Modifiers.Internal, "internal" },
            { Modifiers.Static, "static" },
            { Modifiers.Readonly, "readonly" },
            { Modifiers.Const, "const" },
            { Modifiers.New, "new" },
            { Modifiers.Abstract, "abstract" },
            { Modifiers.Sealed, "sealed" },
            { Modifiers.Virtual, "virtual" },
            { Modifiers.Override, "override" },
            { Modifiers.Extern, "extern" },
            { Modifiers.Unsafe, "unsafe" },
            { Modifiers.Volatile, "volatile" },
            { Modifiers.Async, "async" },
        };

        public static string ListToString(List<Modifiers> modifiers)
        {
            List<string> modifierNames = new List<string>();
            
            foreach(Modifiers modifier in modifiers)
            {
                modifierNames.Add(Names[modifier]);
            }

            return string.Join(" ", modifierNames);
        }
    }
}
