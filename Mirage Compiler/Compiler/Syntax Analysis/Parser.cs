﻿using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Expressions;
using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements;
using Mirage_Compiler.Compiler.Syntax_Analysis.AST.Statements.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.SyntaxAnalysis
{
    internal class Parser
    {
        TokenReader tokenReader { get; set; }

        public Parser(List<LexToken> lexTokens)
        {
            tokenReader = new TokenReader(lexTokens);
        }

        public bool ParseBlockStatement(out BlockStatement blockStatement)
        {
            blockStatement = new BlockStatement();

            if(tokenReader.Expect(LexType.BraceOpen))
            {
                tokenReader.Skip(1);
                blockStatement.Statements = ParseStatements();
                tokenReader.Skip(1);
                return true;
            }

            return false;
        }

        public bool ParseNamespaceStatement(out NamespaceStatement namespaceStatement)
        {
            namespaceStatement = new NamespaceStatement();

            if (tokenReader.Expect(LexType.Keyword, "namespace"))
            {
                tokenReader.Skip(1);
                namespaceStatement.Name = tokenReader.Consume(LexType.Identifier).Value;

                if (tokenReader.ExpectFatal(LexType.BraceOpen))
                {
                    namespaceStatement.Body = new BlockStatement(ParseStatements());
                    return true;
                }
            }

            return false;
        }


        public bool ParseClassStatement(out ClassStatement classStatement) // TODO: Attributes, constructor
        {
            classStatement = new ClassStatement();

            if (tokenReader.Expect(LexType.Keyword, "class"))
            {
                tokenReader.Skip(1);
                classStatement.Name = tokenReader.Consume(LexType.Identifier).Value;


                if (tokenReader.ExpectFatal(LexType.BraceOpen))
                {
                    classStatement.Body = new BlockStatement(ParseStatements());
                    return true;
                }
            }

            return false;
        }

        public bool ParseUsingStatement(out UsingStatement usingStatement)
        {
            usingStatement = new UsingStatement();

            if(tokenReader.Expect(LexType.Keyword, "using"))
            {
                tokenReader.Skip(1);
                usingStatement.Namespace = tokenReader.Consume(LexType.Identifier).Value.Split('.').ToList();

                if (tokenReader.ExpectFatal(LexType.Semicolon))
                {
                    tokenReader.Skip(1);
                }
                return true;
            }

            return false;
        }

        public bool ParseMethodStatement(out MethodStatement methodStatement)
        {
            methodStatement = new MethodStatement();

            List<Modifiers> Modifiers = new List<Modifiers>();
            int _base = 0;
            if (ModifierInfo.Names.ContainsValue(tokenReader.Peek().Value))
            {
                for(int i = 0; ; i++)
                {
                    if(ModifierInfo.Names.ContainsValue(tokenReader.Peek(i).Value))
                    {
                        _base++;
                        Modifiers.Add(ModifierInfo.Names.FirstOrDefault(x => x.Value == tokenReader.Peek(i).Value).Key);
                    }
                    else
                    {
                        break;
                    }
                }
            }


            if (DataTypeInfo.Names.ContainsValue(tokenReader.Peek(_base).Value))
            {
                string dataType = tokenReader.Peek(_base).Value;
                _base += 1;

                if (tokenReader.Expect(LexType.Identifier, _base))
                {
                    string name = tokenReader.Peek(_base).Value;
                    _base += 1;

                    if (tokenReader.Expect(LexType.ParentheseOpen, _base))
                    {
                        tokenReader.Skip(_base);

                        while(!tokenReader.Expect(LexType.ParentheseClose)) // TEMPORARY SOLUTION TO PARSING PARAMETERS
                        {
                            tokenReader.Skip(1);
                        }
                        tokenReader.Skip(1);

                        methodStatement.Modifiers = Modifiers;
                        methodStatement.ReturnType = new DataType() { Type = DataTypeInfo.Names.FirstOrDefault(x => x.Value == dataType).Key };
                        methodStatement.Name = name;
                        if(ParseBlockStatement(out BlockStatement blockStatement))
                        {
                            methodStatement.Body = blockStatement;
                            return true;
                        }

                        // TODO: Parse parameters
                    }
                }
            }

            return false;
        }

        public bool ParseMethodCallStatement(out MethodCallStatement methodCallStatement)
        {
            methodCallStatement = new MethodCallStatement();
            
            if(tokenReader.Expect(LexType.Identifier))
            {
                if(tokenReader.Expect(LexType.ParentheseOpen, 1))
                {
                    methodCallStatement.Name = tokenReader.Peek().Value;
                    tokenReader.Skip(1);
                    methodCallStatement.Arguments = ParseExpressions();

                    if(tokenReader.ExpectFatal(LexType.Semicolon))
                    {
                        tokenReader.Skip(1);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ParseStatement(out Statement statement)
        {
            statement = new Statement();

            if(ParseBlockStatement(out BlockStatement blockStatement))
            {
                statement = blockStatement;
                return true;
            }

            if (ParseClassStatement(out ClassStatement classStatement))
            {
                statement = classStatement;
                return true;
            }

            if (ParseNamespaceStatement(out NamespaceStatement namespaceStatement))
            {
                statement = namespaceStatement;
                return true;
            }

            if (ParseUsingStatement(out UsingStatement usingStatement))
            {
                statement = usingStatement;
                return true;
            }

            if(ParseMethodStatement(out MethodStatement methodStatement))
            {
                statement = methodStatement;
                return true;
            }

            if (ParseMethodCallStatement(out MethodCallStatement methodCallStatement))
            {
                statement = methodCallStatement;
                return true;
            }

            return false;
        }

        public List<Statement> ParseStatements()
        {
            List<Statement> statements = new List<Statement>();

            for(; ; )
            {
                if(ParseStatement(out Statement statement))
                {
                    statements.Add(statement);
                }
                else
                {
                    break;
                }
            }

            return statements;
        }

        public bool ParseLiteralExpression(out LiteralExpression literalExpression)
        {
            literalExpression = new LiteralExpression();

            if (tokenReader.Expect(LexType.String) || tokenReader.Expect(LexType.Number) || tokenReader.Expect(LexType.Boolean))
            {
                literalExpression.Type = LiteralExpression.LiteralLookup[tokenReader.Peek().Type];
                literalExpression.Value = tokenReader.Peek().Value;
                tokenReader.Skip(1);
                return true;
            }

            return false;
        }

        public bool ParseExpression(out Expression expression)
        {
            expression = new Expression();

            if(ParseLiteralExpression(out LiteralExpression literalExpression))
            {
                expression = literalExpression;
                return true;
            }

            return false;
        }

        public List<Expression> ParseExpressions()
        {
            List<Expression> expressions = new List<Expression>();

            if(tokenReader.Expect(LexType.ParentheseOpen))
            {
                while(!tokenReader.Expect(LexType.ParentheseClose))
                {
                    if(ParseExpression(out Expression expression))
                    {
                        expressions.Add(expression);
                    }
                    else
                    {
                        tokenReader.Skip(1);
                    }
                }
                tokenReader.Skip(1);
            }

            return expressions;
        }
    }
}