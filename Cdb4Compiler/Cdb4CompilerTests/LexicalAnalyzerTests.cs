using NUnit.Framework;
using Cdb4Compiler.LexicalAnalysis;

namespace Tests
{
    public class LexicalAnalyzerTests
    {
        private const string CORRECT_CODE_1 = @"
            Var a, b, c, x; // Îáúÿâèëè ïåðåìåííûå
            a := 6;
            b := -6;
            c := 2+2*2;
            IF a==6 THEN x := 1; ELSE x := -1;
            ";

        private const string INCORRECT_CODE_1 = @"
            Var a, b, c;
            a := 4;
            b := (3+5)/2+(a-1); // Äîëæíî ïîëó÷èòüñÿ 7
            IF a>3 THEN a := a + 1;
            IF a<b THEN
                IF a+5<b THEN c := 200; ELSE c := 0;
            ELSE
                c := 100;
            ";

        private const string INCORRECT_CODE_2 = @"
            int a, b, c; // ‘int’ instead of ‘Var’
            a := 4 // no semicolon at the end
            /* Non-supported
                comment style */
            b := (3+5/2+(a-1); // One parenthesis is not closed
            IF a>3 THEN a := a + 1;
            IF a<b THEN
                IF a+5<b THEN c := 200; ELSE c := 0;
            ELSE THEN // No such thing as ‘ELSE THEN’
                c := 100;
            ";

        [Test]
        public void CorrectAnalysis1Test()
        {
            var analyzer = new LexicalAnalyzer(CORRECT_CODE_1);
            analyzer.GetAllTokens();
            analyzer.GetAllErrors();

            //TODO: test tokens
            //TODO: test errors
            Assert.Pass();
        }

        [Test]
        public void IncorrectAnalysis1Test()
        {
            var analyzer = new LexicalAnalyzer(INCORRECT_CODE_1);
            analyzer.GetAllTokens();
            analyzer.GetAllErrors();

            //TODO: test tokens
            //TODO: test errors
            Assert.Pass();
        }

        [Test]
        public void IncorrectAnalysis2Test()
        {
            var analyzer = new LexicalAnalyzer(INCORRECT_CODE_2);
            analyzer.GetAllTokens();
            analyzer.GetAllErrors();

            //TODO: test tokens
            //TODO: test errors
            Assert.Pass();
        }

    }
}
