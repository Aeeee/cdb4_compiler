using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Cdb4Compiler.LexicalAnalysis.Tokens
{
    class TokenRules
    {
        private static TokenRule[] rules = new TokenRule[]
        {
            new TokenRule(TokenType.SPECIAL_SYMBOL, "^(;|,)$"),
            new TokenRule(TokenType.KEYWORD, "^(Var|IF|THEN|ELSE)$"),
            new TokenRule(TokenType.PARENTHESES, "^(\\(|\\))$"),
            new TokenRule(TokenType.COMPARISON_OP, "^(>|<|==)$"),
            new TokenRule(TokenType.MATH_OP, "^(\\+|-|\\*|/)$"),
            new TokenRule(TokenType.ASSIGNMENT_OP, "^:=$"),
            new TokenRule(TokenType.CONSTANT, "^([0-9]+)$"),
            new TokenRule(TokenType.IDENTIFIER, "^([a-z]+)$")
        };

        public static int GetMatchCount(string input)
        {
            int matches = 0;
            foreach (var rule in rules)
                if (Regex.IsMatch(input, rule.Pattern))
                    matches += 1;
            return matches;
        }

        public static Token CreateToken(string input, int line, int column)
        {
            var rule = GetFirstMatch(input);
            if (rule == null)
                throw new ArgumentException("Input does not match any token rules.");
            return new Token(rule.Type, input, line, column);
        }

        private static TokenRule GetFirstMatch(string input)
        {
            foreach (var rule in rules)
                if (Regex.IsMatch(input, rule.Pattern))
                    return rule;
            return null;
        }

    }
}
