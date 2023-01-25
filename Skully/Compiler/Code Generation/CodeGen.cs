using Skully_Compiler.Compiler.Code_Generation.LLVM.AST.Expressions;
using Skully_Compiler.Compiler.Code_Generation.LLVM.AST.Statements;
using Skully_Compiler.Compiler.Code_Generation.LLVM.Objects;
using Skully_Compiler.Compiler.Syntax_Analysis.AST.Expressions;
using Skully_Compiler.Compiler.Syntax_Analysis.AST.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.Code_Generation
{
    internal class CodeGen
    {
        List<Statement> CsStatements = new List<Statement>();
        List<LLVMStatement> LLVMstatements = new List<LLVMStatement>();

        public CodeGen(List<Statement> statements)
        {
            CsStatements = statements;
        }

        public List<LLVMStatement> Generate()
        {
            return GenerateStatements(new BlockStatement() { Statements = this.CsStatements });
        }

        public List<LLVMStatement> GenerateStatements(BlockStatement csStatements)
        {
            foreach(Statement csStatement in csStatements.Statements)
            {
                LLVMstatements.Add(GenerateStatement(csStatement));
            }

            return LLVMstatements;
        }

        LLVMStatement GenerateStatement(Statement csStatement) // Bug where generated statement is actually not returned (Average .NET MS clowns)
        {
            if(csStatement is MethodStatement csMethodStatement)
            {
                DebugOut.Info("Parsed MethodStatement");
                return new LLVMFunctionStatement()
                {
                    Name = csMethodStatement.Name,
                    isLocal = false,
                    Parameters = csMethodStatement.Parameters.Select(t => (LLVMVariableExpression)GenerateExpression(t)).ToList(),
                    Body = GenerateStatements(csMethodStatement.Body)
                };
            }

            if (csStatement is BlockStatement csBlockStatement)
            {
                DebugOut.Info("Parsed BlockStatement");
                GenerateStatements(csBlockStatement);
                return new LLVMStatement();
            }

            if (csStatement is NamespaceStatement csNamespaceStatement)
            {
                DebugOut.Info("Parsed NamespaceStatement");
                GenerateStatements(csNamespaceStatement.Body);
                return new LLVMStatement();
            }

            if (csStatement is ClassStatement csClassStatement)
            {
                DebugOut.Info("Parsed ClassStatement");
                GenerateStatements(csClassStatement.Body);
                return new LLVMStatement();
            }

            if (csStatement is UsingStatement csUsingStatement)
            {
                DebugOut.Info("Parsed UsingStatement");
                return new LLVMStatement(); // discard
            }

            throw new NotImplementedException($"Statement '{csStatement.GetType().FullName}' not supported");
        }

        LLVMExpression GenerateExpression(Expression csExpression)
        {
            if(csExpression is VariableExpression csVariableExpression)
            {
                return new LLVMVariableExpression()
                {
                    Name = csVariableExpression.Name,
                    ReturnType = LLVMDataTypes.CsToLLVM[csVariableExpression.DataType.Type]
                };
            }

            throw new NotImplementedException($"Expression '{csExpression.ToString()}' not supported");
        }
    }
}
