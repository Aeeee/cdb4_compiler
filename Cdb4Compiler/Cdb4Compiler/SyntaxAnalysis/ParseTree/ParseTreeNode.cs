using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.ParseTree
{
    public abstract class ParseTreeNode
    {
        public abstract int ChildCount { get; }
    }
}
