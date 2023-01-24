using Skully_Compiler.Compiler;
using Skully_Compiler.Compiler.Code_Generation;
using Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements;
using Skully_Compiler.Compiler.SyntaxAnalysis;
using Skully_Compiler.Compiler.PEBuilder;
using System.Diagnostics;

namespace Skully_Compiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //HandleCommands(new string[] { "build", "Tests/hello_world.cs", "-showast" });
            HandleCommands(args);
        }

        static void HandleCommands(string[] args)
        {
            switch (args[0])
            {
                case "build":
                    {
                        List<Statement> statements = Build(args[1]);
                        if (args.Contains("-showast"))
                        {
                            Console.WriteLine(String.Join("\n", statements.Select(t => t.ToString()).ToList()));
                        }
                        break;
                    }

                case "benchmark":
                    {
                        if (int.TryParse(args[2], out int count))
                        {
                            Stopwatch sw = new Stopwatch();
                            sw.Start();

                            for(int i = 0; i < count; i++)
                            {
                                Build(args[1]);
                            }

                            sw.Stop();
                            DebugOut.Success($"Ran build {count} times, took {sw.ElapsedMilliseconds}ms");
                        }
                        break;
                    }
            }
        }

        static List<Statement> Build(string path)
        {
            // Generate lexical tokens
            Lexer lexer = new Lexer();
            List<LexToken> lexTokens = lexer.Analyze(File.ReadAllText(path));

            // Create abstract syntax tree
            Parser parser = new Parser(lexTokens);
            List<Statement> statements = parser.ParseStatements();

            return statements;
        }
    }
}