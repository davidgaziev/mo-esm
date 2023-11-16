/*
 * Author: Patrik Lundin, patrik@lundin.live
 * Web: http://www.lundin.live
 * 
 * Source code released under the Microsoft Public License (Ms-PL) 
 * http://www.microsoft.com/en-us/openness/licenses.aspx#MPL
*/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using info.lundin.math;
using System.Collections;

namespace MathParserUnitTests
{
    /// <summary>
    /// Verifies a number of test cases and "common" syntax errors.
    /// </summary>
    [TestClass]
    public class ParserSyntaxTests
    {
        [TestMethod]
        public void Parse_EmptyOrNUllExpression_Fail()
        {
            bool exception = false;

            var parser = new ExpressionParser();

            try
            {
                parser.Parse("");
            }
            catch (ParserException)
            {
                exception = true;
            }

            if (!exception) Assert.Fail("Null expression was allowed");
        }

        [TestMethod]
        public void Parse_MissingVariables_Fail()
        {
            bool exception = false;

            var parser = new ExpressionParser();

            try
            {
                parser.Parse("x+y");
            }
            catch (ParserException)
            {
                exception = true;
            }

            if (!exception) Assert.Fail("Missing variables in table wrongly allowed");
        }


        [TestMethod]
        public void Parse_MissingParentheses_Fail()
        {
            bool exception = false;

            var parser = new ExpressionParser()
            {
                RequireParentheses = true
            };

            try
            { 
                parser.Parse("cos5");
            }
            catch (ParserException)
            {
                exception = true;
            }

            if (!exception) Assert.Fail("Missing parentheses was wrongly allowed");
        }

        [TestMethod]
        public void Parse_UnbalancedParentheses_Fail()
        {
            bool exception = false;

            var parser = new ExpressionParser()
            {
                RequireParentheses = true
            };

            try
            {
                parser.Parse("cos(5)+x-sin(pi");
            }
            catch (ParserException)
            {
                exception = true;
            }

            if (!exception) Assert.Fail("Unbalanced parentheses wrongly allowed");
        }

        [TestMethod]
        public void Parse_MissingArguments_Fail()
        {
            bool exception = false;

            var parser = new ExpressionParser()
            {
                RequireParentheses = true
            };

            try
            {
                parser.Parse("1*5*");
            }
            catch (ParserException)
            {
                exception = true;
            }

            if (!exception) Assert.Fail("Missing argumets wrongly allowed");
        }

        [TestMethod]
        public void Parse_KeywordsInVariable_Fail()
        {
            bool exception = false;

            var parser = new ExpressionParser()
            {
                RequireParentheses = true,
                ImplicitMultiplication = true
            };

            try
            {
                parser.Values.Add("glucose", 5);
                parser.Parse("glucose*5");
            }
            catch (ParserException)
            {
                exception = true;
            }

            if (!exception) Assert.Fail("Function keyword in variable incorrectly allowed when ImplicitMultiplication set to true");
        }


        [TestMethod]
        public void Parse_KeywordsInVariable_Success()
        {
            bool exception = false;

            var parser = new ExpressionParser()
            {
                RequireParentheses = true,
                ImplicitMultiplication = false
            };

            try
            {
                parser.Values.Add("glucose", 5);
                parser.Parse("glucose*5");
            }
            catch (ParserException)
            {
                exception = true;
            }

            if (exception) Assert.Fail("Function keyword in variable should be allowed when ImplicitMultiplication set to false");
        }
    }
}
