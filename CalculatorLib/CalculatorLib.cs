using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace CalculatorLib
{
    public class Calculator
    {
        //Takes a math equation as a string and returns the result as a double
        public double parseOperationString(string input_original)
        {
            string input = "";
            double result = double.NaN;
            int index = 0;

            //Array of all the operations in the order they should be preformed
            string[] all_ops = { "^", "*", "/", "+", "-"};

            //Remove all spaces from the input
            input_original = Regex.Replace(input_original, @"s", "");

            //Handle brackets

            //find the first opening bracket
            int openingBracket = input_original.IndexOf('(');

            //If there is an opening bracket
            if(openingBracket != -1)
            {
                //Find the next closing bracket
                int closingBracket = input_original.IndexOf(")", openingBracket + 1);

                //If there is a closing bracket before the next opening bracket, ie the brackets are not nested
                if (input_original.IndexOf(")", openingBracket + 1) < input_original.IndexOf("(", openingBracket + 1) || input_original.IndexOf("(", openingBracket + 1) == -1)
                {
                    
                    //Append anything before the opening bracket to the input string if needed
                    if (openingBracket != 0)
                    {
                        input += input_original.Substring(0, openingBracket);
                    }

                    //Append the result of the operation inside the brackets to the input string
                    input += parseOperationString(input_original.Substring(openingBracket + 1, (closingBracket - openingBracket) - 1));

                    //Append anything after the closing bracket to the input string if needed
                    if ((closingBracket + 1) < input_original.Length)
                    {
                        input += input_original.Substring(closingBracket + 1, input_original.Length - (closingBracket + 1));
                    }

                    //Parse the remaining operation string
                    input = parseOperationString(input).ToString();

                }
                else //We have nested brackets
                {
                    if (openingBracket != 0) //Append anything before the opening bracket to the input string if needed
                    {
                        input += input_original.Substring(0, openingBracket);
                    }
                    //Append the result of the operation after the opening bracket to the input string
                    input += parseOperationString(input_original.Substring(openingBracket + 1));
                }
                
            }
            else//No brackets
            {
                input = input_original;

            }

            //Remove any extra closing brackets
            if (input.IndexOf(")") != -1)
            {
                input = input.Remove(input.IndexOf(")"), 1);
            }

            //Split the input string into an array of numbers and operations
            string[] input_array = Regex.Split(input, @"([*()\^\/]|(?<!E)[\+\-])");

            //Handle negative numbers

            //remove empty strings from input_array(something is adding them for some reason)
            input_array = input_array.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            //Use a list to make it easier to remove/add items
            List<string> input_list = input_array.ToList();

            int minusIndex;
            int oldIndex = -1;
            do//For each minus sign in the input list
            {
                //Get the index of the next minus sign
                minusIndex = input_list.IndexOf("-", oldIndex + 1);
                oldIndex = minusIndex;

                //If the minus sign is not the first item in the input list
                if (minusIndex > 0 && minusIndex < input_list.Count)
                {
                    //Get the item before and after the minus sign
                    string firstItem = input_list[minusIndex - 1];
                    string secondItem = input_list[minusIndex];
                    string thirdItem = input_list[minusIndex + 1];

                    //If the item before the minus sign is an operation or a bracket, we assume that the minus sign is a negative sign
                    if (all_ops.Contains(firstItem) || firstItem == "(" || firstItem == ")")
                    {
                        //Combine the minus sign and the number after it
                        string newNumber = "-" + thirdItem;
                        //Remove the second and third items and replace them with the new number
                        input_list.RemoveAt(minusIndex);
                        input_list.RemoveAt(minusIndex);

                        //Add the new number to the list
                        input_list.Insert(minusIndex, newNumber);

                    }
                }else if(minusIndex == 0 && input_list.Count == 2) //If the minus sign is the first item in the input list, we assume that the minus sign is a operator
                {
                    //Combine the minus sign and the number after it
                    string newNumber = "-" + input_list[1];

                    //Remove the two items and replace them with the new number
                    input_list.RemoveAt(0);
                    input_list.RemoveAt(0);
                    input_list.Insert(0, newNumber);
                }

            } while (minusIndex != -1);

            //Convert the list back to an array
            input_array = input_list.ToArray();

            //If the input array has more than 2 items, we need to do some operations
            if (input_array.Length > 2)
            {
                //Preform the the operations in the input array
                string[] final_array = doFirstOp(input_array, all_ops);

                //Parse the result to a double
                if(double.TryParse(final_array[0], out result))
                {
                    return result;
                }

                return result;
            }else if(input_array.Length == 1)//If the input array has only one item, it is the result
            {
                //Parse the result to a double
                double.TryParse(input_array[0], out result);
                return result;
            }
            else//If the input array has no items, or one item, we return NaN
            {
                return result;
            }
        }

        //Does the first operation it finds in the input array respecting order of operations
        private string[] doFirstOp(string[] inputArray, string[] ops)
        {
            List<string> outputList = new List<string>();
            int i = 0;
            bool found_op = false;

            //Find the first operation respecting order of operations
            while(i < ops.Length && found_op == false)
            {
                int op_index = Array.IndexOf(inputArray, ops[i]);

                //Once the we have found an operation
                if (op_index != -1)
                {
                    found_op = true;

                    double num1 = 0;
                    double num2 = 0;
                    //Attempt to parse the numbers on either side of the operation
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
                    //Preform the operation and convert the result to a string
                    string[] result = { doOperation(num1, num2, inputArray[op_index]).ToString() };

                    //Create arrays of the items before and after the operation and the result of the operation and combine them into one list
                    ArraySegment<string> firstArraySeg = new ArraySegment<string>(inputArray, 0, op_index - 1).ToArray();
                    ArraySegment<string> secondArraySeg = new ArraySegment<string>(result).ToArray();
                    ArraySegment<string> thirdArraySeg = new ArraySegment<string>(inputArray, op_index + 2, (inputArray.Length - (op_index + 2))).ToArray();
                    outputList = firstArraySeg.Concat(secondArraySeg).Concat(thirdArraySeg).ToList<string>();
                }
                i++;
            }
            //Convert the list to an array
            string[] outputArray = outputList.ToArray();

            //If the output array only has one item, we are done all the operations
            if(outputArray.Length == 1)
            {
                return outputArray;
            }

            else//We need to do more operations
            {
                return doFirstOp(outputArray, ops);
            }
        }

        //Takes two numbers, preforms the given operation, and returns the result
        private double doOperation(double num1, double num2, string op)
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
