using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions;
using Mirage_Compiler.Compiler.Code_Generation.ASM.Data;
using Mirage_Compiler.Compiler.Code_Generation.ASM.References;

namespace Mirage_Compiler.Compiler.Code_Generation.ASM
{
    internal class ASMContext
    {
        List<string> Labels = new List<string>();
        public List<DataDirective> DataSection = new List<DataDirective>();
        public List<Instruction> TextSection = new List<Instruction>();
        public List<Extern> Externs = new List<Extern>();

        public void AddInstruction(Instruction instruction)
        {
            TextSection.Add(instruction);
        }

        public void AddDataDirective(DataDirective dataDirective)
        {
            this.DataSection.Add(dataDirective);
        }

        /// <summary>
        /// If extern has not previously been added, references the target
        /// </summary>
        /// <param name="name"></param>
        public void AddExtern(string name)
        {
            if(!Externs.Select(t => t.Name).Contains(name))
            {
                this.Externs.Add(new Extern(name));
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            

            sb.AppendLine("global WinMain");
            foreach(Extern Extern in this.Externs)
            {
                sb.AppendLine(Extern.ToString());
            }

            sb.AppendLine("section .data");
            foreach(DataDirective dataDirective in this.DataSection)
            {
                sb.AppendLine(dataDirective.ToString());
            }

            sb.AppendLine("section .text");
            foreach(Instruction instruction in this.TextSection)
            {
                sb.AppendLine(instruction.ToString());
            }

            return sb.ToString();
        }
    }
}
