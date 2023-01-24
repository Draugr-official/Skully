using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirage_Compiler.Compiler.SyntaxAnalysis
{
    internal class Lexer
    {
        List<LexToken> Tokens = new List<LexToken>();

        List<string> Keywords = new List<string>
        {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "void",
            "volatile",
            "while"
        };

        LexType Identify(string Value)
        {
            if (int.TryParse(Value, out _))
                return LexType.Number;

            if (Value == "false"
                || Value == "true")
                return LexType.Boolean;

            if (Keywords.Contains(Value))
                return LexType.Keyword;

            return LexType.Identifier;
        }

        public List<LexToken> Analyze(string Input)
        {
            List<LexToken> LexTokens = new List<LexToken>();
            StringBuilder sb = new StringBuilder();
            int Line = 1;

            for (int i = 0; i < Input.Length; i++)
            {
                LexType Type = LexType.Terminal;
                string value = "";
                switch (Input[i])
                {
                    case '(': Type = LexType.ParentheseOpen; break;
                    case ')': Type = LexType.ParentheseClose; break;

                    case '[': Type = LexType.BracketOpen; break;
                    case ']': Type = LexType.BracketClose; break;

                    case '{': Type = LexType.BraceOpen; break;
                    case '}': Type = LexType.BraceClose; break;

                    case '<': Type = LexType.ChevronOpen; break;
                    case '>': Type = LexType.ChevronClose; break;


                    case ':': Type = LexType.Colon; break;
                    case ';': Type = LexType.Semicolon; break;

                    case ',': Type = LexType.Comma; break;

                    case '=': Type = LexType.Equals; break;

                    case '%':
                    case '/':
                    case '*':
                    case '-':
                    case '+': Type = LexType.Operator; break;

                    case '"':
                        {
                            i++;
                            while (Input[i] != '"')
                            {
                                if (Input[i] == '\\' && Input[i + 1] == '"')
                                {
                                    i++;
                                }
                                sb.Append(Input[i]);
                                i++;
                            }
                            value = sb.ToString();
                            Type = LexType.String;
                            sb.Clear();

                            break;
                        }

                    case ' ': // Ignore
                    case '\t':
                    case '\r':
                        break;

                    case '\n':
                        Line++;
                        break;

                    default:
                        {
                            if (Char.IsLetterOrDigit(Input[i])
                                || Input[i] == '.')
                            {
                                while (Input.Length > i && (Char.IsLetterOrDigit(Input[i]) || Input[i] == '.'))
                                {
                                    sb.Append(Input[i++]);
                                }
                                i--;
                            }
                            value = sb.ToString();
                            Type = Identify(value);

                            sb.Clear();
                            break;
                        }
                }


                if (Type != LexType.Terminal)
                {
                    LexTokens.Add(new LexToken()
                    {
                        Type = Type,
                        Value = value,
                        Line = Line
                    });
                }
            }

            LexTokens.Add(new LexToken()
            {
                Type = LexType.EOF
            });

            return LexTokens;
        }
    }

    public struct LexToken
    {
        public LexType Type;
        public string Value;
        public int Line;
    }

    public enum LexType {
        UN,

        Terminal,

        /// <summary>
        /// E.g '['
        /// </summary>
        BracketOpen,

        /// <summary>
        /// E.g ']'
        /// </summary>
        BracketClose,


        /// <summary>
        /// E.g '('
        /// </summary>
        ParentheseOpen,

        /// <summary>
        /// E.g ')'
        /// </summary>
        ParentheseClose,


        /// <summary>
        /// E.g '{'
        /// </summary>
        BraceOpen,

        /// <summary>
        /// E.g '}'
        /// </summary>
        BraceClose,


        /// <summary>
        /// E.g '<'
        /// </summary>
        ChevronOpen,

        /// <summary>
        /// E.g '>'
        /// </summary>
        ChevronClose,

        /// <summary>
        /// E.g ':'
        /// </summary>
        Colon,

        /// <summary>
        /// E.g ';'
        /// </summary>
        Semicolon,

        /// <summary>
        /// E.g '.'
        /// </summary>
        Dot,

        /// <summary>
        /// E.g ','
        /// </summary>
        Comma,

        /// <summary>
        /// E.g '='
        /// </summary>
        Equals,

        /// <summary>
        /// E.g '+, -'
        /// </summary>
        Operator,

        /// <summary>
        /// E.g '"Hello World"'
        /// </summary>
        String,

        /// <summary>
        /// E.g ''a''
        /// </summary>
        Char,

        /// <summary>
        /// E.g '369'
        /// </summary>
        Number,

        /// <summary>
        /// E.g 'true', 'false'
        /// </summary>
        Boolean,

        /// <summary>
        /// E.g 'var1', '8ball'
        /// </summary>
        Identifier,

        /// <summary>
        /// E.g 'if', 'while'
        /// </summary>
        Keyword,

        /// <summary>
        /// End of file
        /// </summary>
        EOF
    }
}
