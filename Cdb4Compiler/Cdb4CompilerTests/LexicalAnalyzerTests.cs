using NUnit.Framework;
using Cdb4Compiler.LexicalAnalysis;

namespace Tests
{
    public class LexicalAnalyzerTests
    {
        private const string CORRECT_CODE_1 = @"
            Var a, b, c, x; // ќбъ€вили переменные
            a := 6;
            b := -6;
            c := 2+2*2;
            IF a==6 THEN x := 1; ELSE x := -1;
            ";

        private const string CORRECT_CODE_2 = @"
            Var a, b, c;
            a := 4;
            b := (3+5)/2+(a-1); // ƒолжно получитьс€ 7
            IF a>3 THEN a := a + 1;
            IF a<b THEN
                IF a+5<b THEN c := 200; ELSE c := 0;
            ELSE
                c := 100;
            ";

        private const string INCORRECT_CODE_1 = @"
            Var a, 4b, c;  // Ќельз€ назвать переменную 4b
            a.x := 11; // “очки в переменных не поддерживаютс€
            IF a&b<3 THEN c := 44f; // —имвол & не поддерживаетс€. Ќельз€ использовать буквы в константе
            ";

        private const string INCORRECT_CODE_2 = @"
            int a, b, c; // СintТ instead of СVarТ
            a := 4 // no semicolon at the end
            /* Non-supported
                comment style */
            b := (3+5/2+(a-1); // One parenthesis is not closed
            IF a>3 THEN a := a + 1;
            IF a<b THEN
                IF a+5<b THEN c := 200; ELSE c := 0;
            ELSE THEN // No such thing as СELSE THENТ
                c := 100;
            ";

        [Test]
        public void CorrectAnalysis1Test()
        {
            var analyzer = new LexicalAnalyzer(CORRECT_CODE_1);
            var tokens = analyzer.GetAllTokens();
            var errors = analyzer.GetAllErrors();

            Assert.AreEqual(41, tokens.Count);
            Assert.AreEqual(0, errors.Count);

            //TODO: better test of tokens?
        }

        [Test]
        public void CorrectAnalysis2Test()
        {
            var analyzer = new LexicalAnalyzer(CORRECT_CODE_2);
            var tokens = analyzer.GetAllTokens();
            var errors = analyzer.GetAllErrors();

            Assert.AreEqual(64, tokens.Count);
            Assert.AreEqual(0, errors.Count);

            //TODO: better test of tokens?
        }

        [Test]
        public void IncorrectAnalysis1Test()
        {
            var analyzer = new LexicalAnalyzer(INCORRECT_CODE_1);
            var tokens = analyzer.GetAllTokens();
            var errors = analyzer.GetAllErrors();

            Assert.Greater(tokens.Count, 10);
            Assert.Greater(errors.Count, 0);
            //TODO: better test of tokens and erros?
        }

        [Test]
        public void IncorrectAnalysis2Test()
        {
            var analyzer = new LexicalAnalyzer(INCORRECT_CODE_2);
            var tokens = analyzer.GetAllTokens();
            var errors = analyzer.GetAllErrors();
            
            Assert.Greater(tokens.Count, 10);
            Assert.Greater(errors.Count, 0);
            //TODO: better test of tokens and erros?
        }

    }
}
