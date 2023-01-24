using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skully_Compiler.Compiler.SyntaxAnalysis
{
    internal class TokenReader
    {
        List<LexToken> LexTokens = new List<LexToken>();

        public TokenReader(List<LexToken> tokenStream)
        {
            this.LexTokens = tokenStream;
        }

        int At = 0;

        /// <summary>
        /// Expects token type at position
        /// </summary>
        /// <param name="lexType"></param>
        /// <returns></returns>
        public bool Expect(LexType lexType, int offset = 0) => LexTokens[At + offset].Type == lexType;

        /// <summary>
        /// Expects a 
        /// </summary>
        /// <param name="lexType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Expect(LexType lexType, string value) => LexTokens[At].Type == lexType && LexTokens[At].Value == value;

        /// <summary>
        /// Expects a token type at current position and throws an exception if not met
        /// </summary>
        /// <param name="lexType"></param>
        /// <param name="Line"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ExpectFatal(LexType lexType)
        {
            if (LexTokens[At].Type == lexType)
            {
                return true;
            }
            else
            {
                throw new Exception($"Expected {lexType} at line {LexTokens[At].Line}, got {LexTokens[At].Type} {LexTokens[At].Value}");
            }
        }

        /// <summary>
        /// Checks if a token type matches and consumes it
        /// </summary>
        /// <param name="lexToken"></param>
        /// <returns></returns>
        public LexToken Consume(LexType lexType)
        {
            if(LexTokens[At].Type == lexType)
            {
                At += 1;
                return LexTokens[At - 1];
            }

            throw new Exception("Identifier expected, got " + lexType.ToString());
        }

        /// <summary>
        /// Gets token at current index + position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public LexToken Peek(int position = 0) => LexTokens[At + position];

        /// <summary>
        /// Peeks the next token
        /// </summary>
        /// <returns></returns>
        public LexToken PeekNext() => LexTokens[At + 1];

        /// <summary>
        /// Peeks the previous token
        /// </summary>
        /// <returns></returns>
        public LexToken PeekPrevious() => LexTokens[At - 1];

        /// <summary>
        /// Skips `count` amount of tokens
        /// </summary>
        /// <param name="count"></param>
        public void Skip(int count) => At += count;

        public void WriteDebug() => Console.WriteLine($"LexType: {this.Peek().Type}, LexValue: {this.Peek().Value}, Line: {this.Peek().Line}.");
    }
}
