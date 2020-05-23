using Cdb4Compiler.LexicalAnalysis;
using Cdb4Compiler.LexicalAnalysis.Tokens;
using Cdb4Compiler.SyntaxAnalysis;
using System;
using System.Collections.Generic;
using System.IO;

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
            var errors = lexicalAnalyzer.GetAllErrors();

            if (errors.Count > 0)
            {
                PrintAllErrors(errors);
                Console.ReadLine();
                return;
            }

            var syntaxAnalyzer = new SyntaxAnalyzer(lexicalAnalyzer.GetAllTokens());


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

        static void PrintAllErrors(IReadOnlyList<LexicalError> errors)
        {
            Console.WriteLine($"Encountered {errors.Count} errors:");
            foreach (var err in errors)
            {
                Console.WriteLine(">> " + err.Text);
                Console.WriteLine($"    (at line {err.AtLine}, col {err.AtColumn})");
            }
        }

        static void PrintTokenList(IReadOnlyList<Token> tokens)
        {
            Console.WriteLine("\n\nToken list:");
            foreach (var token in tokens)
                Console.WriteLine("(" + token.Type + "):  " + token.Text);
        }

    }
}
