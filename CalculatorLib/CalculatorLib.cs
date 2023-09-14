using System.Diagnostics;

using Newtonsoft.Json;

namespace CalculatorLib
{
    public class Calculator
    {
        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calculator.log");
            Trace.Listeners.Add(new TextWriterTraceListener(logFile));
            Trace.AutoFlush = true;
            Trace.WriteLine("Starting Calculator Log");
            Trace.WriteLine($"Started @ {System.DateTime.Now.ToString()}");

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
                    Trace.WriteLine($"{num1} {op} {num2} = {result}");
                    break;
                case "-":
                    //Subtract
                    result = num1 - num2;
                    Trace.WriteLine($"{num1} {op} {num2} = {result}");
                    break;
                case "*":
                    //Multiply
                    result = num1 * num2;
                    Trace.WriteLine($"{num1} {op} {num2} = {result}");
                    break;
                case "/":
                    //Divide
                    //Don't divide by zero
                    if (num2 != 0)
                    {
                        Trace.WriteLine($"{num1} {op} {num2} = {result}");
                        result = num1 / num2;
                    }
                    else
                    {
                        Trace.WriteLine("Cannot Divide By Zero");
                    }
                    break;
            }

            return result;
        }
    }
}
