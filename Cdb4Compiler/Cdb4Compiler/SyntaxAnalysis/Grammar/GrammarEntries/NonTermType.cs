using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries
{
    public enum NonTermType
    {
        PROGRAM,
        VAR_DECL,
        VAR_LIST,
        OPERATOR_LIST,
        OPERATOR,
        ASSIGNMENT,
        EXPRESSION,
        SUBEXPRESSION,
        OPERAND,
        COMPLEX_OPERATOR
    }
}
