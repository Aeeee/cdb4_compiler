using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis
{
    public class SyntaxError
    {
        public string Message { get; private set; }
        public int Line { get; private set; }

        public SyntaxError(int line, string msg)
        {
            Message = msg;
            Line = line;
        }

    }
}
