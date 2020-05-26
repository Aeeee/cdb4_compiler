using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Ast
{
    public class ProgramNode : AstNode
    {
        private List<AstNode> operations = new List<AstNode>();
        public IReadOnlyList<AstNode> Operations { get => operations; }

        public void AddOperation(AstNode op)
        {
            operations.Add(op);
        }

        public override string ToString()
        {
            return $"[Program]";
        }

    }
}
