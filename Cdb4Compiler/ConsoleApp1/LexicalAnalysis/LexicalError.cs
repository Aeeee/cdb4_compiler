using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.LexicalAnalysis
{
    public class LexicalError
    {
        public string Text { get; private set; }
        public int AtLine { get; private set; }
        public int AtColumn { get; private set; }

        public LexicalError(string text, int line, int column)
        {
            Text = text;
            AtLine = line;
            AtColumn = column;
        }

    }
}
