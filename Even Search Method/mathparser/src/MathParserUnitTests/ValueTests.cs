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

namespace MathParserUnitTests
{
    [TestClass]
    public class ValueTests
    {
        [TestMethod]
        public void Change_Value_Success()
        {
            ExpressionParser parser = new ExpressionParser();

            parser.Values.Add("x", 5);

            // This parses the expression and the values 
            // will be cached in the expression tree
            double value = parser.Parse("x+x+x");
            Assert.AreEqual(15, value);

            // Change value
            parser.Values["x"].SetValue(10);

            value = parser.Parse("x+x+x");
            Assert.AreEqual(30, value);
        }

        [TestMethod]
        public void Add_Remove_Value_Fail()
        {
            ExpressionParser parser = new ExpressionParser();

            parser.Values.Add("x", 5);

            // This parses the expression and the values 
            // will be cached in the expression tree
            double value = parser.Parse("x+x+x");
            Assert.AreEqual(15, value);

            // Remove value
            parser.Values.Remove("x");

            bool exception = false;

            try
            {
                value = parser.Parse("x+x+x");
            }
            catch(Exception)
            {
                exception = true;
            }

            if (!exception) Assert.Fail();
        }

        [TestMethod]
        public void Add_Remove_Add_Value_Success()
        {
            ExpressionParser parser = new ExpressionParser();

            parser.Values.Add("x", 5);

            // This parses the expression and the values 
            // will be cached in the expression tree
            double value = parser.Parse("x+x+x");
            Assert.AreEqual(15, value);

            // Remove value
            parser.Values.Remove("x");

            // Add value with same variable
            parser.Values.Add("x", 10);

            value = parser.Parse("x+x+x");

            Assert.AreEqual(30, value);
        }

        [TestMethod]
        public void Add_Clear_Add_Value_Success()
        {
            ExpressionParser parser = new ExpressionParser();

            parser.Values.Add("x", 5);

            // This parses the expression and the values 
            // will be cached in the expression tree
            double value = parser.Parse("x+x+x");
            Assert.AreEqual(15, value);

            // Remove value
            parser.Values.Clear();

            // Add value with same variable
            parser.Values.Add("x", 10);

            value = parser.Parse("x+x+x");

            Assert.AreEqual(30, value);
        }

        [TestMethod]
        public void Add_SetNull_Add_Value_Success()
        {
            ExpressionParser parser = new ExpressionParser();

            parser.Values.Add("x", 5);

            // This parses the expression and the values 
            // will be cached in the expression tree
            double value = parser.Parse("x+x+x");
            Assert.AreEqual(15, value);

            // Remove value
            parser.Values["x"] = null;

            // Add value with same variable
            parser.Values.Add("x", 10);

            value = parser.Parse("x+x+x");

            Assert.AreEqual(30, value);
        }

        [TestMethod]
        public void Add_Repeated_110()
        {
            ExpressionParser parser = new ExpressionParser();

            double total = 0;

            parser.Values.Add("x", 0);
            parser.Parse("x+x");

            Expression expression = parser.Expressions["x+x"];

            for (int i = 1; i <= 10; i++)
            {
                parser.Values["x"].SetValue(i);
                total += parser.EvalExpression(expression);
            }

            Assert.AreEqual(110, total);
        }

        /*
        [TestMethod]
        public void Add_Repeated_110()
        {
            ExpressionParser parser = new ExpressionParser();

            double total = 0;

            parser.Values.Add("x", 0.ToString());
            parser.Parse("x+x");

            Expression expression = parser.Expressions["x+x"];

            for (int i = 1; i <= 10; i++)
            {
                parser.Values["x"].SetValue(i.ToString());
                total += parser.EvalExpression(expression);
            }

            Assert.AreEqual(110, total);
        }*/
    }
}
