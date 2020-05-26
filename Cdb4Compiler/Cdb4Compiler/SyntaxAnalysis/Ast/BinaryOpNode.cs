using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Ast
{
    public class BinaryOpNode : AstNode, IHasLeftAstNode, IHasRightAstNode
    {
        public string Operator { get; private set; }
        public AstNode Left { get; private set; }
        public AstNode Right { get; private set; }

        public BinaryOpNode(string op, AstNode left, AstNode right)
        {
            Operator = op;
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"[BinaryOp '{Operator}']";
        }

    }
}
