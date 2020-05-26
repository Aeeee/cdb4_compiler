using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Ast
{
    public class ConstantNode : AstNode
    {
        public string Value { get; private set; }

        public ConstantNode(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"[Contant '{Value}']";
        }

    }
}
