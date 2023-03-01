using Skully.Compiler.CodeGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully
{
    internal class CLI
    {
        static Dictionary<string, Command> Commands = new Dictionary<string, Command>() {
            { "build", new Command()
                {
                    Name = "build",
                    Description = "Builds the project",
                    Action = (string[] args) =>
                    {
                        CodeGenConfig config = new CodeGenConfig()
                        {
                            Name = Path.GetFileNameWithoutExtension(args[0]),
                            Build = true
                        };
                        CodeGenerator codeGenerator = new CodeGenerator(args[0], config);
                        codeGenerator.GenerateLLVM();

                        if(!Debug.HasError)
                        {
                            ProcessStartInfo clang = new ProcessStartInfo()
                            {
                                FileName = @"C:\Windows\system32\cmd.exe",
                                Arguments = $"/c clang out.ll -Os -o {config.Name}.exe",
                            };
                            Process.Start(clang).WaitForExit();

                            Debug.Success($"Built app to {config.Name}.exe");
                        }

                    }
                }
            },
            { "test", new Command()
                {
                    Name = "test",
                    Description = "Tests the project and displays any errors",
                    Action = (string[] args) =>
                    {
                        CodeGenConfig config = new CodeGenConfig()
                        {
                            Name = Path.GetFileNameWithoutExtension(args[0]),
                            Build = false
                        };
                        CodeGenerator codeGenerator = new CodeGenerator(args[0], config);
                        codeGenerator.GenerateLLVM();
                    }
                }
            },
            { "help", new Command() 
                {
                    Name = "help",
                    Description = "Displays a list of commands",
                    Action = (string[] args) =>
                    {
                        Console.WriteLine("Skully - A modern AOT C# compiler\n");
                        Console.WriteLine("Usage: skully [COMMAND] [FILE NAME] [OPTIONS]\n");
                        Console.WriteLine("Commands:");

                        foreach(Command command in Commands.Values)
                        {
                            Console.Write($"    {command.Name}\t");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(command.Description);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                }
            }
        };

        public static void Parse(string[] args)
        {
            if (args.Length == 0)
            {
                Debug.Error("No arguments provided", "Please run the compiler through the command line with arguments");
                return;
            }

            Commands[args[0]].Run(args.ToList().Skip(1).ToArray());
        }
    }

    class Command
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Action<string[]> Action { get; set; }

        public Command(string name, string description, Action<string[]> action)
        {
            this.Name = name;
            this.Description = description;
            this.Action = action;
        }

        public Command()
        {
            this.Name = "";
            this.Description = "";
            this.Action = (string[] arg) => { };
        }

        public void Run(string[] arguments)
        {
            this.Action(arguments);
        }
    }
}
