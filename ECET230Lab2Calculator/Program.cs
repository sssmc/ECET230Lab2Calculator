using System.Diagnostics;

using CalculatorLib;

class Program
{
    static void Main(string[] args)
    {
        Calculator calculator = new Calculator();

        var stopProgram = false;

        Console.WriteLine("Welcome to the Calculator App by Sebastien Robitaille");
        Console.WriteLine("-----------------------------------------------------");

        while (!stopProgram)
        {
            //Ask User for first number
            Console.Write("Enter First Number: ");
            string num1String = Console.ReadLine();
            double num1;
            //Loop until user inputs a valid double
            while (!double.TryParse(num1String, out num1))
            {
                Console.Write("Input is not a number, try again: ");
                num1String = Console.ReadLine();
            }

            //Ask user for second number
            Console.Write("Enter Second Number: ");
            string num2String = Console.ReadLine();
            double num2;
            //Loop until user inputs a valid double
            while (!double.TryParse(num2String, out num2))
            {
                Console.Write("Input is not a number, try again: ");
                num2String = Console.ReadLine();
            }

            //Ask user for operation
            Console.WriteLine(@"Select from the following operation
             + - Add
            - - Subtract
            * - Multiply
            / - Divide
            ^ - power(first number to the second num");
            Console.Write("Enter Operation: ");
            string op = Console.ReadLine();

            //Calculate the result
            double result = calculator.doOperation(num1, num2, op);
            //Check if result is Nan(tried to divide by zero)
            if (double.IsNaN(result))
            {
                Console.WriteLine("Invailid Math Operation");
            }
            else
            {
                //Output result
                Console.WriteLine($"{num1} {op} {num2} = {result}");
            }
            
            Console.WriteLine("------------------------------------------------------");

            //Check if user want to exit or continue
            Console.Write("Enter x to exit program or any other key to continue: ");
            string input = Console.ReadLine();
            if(input == "x"){
                stopProgram = true;
            }
        }
        calculator.Finish();
        return;
    }
}
