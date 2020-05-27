using Cdb4Compiler.DataFormatting;
using Cdb4Compiler.LexicalAnalysis;
using Cdb4Compiler.LexicalAnalysis.Tokens;
using Cdb4Compiler.SyntaxAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = GetSourceFilePath();
            if (filename == null)
            {
                Console.ReadLine();
                return;
            }

            string sourceText = ReadSource(filename);
            var lexicalAnalyzer = new LexicalAnalyzer(sourceText);
            var lexErrors = lexicalAnalyzer.GetAllErrors();

            if (lexErrors.Count > 0)
            {
                PrintLexErrors(lexErrors);
                Console.ReadLine();
                return;
            }

            var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalyzer.GetAllTokens());
            var syntaxErrors = syntaxAnalyzer.GetErrors();

            if (syntaxErrors.Count > 0)
            {
                PrintSyntaxErrors(syntaxErrors, sourceText);
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Success!");
            Console.WriteLine("\n\nParse tree:");
            ParseTreeOutput.Print(syntaxAnalyzer.GetParseTreeRoot());

            Console.WriteLine("\n\nAST:");
            AstOutput.Print(syntaxAnalyzer.GetAstRoot());

            Console.ReadLine();
        }

        static string GetSourceFilePath()
        {
            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory() + "\n\n");
            Console.WriteLine("Enter source file name: ");

            string filename = Console.ReadLine();
            if (!File.Exists(filename))
            {
                Console.WriteLine("File does not exist.");
                return null;
            }

            return filename;
        }

        static string ReadSource(string filename)
        {
            using (var sr = new StreamReader(filename))
                return sr.ReadToEnd();
        }

        static void PrintLexErrors(IReadOnlyList<LexicalError> errors)
        {
            Console.WriteLine($"Encountered {errors.Count} error(s):");
            foreach (var err in errors)
            {
                Console.WriteLine(">> " + err.Text);
                Console.WriteLine($"    (at line {err.AtLine}, col {err.AtColumn})");
            }
        }

        static void PrintSyntaxErrors(IReadOnlyList<SyntaxError> errors, string sourceCode)
        {
            Console.WriteLine($"Encountered {errors.Count} error(s):");
            foreach (var err in errors)
            {
                Console.WriteLine(">> " + err.Message);
                if (err.Line >= 0)
                    Console.WriteLine($"    (at line {err.Line})");
            }
        }

        static string GetSourcePart(string source, int fromLine, int fromCol, int toLine, int toCol)
        {
            StringBuilder sb = new StringBuilder();
            string[] lines = source.Split('\n');
            for (int line = fromLine; line <= toLine; line++)
            {
                string lineStr = lines[line - 1];
                int startCol = (line == fromLine) ? fromCol : 1;
                int endCol = (line == toLine) ? toCol : lineStr.Length;
                sb.Append(lineStr.Substring(startCol - 1, endCol - startCol));

                if (line != toLine)
                    sb.AppendLine();
            }
            return sb.Replace('\r', ' ').ToString();
        }

        static void PrintTokenList(IReadOnlyList<Token> tokens)
        {
            Console.WriteLine("\n\nToken list:");
            foreach (var token in tokens)
                Console.WriteLine("(" + token.Type + "):  " + token.Text);
        }

    }
}
