using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Ast
{
    public class VarDeclNode : AstNode
    {
        private List<AstNode> children = new List<AstNode>();
        public IReadOnlyList<AstNode> Children { get => children; }

        public void AddChild(AstNode node)
        {
            children.Add(node);
        }

        public override string ToString()
        {
            return $"[Variable Declaration]";
        }

    }
}
