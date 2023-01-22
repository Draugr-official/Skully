using Mirage_Compiler.Compiler;
using Mirage_Compiler.Compiler.Code_Generation;
using Mirage_Compiler.Compiler.Code_Generation.ASM;
using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements;
using Mirage_Compiler.Compiler.SyntaxAnalysis;
using Mirage_Compiler.Compiler.PEBuilder;

namespace Mirage_Compiler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Lexer lexer = new Lexer();
            //List<LexToken> lexTokens = lexer.Analyze(File.ReadAllText("Tests/hello_world.cs"));

            //foreach(LexToken tokens in lexTokens)
            //{
            //    Console.WriteLine(tokens.Type + " " + tokens.Value);
            //}

            //Parser parser = new Parser(lexTokens);
            //List<Statement> statements = parser.ParseStatements();
            //Console.WriteLine(String.Join("\n", statements.Select(t => t.ToString()).ToList()));

            //CodeGen codeGen = new CodeGen(statements);
            //ASMContext ctx = codeGen.Generate();
            //Console.WriteLine(ctx.ToString());

            PE pe = new PE();
            pe.Build(@"; hello-world.ll

@string = private constant [15 x i8] c""Hello, world!\0A\00""

declare i32 @puts(i8*)

define i32 @main() {
  %address = getelementptr [15 x i8], [15 x i8]* @string, i64 0, i64 0
  call i32 @puts(i8* %address)
  ret i32 0
}");
            Console.ReadLine();
        }
    }
}