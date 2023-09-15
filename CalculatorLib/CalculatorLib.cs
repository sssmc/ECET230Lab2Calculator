using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CalculatorLib
{
    public class Calculator
    {

        JsonWriter writer;

        public Calculator()
        {
            //Start JSON logging
            StreamWriter logFile = File.CreateText("calculator.json");
            Trace.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();

        }
        //Finish the JSON log file
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }

        public double parseOperationString(string input)
        {
            double result = double.NaN;
            int index = 0;

            string[] all_ops = { "*", "/", "+", "-"};

            string[] input_array = Regex.Split(input, @"([*()\^\/]|(?<!E)[\+\-])");

            string[] final_array = doFirstOp(input_array, all_ops);

            double.TryParse(final_array[0], out result);

            return result;
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
                    double.TryParse(inputArray[op_index - 1], out num1);
                    double.TryParse(inputArray[op_index + 1], out num2);
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

            //JSON logging
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");

            switch (op)
            {
                case "+":
                    //Add
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;
                case "-":
                    //Subtract
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;
                case "*":
                    //Multiply
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;
                case "/":
                    //Divide
                    //Don't divide by zero
                    if (num2 != 0)
                    {
                        writer.WriteValue("Divide");
                        result = num1 / num2;
                    }
                    else
                    {
                        writer.WriteValue("NaN");
                    }
                    break;
                case "^":
                    //Power
                    result = Math.Pow(num1, num2);
                    writer.WriteValue("Power");
                    break;
            }
            //JSON logging
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
            return result;
        }
    }
}
