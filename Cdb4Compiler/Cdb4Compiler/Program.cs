using Cdb4Compiler.LexicalAnalysis;
using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Current directory: " + Directory.GetCurrentDirectory() + "\n\n");

            Console.WriteLine("Enter source file name: ");
            
            string filename = Console.ReadLine();
            if (!File.Exists(filename))
            {
                Console.WriteLine("File does not exist.");
                Console.ReadLine();
                return;
            }

            StreamReader sr = new StreamReader(filename);
            string sourceText = sr.ReadToEnd();
            sr.Close();
            var analyzer = new LexicalAnalyzer(sourceText);
            var errors = analyzer.GetAllErrors();

            if (errors.Count > 0)
            {
                Console.WriteLine($"Encountered {errors.Count} errors:");
                foreach (var err in errors)
                {
                    Console.WriteLine(">> " + err.Text);
                    Console.WriteLine($"    (at line {err.AtLine}, col {err.AtColumn})");
                }
            }

            Console.WriteLine("\n\nToken list:");
            var tokens = analyzer.GetAllTokens();
            foreach (var token in tokens)
            {
                Console.WriteLine("(" + token.Type + "):  " + token.Text);
            }

            Console.ReadLine();
        }
    }
}
