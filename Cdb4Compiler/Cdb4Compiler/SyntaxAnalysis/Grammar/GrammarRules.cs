using Cdb4Compiler.LexicalAnalysis.Tokens;
using Cdb4Compiler.SyntaxAnalysis.Grammar.GrammarEntries;
using Cdb4Compiler.SyntaxAnalysis.ParseTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Grammar
{
    public class GrammarRules
    {
        private static GrammarRule[] rules = new GrammarRule[]
        {
            new GrammarRule(Nt(NonTermType.PROGRAM),
                new GrammarEntrySet(
                    Nt(NonTermType.VAR_DECL),
                    Nt(NonTermType.OPERATOR_LIST),
                    T(TokenType.END_TEXT)
                )
            ),
            new GrammarRule(Nt(NonTermType.VAR_DECL),
                new GrammarEntrySet(
                    Nt(NonTermType.VAR_LIST),
                    T(TokenType.SPECIAL_SYMBOL, ";")
                )
            ),
            new GrammarRule(Nt(NonTermType.VAR_LIST),
                new GrammarEntrySet(
                    Nt(NonTermType.VAR_LIST),
                    T(TokenType.SPECIAL_SYMBOL, ","),
                    T(TokenType.IDENTIFIER)
                ),
                new GrammarEntrySet(
                    T(TokenType.KEYWORD, "Var"),
                    T(TokenType.IDENTIFIER)
                )
            ),
            new GrammarRule(Nt(NonTermType.OPERATOR_LIST),
                new GrammarEntrySet(
                    Nt(NonTermType.OPERATOR_LIST),
                    Nt(NonTermType.OPERATOR)
                ),
                new GrammarEntrySet(
                    Nt(NonTermType.OPERATOR)
                )
            ),
            new GrammarRule(Nt(NonTermType.OPERATOR),
                new GrammarEntrySet(
                    Nt(NonTermType.ASSIGNMENT)
                ),
                new GrammarEntrySet(
                    Nt(NonTermType.COMPLEX_OPERATOR)
                )
            ),
            new GrammarRule(Nt(NonTermType.ASSIGNMENT),
                new GrammarEntrySet(
                    T(TokenType.IDENTIFIER),
                    T(TokenType.ASSIGNMENT_OP),
                    Nt(NonTermType.EXPRESSION),
                    T(TokenType.SPECIAL_SYMBOL, ";")
                )
            ),
            new GrammarRule(Nt(NonTermType.EXPRESSION),
                new GrammarEntrySet(
                    T(TokenType.MATH_OP, "-"),
                    Nt(NonTermType.SUBEXPRESSION)
                ),
                new GrammarEntrySet(
                    Nt(NonTermType.SUBEXPRESSION)
                )
            ),
            new GrammarRule(Nt(NonTermType.SUBEXPRESSION),
                new GrammarEntrySet(
                    T(TokenType.PARENTHESES, "("),
                    Nt(NonTermType.EXPRESSION),
                    T(TokenType.PARENTHESES, ")")
                ),
                new GrammarEntrySet(
                    Nt(NonTermType.OPERAND)
                ),
                new GrammarEntrySet(
                    Nt(NonTermType.SUBEXPRESSION),
                    T(TokenType.MATH_OP),
                    Nt(NonTermType.SUBEXPRESSION)
                )
            ),
            new GrammarRule(Nt(NonTermType.OPERAND),
                new GrammarEntrySet(
                    T(TokenType.IDENTIFIER)
                ),
                new GrammarEntrySet(
                    T(TokenType.CONSTANT)
                )
            ),
            new GrammarRule(Nt(NonTermType.COMPLEX_OPERATOR),
                new GrammarEntrySet(
                    T(TokenType.KEYWORD, "IF"),
                    Nt(NonTermType.EXPRESSION),
                    T(TokenType.KEYWORD, "THEN"),
                    Nt(NonTermType.OPERATOR),
                    T(TokenType.KEYWORD, "ELSE"),
                    Nt(NonTermType.OPERATOR)
                ),
                new GrammarEntrySet(
                    T(TokenType.KEYWORD, "IF"),
                    Nt(NonTermType.EXPRESSION),
                    T(TokenType.KEYWORD, "THEN"),
                    Nt(NonTermType.OPERATOR)
                )
            )
        };

        private static NonTermEntry Nt(NonTermType type) => new NonTermEntry(type);
        private static TerminalEntry T(TokenType type) => new TerminalEntry(type);
        private static TerminalEntry T(TokenType type, string restr) => new TerminalEntry(type, restr);

        public static (NonTermEntry, int) MatchReduce(List<ParseTreeNode> nodes, int startFrom)
        {
            (var rule, int length) = GetMatchingRule(nodes, startFrom);
            if (rule != null)
                return (rule.Result, length);
            return (null, 0);
        }

        private static (GrammarRule, int) GetMatchingRule(List<ParseTreeNode> nodes, int startFrom)
        {
            foreach (var rule in rules)
                foreach (var pattern in rule.Patterns)
                    if (DoesMatch(pattern, nodes, startFrom))
                        return (rule, pattern.Length);
            return (null, 0);
        }

        private static bool DoesMatch(GrammarEntrySet pattern, List<ParseTreeNode> nodes, int startFrom)
        {
            int i = startFrom;
            foreach (var entry in pattern.Entries)
            {
                if (i >= nodes.Count)
                    return false;

                if (!DoesMatch(nodes[i], entry))
                    return false;

                i += 1;
            }

            return true;
        }

        private static bool DoesMatch(ParseTreeNode node, GrammarEntry entry)
        {
            if (node is TermParseTreeNode && entry is TerminalEntry)
            {
                var termNode = node as TermParseTreeNode;
                var nodeToken = termNode.Token;
                var termEntry = entry as TerminalEntry;
                return nodeToken.Type == termEntry.Type &&
                    (termEntry.RequiredValue == null || nodeToken.Text == termEntry.RequiredValue);
            }

            if (node is NonTermParseTreeNode && entry is NonTermEntry)
            {
                var nonTermNode = node as NonTermParseTreeNode;
                var nonTermEntry = entry as NonTermEntry;
                return nonTermNode.Type == nonTermEntry.Type;
            }

            return false;
        }

    }
}
