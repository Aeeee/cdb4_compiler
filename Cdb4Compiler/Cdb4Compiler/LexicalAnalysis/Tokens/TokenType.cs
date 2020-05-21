using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.LexicalAnalysis.Tokens
{
    public enum TokenType
    {
        SPECIAL_SYMBOL,
        KEYWORD,
        PARENTHESES,
        COMPARSION_OP,
        SUMM_DIFF_OP,
        MUL_DIV_OP,
        ASSIGNMENT_OP,
        CONSTANT,
        IDENTIFIER,
        END_TEXT
    }
}
