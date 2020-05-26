using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Ast
{
    public class UnaryOpNode : AstNode, IHasRightAstNode
    {
        public string Operator { get; private set; }
        public AstNode Right { get; private set; }

        public UnaryOpNode(string op, AstNode right)
        {
            Operator = op;
            Right = right;
        }

        public override string ToString()
        {
            return $"[UnaryOp '{Operator}']";
        }

    }
}
