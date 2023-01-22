using Mirage_Compiler.Compiler.Code_Generation.ASM;
using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements;
using Mirage_Compiler.Compiler.Code_Generation.ASM.Instructions;
using Mirage_Compiler.Compiler.Code_Generation.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirage_Compiler.Compiler.Code_Generation.ASM.Registers;
using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Expressions;

namespace Mirage_Compiler.Compiler.Code_Generation
{
    internal class CodeGenOLD
    {
        List<Statement> Statements = new List<Statement>();
        ASMContext Context = new ASMContext();

        public CodeGenOLD(List<Statement> statements)
        {
            this.Statements = statements;
        }

        public ASMContext Generate()
        {
            Context.AddExtern("ExitProcess");
            GenerateStatements(Statements);

            Context.AddInstruction(new Xor(Registers.ECX, Registers.ECX));
            Context.AddInstruction(new Call("ExitProcess"));
            Context.AddInstruction(new Hlt());

            return this.Context;
        }

        void GenerateStatements(List<Statement> statements)
        {
            foreach(Statement statement in statements)
            {
                GenerateStatement(statement);
            }
        }

        void GenerateStatement(Statement statement)
        {
            if (statement is NamespaceStatement namespaceStatement)
            {
                GenerateNamespaceStatement(namespaceStatement);
            }

            if(statement is ClassStatement classStatement)
            {
                GenerateClassStatement(classStatement);
            }

            if (statement is MethodStatement methodStatement)
            {
                GenerateMethodStatement(methodStatement);
            }

            if(statement is MethodCallStatement methodCallStatement)
            {
                GenerateMethodCallStatement(methodCallStatement);
            }

            if (statement is BlockStatement blockStatement)
            {
                GenerateBlockStatement(blockStatement);
            }
        }

        void GenerateMethodCallStatement(MethodCallStatement methodCallStatement)
        {
            Console.WriteLine("GRRRRRRRRRRRRAHHHHHHHHHHHHHHHHHHHHHHHHHHH: ");
            if (WinApiLookup.Lookups.ContainsKey(methodCallStatement.Name))
            {
                //WinApiLookup.Lookups[methodCallStatement.Name].AddCall(this.Context, methodCallStatement.Arguments);

                foreach (Expression arg in methodCallStatement.Arguments)
                {
                    if(arg is LiteralExpression literalExpression)
                    {
                        Context.AddDataDirective(new ASM.Data.DataDirective($"{methodCallStatement.Name}_arg{Context.DataSection.Count}", ASM.Data.DefinedDirectives.Byte, $"'{literalExpression.Value}', 0"));
                    }
                }
            }
            else
            {
                Console.WriteLine("This does not exist!");
            }
        }

        void GenerateMethodStatement(MethodStatement methodStatement)
        {
            Context.AddInstruction(new Label(methodStatement.Name));

            GenerateStatement(methodStatement.Body);

            Context.AddInstruction(new Ret());
        }

        void GenerateClassStatement(ClassStatement classStatement)
        {
            GenerateStatement(classStatement.Body);
        }

        void GenerateNamespaceStatement(NamespaceStatement namespaceStatement)
        {
            GenerateStatement(namespaceStatement.Body);
        }

        void GenerateBlockStatement(BlockStatement blockStatement)
        {
            GenerateStatements(blockStatement.Statements);
        }
    }
}
