namespace CalculatorLib
{
    public class Calculator
    {
        //Takes two numbers, preforms the given operation, and returns file
        public static double doOperation(double num1, double num2, string op)
        {
            //Default the result to NaN
            double result = double.NaN;
            switch (op)
            {
                case "+":
                    //Add
                    result = num1 + num2;
                    break;
                case "-":
                    //Subtract
                    result = num1 - num2;
                    break;
                case "*":
                    //Multiply
                    result = num1 * num2;
                    break;
                case "/":
                    //Divide
                    //Don't divide by zero
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    break;
            }

            return result;
        }
    }
}
