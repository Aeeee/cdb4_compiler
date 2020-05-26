using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Ast
{
    public class IdentifierNode : AstNode
    {
        public string Name { get; private set; }

        public IdentifierNode(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"[Identifier '{Name}']";
        }

    }
}
