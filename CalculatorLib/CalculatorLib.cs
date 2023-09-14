using System.Diagnostics;
using Newtonsoft.Json;

namespace CalculatorLib
{
    public class Calculator
    {

        JsonWriter writer;

        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calculator.log");
            Trace.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();

        }
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
        //Takes two numbers, preforms the given operation, and returns file
        public double doOperation(double num1, double num2, string op)
        {
            //Default the result to NaN
            double result = double.NaN;

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
                    result = Math.Pow(num1, num2);
                    writer.WriteValue("Power");
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
            return result;
        }
    }
}
