using System.Collections.Generic;

namespace parserDecimal.Parser
{
    class Computer
    {
        Operands operands = new Operands();
        Polish polish = new Polish();
        Calculator calculator = new Calculator();

        internal decimal Compute(string function, decimal value)
        {
            List<string> splitExpression = operands.returnSplitExpression(function.Replace(" ", ""));
            Queue<string> revpExpression = polish.returnPolish(splitExpression);

            decimal answer = calculator.calculate(revpExpression, value);
            return answer;
        }
    }
}
