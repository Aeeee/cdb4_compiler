using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Ast
{
    public class ConditionNode : AstNode
    {
        public AstNode Condition { get; private set; }
        public AstNode Then { get; private set; }
        public AstNode Else { get; private set; }

        public ConditionNode(AstNode condition, AstNode thenNode, AstNode elseNode)
        {
            Condition = condition;
            Then = thenNode;
            Else = elseNode;
        }

        public override string ToString()
        {
            return $"[Condition]";
        }

    }
}
