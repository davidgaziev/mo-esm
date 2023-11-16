/*
 * Author: Patrik Lundin, patrik@lundin.live
 * Web: http://www.lundin.live
 * 
 * Source code released under the Microsoft Public License (Ms-PL) 
 * http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using info.lundin.math;

namespace MathParserUnitTests
{
    /// <summary>
    /// Tests some "common" expressions
    /// </summary>
    [TestClass]
    public class ParserEvaluationTests
    {
        ExpressionParser parser;

        [TestInitialize]
        public void Initialize()
        {
            parser = new ExpressionParser();
        }
        
        [TestMethod]
        public void Parser_OnePlustwoEqualsThree()
        {
           Assert.AreEqual(3, parser.Parse("1+2"));
        }

        [TestMethod]
        public void Parser_OnePlustwoEqualsThreeVariables()
        {
            parser.Values.Add("x", 1);
            parser.Values.Add("y", 2);

            Assert.AreEqual(3, parser.Parse("x+y"));

            parser.Values.Clear();
        }

        [TestMethod]
        public void Parser_SciNotation_1000()
        {
            Assert.AreEqual(1000, parser.Parse("1E3"));
        }

        [TestMethod]
        public void Parser_SciNotation_OneThousandth()
        {
            double delta = 1.22464679914737E-16;
            Assert.AreEqual(0.001, parser.Parse("1/1E3"), delta);
        }

        [TestMethod]
        public void Parser_SciNotation_1234()
        {
            Assert.AreEqual(1234, parser.Parse("1.234E3"));
        }

        [TestMethod]
        public void Parser_SciNotation_1235()
        {
            Assert.AreEqual(1235, parser.Parse("1E3+2.35E2"));
        }

        [TestMethod]
        public void Parser_UnaryMinus_NegOne()
        {
            Assert.AreEqual(-1, parser.Parse("-1"));
        }

        [TestMethod]
        public void Parser_UnaryPlus_1()
        {
            Assert.AreEqual(1, parser.Parse("+1"));
        }

        [TestMethod]
        public void Parser_Expression1_22()
        {
            parser.Values.Add("x", 10);
            parser.Values.Add("y", 4);
            parser.Values.Add("z", 2);

            Assert.AreEqual(22, parser.Parse("x + 2(y + z)"));
        }


        [TestMethod]
        public void Parser_Expression2_72()
        {
            parser.Values.Add("x", 10);
            parser.Values.Add("y", 4);
            parser.Values.Add("z", 2);

            Assert.AreEqual(72, parser.Parse("(x + 2)(y + z)"));
        }

        [TestMethod]
        public void Parser_Expression3_96()
        {
            parser.Values.Add("x", 10);
            parser.Values.Add("y", 4);
            parser.Values.Add("z", 2);

            Assert.AreEqual(96, parser.Parse("x^2 - y^2 + 3z^2"));
        }

        [TestMethod]
        public void Parser_Expression4_1_68()
        {
            parser.Values.Add("x", 10);
            parser.Values.Add("y", 4);
            parser.Values.Add("z", 2);

            Assert.AreEqual(1.68, parser.Parse("(10y^2 + z^3) / x^2"));
        }

        [TestMethod]
        public void Parser_Expression5_1()
        {
            parser.Values.Add("x", 10);
            parser.Values.Add("y", 4);

            Assert.AreEqual(1, parser.Parse("sin(x)^2+cos(x)^2"));
        }

        [TestMethod]
        public void Parser_Expression6_Neg184()
        {
            Assert.AreEqual(-184, parser.Parse("(3)(-4)^3 - (-2)(4)"));
        }

        [TestMethod]
        public void Parser_NestedExpression1_1_68()
        {
            parser.Values.Add("x", 10);
            parser.Values.Add("y", 4);
            parser.Values.Add("z", 2);
            parser.Values.Add("n", "(10y^2 + z^3)");

            Assert.AreEqual(1.68, parser.Parse("n / x^2"));
        }

        [TestMethod]
        public void Parser_NestedExpression2_1_68()
        {
            parser.Values.Add("x", 10);
            parser.Values.Add("y", 4);
            parser.Values.Add("z", 2);
            parser.Values.Add("n1", "10y^2");
            parser.Values.Add("n2", "z^3");
            parser.Values.Add("n", "(n1 + n2)");

            Assert.AreEqual(1.68, parser.Parse("n / x^2"));
        }

        [TestMethod]
        public void Parser_KeywordsInVariables_1_68()
        {
            parser.RequireParentheses = true;
            parser.ImplicitMultiplication = false;

            parser.Values.Add("glucose", 10);
            parser.Values.Add("y", 4);
            parser.Values.Add("z", 2);
            parser.Values.Add("sin", "10*y^2");
            parser.Values.Add("n2", "z^3");
            parser.Values.Add("n", "(sin + n2)");

            Assert.AreEqual(1.68, parser.Parse("n / glucose^2"));
        }

        [TestMethod]
        public void Parser_LogicExpression1_True()
        {
            parser.Values.Add("x", 2);
            parser.Values.Add("y", -2);

            Assert.IsTrue(parser.Parse("!(x+y)") == 1);
        }

        [TestMethod]
        public void Parser_LogicExpression2_True()
        {
            parser.Values.Add("x", "true");
            parser.Values.Add("y", "false");

            Assert.IsTrue(parser.Parse("x && !y") == 1);
        }

        [TestMethod]
        public void Parser_LogicExpression3_True()
        {
            parser.Values.Add("x", "true");
            parser.Values.Add("y", "cos(pi/2)");

            Assert.IsTrue(parser.Parse("x*-cos(pi) && !y") == 1);
        }

        [TestMethod]
        public void Parser_LogicExpression4_True()
        {
            parser.Values.Add("x", "true");
            parser.Values.Add("y", "cos(pi/2)");

            Assert.IsTrue(parser.Parse("!cos(pi) && !y") == 1);
        }

        [TestMethod]
        public void Parser_LogicExpression5_True()
        {
            parser.Values.Add("x", "true");
            parser.Values.Add("y", "false");

            Assert.IsTrue(parser.Parse("x || y") == 1);
        }


        [TestMethod]
        public void Parser_LogicExpression6_False()
        {
            parser.Values.Add("x", "true");
            parser.Values.Add("y", "false");

            Assert.IsTrue(parser.Parse("(x || y) && false") == 0);
        }

        [TestMethod]
        public void Parser_LogicExpression7_True()
        {
            parser.Values.Add("x", "true");
            parser.Values.Add("y", "false");

            Assert.IsTrue(parser.Parse("(x || y) && (y || x)") == 1);
        }
    }
}
