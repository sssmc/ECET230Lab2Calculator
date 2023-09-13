class Calculator
{
    public static double doOperation(double num1, double num2, string op)
    {
        double result = double.NaN;

        switch (op)
        {
            case "+":
                result = num1 + num2;
                break;
            case "-":
                result = num1 - num2;
                break;
            case "*":
                result = num1 * num2;
                break;
            case "/":
                if (num2 != 0)
                {
                    result = num1 / num2;
                }
                break;
        }

        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var stopProgram = false;
        Console.WriteLine("Welcome to the Calculator App by Sebastien Robitaille");
        Console.WriteLine("-----------------------------------------------------");
        while (!stopProgram)
        {
            Console.Write("Enter First Number: ");
            string num1String = Console.ReadLine();
            double num1;
            while (!double.TryParse(num1String, out num1))
            {
                Console.Write("Input is not a number, try again: ");
                num1String = Console.ReadLine();
            }

            Console.Write("Enter Second Number: ");
            string num2String = Console.ReadLine();
            double num2;
            while (!double.TryParse(num2String, out num2))
            {
                Console.Write("Input is not a number, try again: ");
                num2String = Console.ReadLine();
            }

            Console.WriteLine(@"Select from the following operation
                                + - Add
                                - - Subtract
                                * - Multiply
                                / - Devide");
            Console.Write("Enter Operation: ");
            string op = Console.ReadLine();
            double result = Calculator.doOperation(num1, num2, op);
            if (double.IsNaN(result))
            {
                Console.WriteLine("Invailid Math Operation");
            }
            else
            {
                Console.WriteLine($"{num1} {op} {num2} = {result}");
            }
            Console.WriteLine("------------------------------------------------------");
            Console.Write("Enter x to exit program or any other key to continue: ");
            string input = Console.ReadLine();

            if(input == "x"){
                stopProgram = true;
            }
        }
    }
}
