using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis
{
    public class SyntaxError
    {
        public string Message { get; private set; }
        public int Line { get; private set; }
        //public int StartLine { get; private set; }
        //public int StartColumn { get; private set; }
        //public int? EndLine { get; private set; }
        //public int? EndColumn { get; private set; }
        //public bool IncludeFragment { get; private set; }

        public SyntaxError(int line, string msg)
        {
            Message = msg;
            Line = line;
        }

        //public SyntaxError(int startLine, int startColumn,
        //    int? endLine, int? endColumn, string message, bool includeFragment)
        //{
        //    Message = message;
        //    StartLine = startLine;
        //    StartColumn = startColumn;
        //    EndLine = endLine;
        //    EndColumn = endColumn;
        //    IncludeFragment = includeFragment;
        //}

        //public SyntaxError(int line, int column, string message)
        //    : this(line, column, null, null, message, false) {}



    }
}
