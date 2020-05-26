using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Ast
{
    public interface IHasLeftAstNode
    {
        AstNode Left { get; }
    }
}
