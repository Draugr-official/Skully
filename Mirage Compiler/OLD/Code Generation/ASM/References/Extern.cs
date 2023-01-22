using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM.References
{
    internal class Extern
    {
        public string Name = "";

        public Extern(string name)
        {
            Name = name;
        }

        public Extern() { }

        public override string ToString()
        {
            return $"extern {this.Name}";
        }
    }
}
