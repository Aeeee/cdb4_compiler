using Cdb4Compiler.LexicalAnalysis.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.LexicalAnalysis.Impl
{
    sealed class LexicalAnalyzerImpl
    {
        private List<Token> tokens;
        private List<LexicalError> errors;

        public LexicalAnalyzerImpl(List<Token> tokens, List<LexicalError> errors)
        {
            this.tokens = tokens;
            this.errors = errors;
        }

        private bool skipToLineEnd = false;
        private int bufferStart = 0;
        private string curLine = null;

        public void Analyze(string sourceText)
        {
            string[] lines = sourceText.Split(
                new string[] { "\n", "\r\n" }, StringSplitOptions.None);

            for (int y = 0; y < lines.Length; y++)
            {
                NextLine(lines[y]);

                for (int x = 0; x < lines[y].Length; x++)
                {
                    NextChar(lines[y][x], y, x);
                    if (skipToLineEnd)
                        break;
                }
            }
        }


        private void NextChar(char c, int line, int col)
        {
            if (char.IsWhiteSpace(c))
            {
                if (bufferStart == col)
                    bufferStart += 1;
                return;
            }

            char? nextChar = PeekChar(col + 1);
            if (c == '/' && nextChar == '/')
            {
                skipToLineEnd = true;
                return;
            }

            int curMatches = TokenRules.GetMatchCount(GetBufferContent(col));
            int nextMatches = (nextChar == null) ? 0 :
                TokenRules.GetMatchCount(GetBufferContent(col + 1));

            //Console.WriteLine("buffer: " + GetBufferContent(col));
            //Console.WriteLine("cur matches: " + curMatches + ", next: " + nextMatches);

            if (curMatches > 0 && nextMatches == 0)
            {
                var token = TokenRules.CreateToken(GetBufferContent(col), line+1, bufferStart+1);
                tokens.Add(token);
                bufferStart = col + 1;
            }

            bool nextIsNullOrWhiteSpace = nextChar == null || char.IsWhiteSpace(nextChar.Value);
            if (curMatches == 0 && nextIsNullOrWhiteSpace)
            {
                var error = new LexicalError(
                    "Token not recognized: " + GetBufferContent(col), line+1, bufferStart+1);
                errors.Add(error);
                bufferStart = col + 1;
            }
        }

        private char? PeekChar(int col)
        {
            if (curLine.Length <= col)
                return null;
            return curLine[col];
        }

        private void NextLine(string lineStr)
        {
            skipToLineEnd = false;
            bufferStart = 0;
            curLine = lineStr;
        }
        
        private string GetBufferContent(int until)
        {
            return curLine.Substring(bufferStart, until - bufferStart + 1);
        }

    }
}
