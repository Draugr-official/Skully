using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
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

        public LLVMModuleRef Module;
        public LLVMBuilderRef Builder;
        
        List<LLVMModuleRef> Imports = new List<LLVMModuleRef>();

        const int BOM_SIZE = 3;

        string EntryFile = "";

        bool MainExists = false;

        public CodeGenerator(string file, CodeGenConfig config)
        {
            this.EntryFile = file;
            this.Config = config;
            this.Module = LLVM.ModuleCreateWithName(config.Name);
            this.AST = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
            this.variableTable = new Dictionary<string, LLVMValueRef>();
        }

        public void ImportFile(string filePath)
        {
            // Debug.Log(File.ReadAllText(filePath));
            byte[] inputData = File.ReadAllBytes(filePath);
            IntPtr inputDataPtr = Marshal.AllocHGlobal(inputData.Length);
            Marshal.Copy(inputData, BOM_SIZE, inputDataPtr, inputData.Length - BOM_SIZE);

            LLVMMemoryBufferRef llvmBuffer = LLVM.CreateMemoryBufferWithMemoryRangeCopy(inputDataPtr, inputData.Length - BOM_SIZE, "buffer");
            if (LLVM.ParseIRInContext(LLVM.GetModuleContext(this.Module), llvmBuffer, out LLVMModuleRef module, out string parseIRErrorMsg))
            {
                Debug.Error(parseIRErrorMsg, "Error at LLVM.ParseIRInContext");
            }
            else
            {
                if (LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMReturnStatusAction, out var verifyErrorMsg))
                {
                    Debug.Error(verifyErrorMsg, "Error at LLVM.VerifyModule");
                }
                else
                {
                    LLVM.LinkModules2(this.Module, module);
                    Imports.Add(module);
                    // this.Module = module;
                }
            }
        }

        public void GenerateLLVM()
        {
            LLVM.InitializeX86Target();
            LLVM.InitializeX86TargetInfo();
            LLVM.InitializeX86TargetMC();
            LLVM.InitializeX86AsmPrinter();

            // Assign Module and Builder
            this.Module = LLVM.ModuleCreateWithName(this.Config.Name);
            this.Builder = LLVM.CreateBuilder();

            ImportFile(Environment.CurrentDirectory + "\\StandardLibrary\\system.ll");

            // Generate LLVM
            CompilationUnitSyntax compilationUnit = this.AST.GetCompilationUnitRoot();
            GenerateSyntaxNodes(compilationUnit.ChildNodes());
            
            if (LLVM.GetNamedFunction(this.Module, "main").Pointer == IntPtr.Zero)
            {
                Debug.Error("You're missing an entry point", "Add a `Main` method to the file", this.EntryFile);
            }

            if (!Debug.HasError)
            {
                LLVM.DisposeBuilder(Builder);
                LLVM.VerifyModule(Module, LLVMVerifierFailureAction.LLVMReturnStatusAction, out var message);
                if (!string.IsNullOrEmpty(message))
                {
                    LLVM.DumpModule(Module);
                    Debug.Error(message);
                }
                if (this.Config.Build)
                {
                    LLVM.PrintModuleToFile(Module, "out.ll", out string ErrorMessage);
                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        Debug.Error(ErrorMessage);
                    }
                }
            }
        }

        private Dictionary<string, LLVMValueRef> variableTable;
        
        private void DeclareVariable(string variableName, LLVMValueRef variableValue)
        {
            this.variableTable[variableName] = variableValue;
        }
        
        private LLVMValueRef GetVariable(string variableName)
        {
            if (this.variableTable.ContainsKey(variableName))
            {
                return this.variableTable[variableName];
            }
            else
            {
                return LLVM.ConstNull(LLVM.Int32Type());
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
            var func = LLVM.AddFunction(Module, methodName, funcType);
            var block = LLVM.AppendBasicBlock(func, "");
            LLVM.PositionBuilderAtEnd(Builder, block);

            if(methodDeclaration.Body != null)
            {
                GenerateSyntaxNodes(methodDeclaration.Body.ChildNodes());
            }

            LLVM.BuildRetVoid(Builder);
        }

        void GenerateIfStatement(IfStatementSyntax ifStatement)
        {
            LLVMValueRef conditionValue = GenerateExpression(ifStatement.Condition);
            LLVMValueRef function = LLVM.GetBasicBlockParent(LLVM.GetInsertBlock(Builder));

            LLVMBasicBlockRef thenBB = LLVM.AppendBasicBlock(function, ""); // if-then
            LLVMBasicBlockRef elseBB = LLVM.AppendBasicBlock(function, ""); // if-else
            LLVMBasicBlockRef endBB = LLVM.AppendBasicBlock(function, "");   // if-end

            if (ifStatement.Else == null)
            {
                elseBB.DeleteBasicBlock();
                LLVM.BuildCondBr(Builder, conditionValue, thenBB, endBB);
            }
            else
            {
                LLVM.BuildCondBr(Builder, conditionValue, thenBB, elseBB);
                LLVM.PositionBuilderAtEnd(Builder, elseBB);
                GenerateSyntaxNodes(ifStatement.Else.Statement.ChildNodes());
                LLVM.BuildBr(Builder, endBB);
            }

            LLVM.PositionBuilderAtEnd(Builder, thenBB);
            GenerateSyntaxNodes(ifStatement.Statement.ChildNodes());
            LLVM.BuildBr(Builder, endBB);

            LLVM.PositionBuilderAtEnd(Builder, endBB);
        }

        void GenerateWhileStatement(WhileStatementSyntax whileStatement)
        {
            var function = LLVM.GetBasicBlockParent(LLVM.GetInsertBlock(Builder));

            LLVMBasicBlockRef conditionBB = LLVM.AppendBasicBlock(function, "");
            LLVMBasicBlockRef bodyBB = LLVM.AppendBasicBlock(function, "");
            LLVMBasicBlockRef endBB = LLVM.AppendBasicBlock(function, "");

            LLVM.BuildBr(Builder, conditionBB);
            LLVM.PositionBuilderAtEnd(Builder, conditionBB);
            LLVMValueRef conditionValue = GenerateExpression(whileStatement.Condition);
            LLVM.BuildCondBr(Builder, conditionValue, bodyBB, endBB);

            LLVM.PositionBuilderAtEnd(Builder, bodyBB);
            GenerateSyntaxNodes(whileStatement.Statement.ChildNodes());
            LLVM.BuildBr(Builder, conditionBB);

            LLVM.PositionBuilderAtEnd(Builder, endBB);
        }

        void GenerateDeclarationStatement(LocalDeclarationStatementSyntax localDeclarationStatement)
        {
            var variableType = localDeclarationStatement.Declaration.Type.ToString();
            foreach (var variable in localDeclarationStatement.Declaration.Variables)
            {
                var variableName = variable.Identifier.Text;
                var variableValue = LLVM.ConstInt(LLVM.Int32Type(), 0, false);
                if (variable.Initializer != null)
                {
                    variableValue = GenerateExpression(variable.Initializer.Value);
                }
                DeclareVariable(variableName, variableValue);
            }
        }
        
        List<string> Indentation = new List<string>();
        void GenerateSyntaxNodes(IEnumerable<SyntaxNode> syntaxNodes)
        {
            Indentation.Add("\t");
            foreach (SyntaxNode node in syntaxNodes)
            {
                Console.WriteLine(string.Join("", this.Indentation) + node.Kind());
                if(node.IsKind(SyntaxKind.ExpressionStatement))
                {
                    Console.WriteLine(string.Join("", this.Indentation) + "ExpressionStatement > " + ((ExpressionStatementSyntax)node).Expression.Kind());
                }
                GenerateSyntaxNode(node);
            }
            Indentation.RemoveAt(0);
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
                            GenerateIfStatement((IfStatementSyntax)syntaxNode);
                            break;
                        }

                    case SyntaxKind.WhileStatement:
                        {
                            GenerateWhileStatement((WhileStatementSyntax)syntaxNode);
                            break;
                        }

                    case SyntaxKind.LocalDeclarationStatement:
                        {
                            GenerateDeclarationStatement((LocalDeclarationStatementSyntax)syntaxNode);
                            break;
                        }
                }
            }
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
                case SyntaxKind.NullLiteralExpression:
                    {
                        return GenerateLiteralExpression((LiteralExpressionSyntax)syntaxNode);
                    }

                case SyntaxKind.InvocationExpression:
                    {
                        return GenerateInvocationExpression((InvocationExpressionSyntax)syntaxNode);
                    }

                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.GreaterThanEqualsToken:
                case SyntaxKind.LessThanEqualsToken:
                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.NotEqualsExpression:
                    {
                        return GenerateRelationalExpression((BinaryExpressionSyntax)syntaxNode);
                    }

                case SyntaxKind.AddExpression:
                case SyntaxKind.SubtractExpression:
                case SyntaxKind.MultiplyExpression:
                case SyntaxKind.DivideExpression:
                case SyntaxKind.ModuloExpression:
                    {
                        return GenerateArithmeticExpression((BinaryExpressionSyntax)syntaxNode);
                    }

                case SyntaxKind.IdentifierName:
                    {
                        return GenerateIdentifierExpression((IdentifierNameSyntax)syntaxNode);
                    }

                case SyntaxKind.SimpleAssignmentExpression:
                    {
                        return GenerateAssignmentExpression((AssignmentExpressionSyntax)syntaxNode);
                    }
            }

            throw Debug.Error($"{syntaxNode.Kind()} is not a supported expression", "Create an issue at https://github.com/Draugr-official/Skully if you believe this is wrong");
        }

        LLVMValueRef GenerateArithmeticExpression(BinaryExpressionSyntax binaryExpression)
        {
            switch (binaryExpression.Kind())
            {
                case SyntaxKind.AddExpression:
                    {
                        LLVMValueRef left = GenerateExpression(binaryExpression.Left);
                        LLVMValueRef right = GenerateExpression(binaryExpression.Right);
                        return LLVM.BuildAdd(Builder, left, right, "");
                    }
                case SyntaxKind.SubtractExpression:
                    {
                        LLVMValueRef left = GenerateExpression(binaryExpression.Left);
                        LLVMValueRef right = GenerateExpression(binaryExpression.Right);
                        return LLVM.BuildSub(Builder, left, right, "");
                    }
            }

            throw Debug.Error($"{binaryExpression.Kind()} is not a supported binary operation", "Create an issue at https://github.com/Draugr-official/Skully if you believe this is wrong");
        }

        LLVMValueRef GenerateAssignmentExpression(AssignmentExpressionSyntax assignmentExpression)
        {
            switch(assignmentExpression.Kind())
            {
                case SyntaxKind.SimpleAssignmentExpression:
                    {
                        var left = assignmentExpression.Left;
                        var right = assignmentExpression.Right;
                        var variableName = ((IdentifierNameSyntax)left).Identifier.Text;
                        var variableValue = GenerateExpression(right);
                        DeclareVariable(variableName, variableValue);
                        return variableValue;
                    }
            }

            throw Debug.Error($"{assignmentExpression.Kind()} is not a supported assignment", "Create an issue at https://github.com/Draugr-official/Skully if you believe this is wrong");
        }
        
        LLVMValueRef GenerateIdentifierExpression(IdentifierNameSyntax identifierName)
        {
            return GetVariable(identifierName.Identifier.Text);
        }

        LLVMValueRef GenerateRelationalExpression(BinaryExpressionSyntax binaryExpression)
        {
            switch(binaryExpression.Kind())
            {
                case SyntaxKind.GreaterThanExpression:
                    {
                        var left = GenerateExpression(binaryExpression.Left);
                        var right = GenerateExpression(binaryExpression.Right);
                        Console.WriteLine("RELATIONAL OPERATOR GREATER THAN");
                        return LLVM.BuildICmp(Builder, LLVMIntPredicate.LLVMIntSGT, left, right, "GREATER THAN");
                    }

                case SyntaxKind.LessThanExpression:
                    {
                        var left = GenerateExpression(binaryExpression.Left);
                        var right = GenerateExpression(binaryExpression.Right);
                        Console.WriteLine("RELATIONAL OPERATOR LESS THAN");
                        return LLVM.BuildICmp(Builder, LLVMIntPredicate.LLVMIntSLT, left, right, "LESS THAN");
                    }
            }
            throw Debug.Error($"{binaryExpression.Kind()} is not a supported operation", "Create an issue at https://github.com/Draugr-official/Skully if you believe this is wrong");
        }

        // generate boolean expression
        LLVMValueRef GenerateBooleanExpression(LiteralExpressionSyntax literalExpression)
        {
            return LLVM.ConstInt(LLVM.Int1Type(), Convert.ToUInt64(literalExpression.Token.Value), false);
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
            string literalValue = stringExpression.GetText().ToString();
            literalValue = literalValue.Substring(1, literalValue.Length - 2);
            var str = LLVM.BuildGlobalString(Builder, literalValue, "");
            return LLVM.ConstGEP(str, new LLVMValueRef[] { LLVM.ConstInt(LLVM.Int32Type(), 0, false), LLVM.ConstInt(LLVM.Int32Type(), 0, false) });
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
                case SyntaxKind.FalseLiteralExpression:
                case SyntaxKind.TrueLiteralExpression:
                    {
                        return GenerateBooleanExpression(literalExpression);
                    }

            }
            return LLVM.ConstNull(LLVM.Int8Type());
        }

        LLVMValueRef GenerateInvocationExpression(InvocationExpressionSyntax invocationExpression) // WORK ON THIS
        {
            string invocationName = invocationExpression.Expression.ToString();

            var invocationType = LLVM.FunctionType(LLVM.Int32Type(), new LLVMTypeRef[] { LLVM.PointerType(LLVM.Int8Type(), 0) }, true);
            var invocationFunc = LLVM.GetNamedFunction(Module, invocationName).Pointer == IntPtr.Zero ? LLVM.AddFunction(Module, invocationName, invocationType) : LLVM.GetNamedFunction(Module, invocationName);

            //var str = LLVM.BuildGlobalString(Builder, "Hello, world!\n", "");
            //var format = LLVM.ConstGEP(str, new LLVMValueRef[] { LLVM.ConstInt(LLVM.Int32Type(), 0, false), LLVM.ConstInt(LLVM.Int32Type(), 0, false) });

            var call = LLVM.BuildCall(Builder, invocationFunc, ArgumentBuilder(invocationExpression.ArgumentList), "");
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
