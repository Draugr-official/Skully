using Mirage_Compiler.Compiler.Code_Generation.Windows.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.Windows
{
    internal class WinApiLookup
    {
        public static Dictionary<string, KernelFunction> Lookups = new Dictionary<string, KernelFunction>()
        {
            { MessageBoxA.CsName, new MessageBoxA() }
        };
    }
}
