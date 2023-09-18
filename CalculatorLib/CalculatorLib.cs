using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace CalculatorLib
{
    public class Calculator
    {

        public Calculator()
        {

        }

        public double parseOperationString(string input_original)
        {
            string input = "";
            double result = double.NaN;
            int index = 0;
            string[] all_ops = { "^", "*", "/", "+", "-"};

            input_original = Regex.Replace(input_original, @"s", "");

            //Brackets

            int openingBracket = input_original.IndexOf('(');
            if(openingBracket != -1)
            {
                int closingBracket = input_original.IndexOf(")", openingBracket + 1);
                if (input_original.IndexOf(")", openingBracket + 1) < input_original.IndexOf("(", openingBracket + 1) || input_original.IndexOf("(", openingBracket + 1) == -1)
                {
                  
                    if (openingBracket != 0)
                    {
                        input += input_original.Substring(0, openingBracket);
                    }

                    input += parseOperationString(input_original.Substring(openingBracket + 1, (closingBracket - openingBracket) - 1));

                    if ((closingBracket + 1) < input_original.Length)
                    {
                        input += input_original.Substring(closingBracket + 1, input_original.Length - (closingBracket + 1));
                    }

                    input = parseOperationString(input).ToString();

                }
                else
                {
                    if (openingBracket != 0)
                    {
                        input += input_original.Substring(0, openingBracket);
                    }
                    input += parseOperationString(input_original.Substring(openingBracket + 1));
                }
                
            }
            else
            {
                input = input_original;

            }

            if (input.IndexOf(")") != -1)
            {
                input = input.Remove(input.IndexOf(")"), 1);
            }

            string[] input_array = Regex.Split(input, @"([*()\^\/]|(?<!E)[\+\-])");

            if (input_array.Length > 2)
            {
                string[] final_array = doFirstOp(input_array, all_ops);

                if(double.TryParse(final_array[0], out result))
                {
                    return result;
                }

                return result;
            }else if(input_array.Length == 1)
            {
                double.TryParse(input_array[0], out result);
                return result;
            }
            else
            {
                return result;
            }
        }

        private string[] doFirstOp(string[] inputArray, string[] ops)
        {
            List<string> outputList = new List<string>();
            int i = 0;
            bool found_op = false;

            while(i < ops.Length && found_op == false)
            {
                int op_index = Array.IndexOf(inputArray, ops[i]);
                if (op_index != -1)
                {
                    found_op = true;

                    double num1 = 0;
                    double num2 = 0;
                    if(!double.TryParse(inputArray[op_index - 1], out num1))
                    {
                        string[] errorOut = { "NaN" };
                        return errorOut;
                    }
                    if(!double.TryParse(inputArray[op_index + 1], out num2))
                    {
                        string[] errorOut = { "NaN" };
                        return errorOut;
                    }
                    string[] result = { doOperation(num1, num2, inputArray[op_index]).ToString() };
                    ArraySegment<string> firstArraySeg = new ArraySegment<string>(inputArray, 0, op_index - 1).ToArray();
                    ArraySegment<string> secondArraySeg = new ArraySegment<string>(result).ToArray();
                    ArraySegment<string> thirdArraySeg = new ArraySegment<string>(inputArray, op_index + 2, (inputArray.Length - (op_index + 2))).ToArray();
                    outputList = firstArraySeg.Concat(secondArraySeg).Concat(thirdArraySeg).ToList<string>();
                }
                i++;
            }
            string[] outputArray = outputList.ToArray();
            if(outputArray.Length == 1)
            {
                return outputArray;
            }

            else
            {
                return doFirstOp(outputArray, ops);
            }
        }

        //Takes two numbers, preforms the given operation, and returns file
        public double doOperation(double num1, double num2, string op)
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
                case "^":
                    //Power
                    result = Math.Pow(num1, num2);
                    break;
            }
            return result;
        }
    }
}
