﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cdb4Compiler.SyntaxAnalysis.Ast
{
    public interface IHasRightAstNode
    {
        AstNode Right { get; }
    }
}
