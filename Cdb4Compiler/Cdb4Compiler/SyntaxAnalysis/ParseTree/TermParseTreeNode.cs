using Cdb4Compiler.LexicalAnalysis.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.ParseTree
{
    public class TermParseTreeNode : ParseTreeNode
    {
        public Token Token { get; private set; }
        public override int ChildCount => 0;

        public TermParseTreeNode(Token token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"[Term({Token.Type}, {Token.Text})]";
        }

    }
}
