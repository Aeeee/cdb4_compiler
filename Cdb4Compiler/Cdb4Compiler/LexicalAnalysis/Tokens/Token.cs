using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.LexicalAnalysis.Tokens
{
    public class Token
    {
        public TokenType Type { get; private set; }
        public string Text { get; private set; }
        public int AtLine { get; private set; }
        public int AtColumn { get; private set; }

        public Token(TokenType type, string text, int line, int column)
        {
            Type = type;
            Text = text;
            AtLine = line;
            AtColumn = column;
        }

    }
}
