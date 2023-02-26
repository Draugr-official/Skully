using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLVMSharp;

namespace Skully.Compiler.CodeGen.Objects
{
    /*
     This is all super cancer, rewrite
    NOTE: Puts is a lot faster than printf, this should be prioritized when format is not used
     */

    internal class Std
    {
        CodeGenerator CodeGen { get; set; }

        public Std(CodeGenerator codeGenerator)
        {
            this.CodeGen = codeGenerator;
        }

        public void Generate()
        {
            GenerateConsoleWriteLine();
            GenerateConsoleWrite();
        }

        LLVMValueRef GenerateMethod(LLVMTypeRef returnType, string name, LLVMTypeRef[] parameters, bool isVarArg = false)
        {
            var funcType = LLVM.FunctionType(returnType, parameters, isVarArg);
            var func = LLVM.AddFunction(CodeGen.Module, name, funcType);
            var block = LLVM.AppendBasicBlock(func, "entry");
            LLVM.PositionBuilderAtEnd(CodeGen.Builder, block);

            return func;
        }

        LLVMValueRef stdFormat;
        void GenerateConsoleWriteLine()
        {
            LLVMValueRef writelineFunc = GenerateMethod(LLVM.VoidType(), "Console.WriteLine", new LLVMTypeRef[] { LLVM.PointerType(LLVM.Int8Type(), 0) }, true);
            LLVMTypeRef funcType = LLVM.FunctionType(LLVM.Int32Type(), new LLVMTypeRef[] { LLVM.PointerType(LLVM.Int8Type(), 0) }, true);
            LLVMValueRef func = LLVM.AddFunction(CodeGen.Module, "printf", funcType);
            stdFormat = LLVM.BuildGlobalStringPtr(CodeGen.Builder, "%s\n", "");
            LLVM.BuildCall(CodeGen.Builder, func, new LLVMValueRef[] { stdFormat, writelineFunc.GetFirstParam() }, "");
            LLVM.BuildRetVoid(CodeGen.Builder);
        }

        void GenerateConsoleWrite()
        {
            LLVMValueRef writelineFunc = GenerateMethod(LLVM.VoidType(), "Console.Write", new LLVMTypeRef[] { LLVM.PointerType(LLVM.Int8Type(), 0) }, true);
            LLVMValueRef func = LLVM.GetNamedFunction(CodeGen.Module, "printf");
            LLVM.BuildCall(CodeGen.Builder, func, new LLVMValueRef[] { stdFormat, writelineFunc.GetFirstParam() }, "");
            LLVM.BuildRetVoid(CodeGen.Builder);
        }
    }
}