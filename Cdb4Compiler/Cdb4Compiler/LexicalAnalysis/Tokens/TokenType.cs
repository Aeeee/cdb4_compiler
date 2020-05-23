﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.LexicalAnalysis.Tokens
{
    public enum TokenType
    {
        SPECIAL_SYMBOL,
        KEYWORD,
        PARENTHESES,
        COMPARISON_OP,
        MATH_OP,
        ASSIGNMENT_OP,
        CONSTANT,
        IDENTIFIER,
        END_TEXT
    }
}
