using Skully_Compiler.Compiler;
using Skully_Compiler.Compiler.Code_Generation;
using Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements;
using Skully_Compiler.Compiler.SyntaxAnalysis;
using Skully_Compiler.Compiler.PEBuilder;
using System.Diagnostics;
using System.IO;
using Skully_Compiler.Compiler.Code_Generation.LLVM.Objects;
using Skully_Compiler.Compiler.Code_Generation.LLVM.AST.Statements;

namespace Skully_Compiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HandleCommands(new string[] { "build", "Tests/hello_world.cs", "-showast" });
            // HandleCommands(args);
        }

        static void HandleCommands(string[] args)
        {
            if(args.Length > 0)
            {
                switch (args[0])
                {
                    case "build":
                        {
                            List<Statement> statements = Build(args[1], args.Contains("-showast"));
                            break;
                        }

                    case "benchmark":
                        {
                            if (int.TryParse(args[2], out int count))
                            {
                                string Source = File.ReadAllText(args[1]);
                                Stopwatch sw = new Stopwatch();
                                sw.Start();

                                for (int i = 0; i < count; i++)
                                {
                                    BuildSource(Source);
                                }

                                sw.Stop();
                                DebugOut.Success($"Ran build {count} times, took {sw.ElapsedMilliseconds}ms");
                            }
                            break;
                        }

                    default:
                        {
                            DebugOut.Error($"Command '{args[0]}' does not exist");
                            break;
                        }
                }
            }
            else
            {
                DebugOut.Info($"Welcome to Skully! Check out https://github.com/Draugr-official/Skully to learn how to use this compiler.");
            }
        }

        static List<Statement> Build(string path, bool debug = false)
        {
            return BuildSource(File.ReadAllText(path), debug);
        }

        static List<Statement> BuildSource(string src, bool debug = false)
        {
            // Generate lexical tokens
            Lexer lexer = new Lexer();
            List<LexToken> lexTokens = lexer.Analyze(src);
            DebugOut.Info("Generated lex tokens");

            // Create abstract syntax tree
            Parser parser = new Parser(lexTokens);
            List<Statement> statements = parser.ParseStatements();

            if(debug)
            {
                DebugOut.Info("Abstact syntax tree:");
                Console.WriteLine(String.Join("\n", statements.Select(t => t.ToString()).ToList()));
            }
            DebugOut.Info("Generated AST");

            CodeGen codeGen = new CodeGen(statements);
            List<LLVMStatement> LLVMStatements = codeGen.Generate();

            if (debug)
            {
                DebugOut.Info("LLVM:");
                DebugOut.Info($"{LLVMStatements.Count} elements");
                Console.WriteLine(String.Join("\n", LLVMStatements.Select(t => t.ToString())));
            }
            DebugOut.Info("Generated LLVM");

            return statements;
        }
    }
}