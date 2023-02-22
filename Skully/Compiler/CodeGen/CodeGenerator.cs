using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LLVMSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Skully.Compiler.CodeGen
{
    internal class CodeGenerator
    {
        CodeGenConfig Config { get; set; }
        SyntaxTree AST { get; set; }

        public LLVMModuleRef module;
        public LLVMBuilderRef builder;

        string EntryFile = "";

        bool MainExists = false;

        public CodeGenerator(string file, CodeGenConfig config)
        {
            this.EntryFile = file;
            this.Config = config;
            this.module = LLVM.ModuleCreateWithName(config.Name);
            this.AST = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
        }

        public void GenerateLLVM()
        {
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86AsmPrinter();

            // Assign module and builder
            this.module = LLVM.ModuleCreateWithName(this.Config.Name);
            builder = LLVM.CreateBuilder();

            Objects.Std standardLibrary = new Objects.Std(this);
            standardLibrary.Generate();

            // Generate LLVM
            CompilationUnitSyntax compilationUnit = this.AST.GetCompilationUnitRoot();
            GenerateSyntaxNodes(compilationUnit.ChildNodes());
            
            if (!MainExists)
            {
                Debug.Error("You're missing an entry point", "Add a `Main` method to the file", this.EntryFile);
            }

            //if (LLVM.CreateMemoryBufferWithContentsOfFile("cs-stl.ll", out LLVMMemoryBufferRef llvmBuffer, out string error))
            //{
            //    Debug.Error(error);
            //}
            //else
            //{
            //    LLVMContextRef llvmContext = LLVM.GetGlobalContext();
            //    if (LLVM.ParseBitcodeInContext(llvmContext, llvmBuffer, out LLVMModuleRef llvmModule, out string parseError))
            //    {
            //        Debug.Error(parseError);
            //    }
            //    else
            //    {
            //        // ...
            //    }
            //}
            
            if(!Debug.HasError)
            {
                LLVM.DisposeBuilder(builder);
                LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMReturnStatusAction, out var message);
                if (!string.IsNullOrEmpty(message))
                {
                    LLVM.DumpModule(module);
                    Debug.Error(message);
                }
                if (this.Config.Build)
                {
                    LLVM.PrintModuleToFile(module, "out.ll", out string ErrorMessage);
                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        Debug.Error(ErrorMessage);
                    }
                }
            }
        }


        void GenerateClassDeclaration(ClassDeclarationSyntax classDeclaration) // NOT FINSIHED
        {
            GenerateSyntaxNodes(classDeclaration.ChildNodes());
        }
        void GenerateMethodDeclaration(MethodDeclarationSyntax methodDeclaration)
        {
            string methodName = methodDeclaration.Identifier.Text;
            if(methodName == "Main")
            {
                MainExists = true;
                methodName = methodName.ToLower();
            }

            var funcType = LLVM.FunctionType(LLVM.VoidType(), new LLVMTypeRef[] { LLVM.PointerType(LLVM.Int8Type(), 0) }, false);
            var func = LLVM.AddFunction(module, methodName, funcType);
            var block = LLVM.AppendBasicBlock(func, "entry");
            LLVM.PositionBuilderAtEnd(builder, block);

            if(methodDeclaration.Body != null)
            {
                GenerateSyntaxNodes(methodDeclaration.Body.ChildNodes());
            }

            LLVM.BuildRetVoid(builder);
        }

        void GenerateIfStatement(IfStatementSyntax ifStatement)
        {
            GenerateSyntaxNodes(ifStatement.ChildNodes());
        }
        
        void GenerateSyntaxNodes(IEnumerable<SyntaxNode> syntaxNodes)
        {
            foreach (SyntaxNode node in syntaxNodes)
            {
                GenerateSyntaxNode(node);
            }
        }

        void GenerateSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode != null)
            {
                switch (syntaxNode.Kind())
                {
                    case SyntaxKind.ClassDeclaration:
                        {
                            GenerateClassDeclaration((ClassDeclarationSyntax)syntaxNode);
                            break;
                        }

                    case SyntaxKind.MethodDeclaration:
                        {
                            GenerateMethodDeclaration((MethodDeclarationSyntax)syntaxNode);
                            break;
                        }

                    case SyntaxKind.ExpressionStatement:
                        {
                            GenerateExpressionStatement((ExpressionStatementSyntax)syntaxNode);
                            break;
                        }

                    case SyntaxKind.IfStatement:
                        {
                            break;
                        }
                }
            }
        }

        LLVMValueRef GenerateLiteralExpressiona(LiteralExpressionSyntax literalExpression)
        {
            switch (literalExpression.Kind())
            {
                case SyntaxKind.FalseLiteralExpression:
                    {
                        return LLVM.ConstInt(LLVM.Int1Type(), 0, false);
                    }

                case SyntaxKind.TrueLiteralExpression:
                    {
                        return LLVM.ConstInt(LLVM.Int1Type(), 1, false);
                    }

                case SyntaxKind.NumericLiteralExpression:
                    {
                        return LLVM.ConstInt(LLVM.Int32Type(), uint.Parse(literalExpression.Token.ValueText), false);
                    }

                case SyntaxKind.CharacterLiteralExpression:
                    {
                        return LLVM.ConstInt(LLVM.Int8Type(), (uint)literalExpression.Token.ValueText[0], false);
                    }

                case SyntaxKind.StringLiteralExpression:
                    {
                        return LLVM.ConstStringInContext(LLVM.GetGlobalContext(), literalExpression.Token.ValueText, (uint)literalExpression.Token.ValueText.Length, true);
                    }
            }

            return LLVM.ConstInt(LLVM.Int32Type(), 0, false);
        }

        void GenerateExpressionStatement(ExpressionStatementSyntax expressionStatement)
        {
            GenerateExpression(expressionStatement.Expression);
        }

        LLVMValueRef GenerateExpression(SyntaxNode syntaxNode)
        {
            switch(syntaxNode.Kind())
            {
                case SyntaxKind.FalseLiteralExpression:
                case SyntaxKind.TrueLiteralExpression:
                case SyntaxKind.NumericLiteralExpression:
                case SyntaxKind.CharacterLiteralExpression:
                case SyntaxKind.StringLiteralExpression:
                    {
                        return GenerateLiteralExpression((LiteralExpressionSyntax)syntaxNode);
                    }

                case SyntaxKind.InvocationExpression:
                    {
                        return GenerateInvocationExpression((InvocationExpressionSyntax)syntaxNode);
                    }
            }

            return LLVM.ConstNull(LLVM.Int8Type());
        }
        
        LLVMValueRef GenerateNumericExpression(LiteralExpressionSyntax numericExpression) // TODO: Add all numeric types
        {
            LLVMTypeRef type = LLVM.Int32Type();
            
            switch(numericExpression.Token.Value)
            {
                case Int16: type = LLVM.Int16Type(); break;
                case Int32: type = LLVM.Int32Type(); break;
                case Int64: type = LLVM.Int64Type(); break;
                case Byte: type = LLVM.Int8Type(); break;
            }

            return LLVM.ConstInt(type, Convert.ToUInt64(numericExpression.Token.Value), false);
        }
        
        LLVMValueRef GenerateStringExpression(LiteralExpressionSyntax stringExpression)
        {
            //string literalValue = stringExpression.GetText().ToString();
            //literalValue = literalValue.Substring(1, literalValue.Length - 2);
            //var str = LLVM.BuildGlobalString(builder, literalValue, "");
            //return LLVM.ConstGEP(str, new LLVMValueRef[] { LLVM.ConstInt(LLVM.Int32Type(), 0, false), LLVM.ConstInt(LLVM.Int32Type(), 0, false) });
            return LLVM.ConstStringInContext(LLVM.GetGlobalContext(), stringExpression.Token.ValueText, (uint)stringExpression.Token.ValueText.Length, true);
        }
        
        LLVMValueRef GenerateLiteralExpression(LiteralExpressionSyntax literalExpression)
        {
            switch (literalExpression.Kind())
            {
                case SyntaxKind.StringLiteralExpression:
                    {
                        return GenerateStringExpression(literalExpression);
                    }
                case SyntaxKind.NumericLiteralExpression:
                    {
                        return GenerateNumericExpression(literalExpression);
                    }
                    
            }
            return LLVM.ConstNull(LLVM.Int8Type());
        }

        LLVMValueRef GenerateInvocationExpression(InvocationExpressionSyntax invocationExpression) // WORK ON THIS
        {
            string invocationName = invocationExpression.Expression.ToString();

            var invocationType = LLVM.FunctionType(LLVM.Int32Type(), new LLVMTypeRef[] { LLVM.PointerType(LLVM.Int8Type(), 0) }, true);
            var invocationFunc = LLVM.GetNamedFunction(module, invocationName).Pointer == IntPtr.Zero ? LLVM.AddFunction(module, invocationName, invocationType) : LLVM.GetNamedFunction(module, invocationName);

            //var str = LLVM.BuildGlobalString(builder, "Hello, world!\n", "");
            //var format = LLVM.ConstGEP(str, new LLVMValueRef[] { LLVM.ConstInt(LLVM.Int32Type(), 0, false), LLVM.ConstInt(LLVM.Int32Type(), 0, false) });

            var call = LLVM.BuildCall(builder, invocationFunc, ArgumentBuilder(invocationExpression.ArgumentList), "");
            return call;
        }

        LLVMValueRef[] ArgumentBuilder(ArgumentListSyntax argumentList)
        {
            List<LLVMValueRef> llvmValueRefs = new List<LLVMValueRef>();
            foreach(ArgumentSyntax argument in argumentList.Arguments)
            {
                llvmValueRefs.Add(GenerateExpression(argument.Expression));
            }

            return llvmValueRefs.ToArray();
        }
    }
}
