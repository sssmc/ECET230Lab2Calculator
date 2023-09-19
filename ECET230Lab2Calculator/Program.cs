//ECET 230 - Sebastien Robitaille

//Commandline Calculator supporting add, subtract, multiply, divide, and power operations

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
            //Ask User for the equation
            Console.Write("Enter Equation: ");
            string equation = Console.ReadLine();

            //Parse the equation
            double result = calculator.parseOperationString(equation);
            Console.WriteLine($"Result: {result}");
            Console.WriteLine("------------------------------------------------------");

            //Check if user want to exit or continue
            Console.Write("Enter x to exit program or any other key to continue: ");
            string input = Console.ReadLine();
            if(input == "x"){
                stopProgram = true;
            }
        }
        return;
    }
}
