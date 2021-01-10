using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeagenCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Basic calculation\n");
            Console.WriteLine("1 + 1 = " + Calculate("1 + 1")); // 2
            Console.WriteLine("2 * 2 = " + Calculate("2 * 2")); // 4
            Console.WriteLine("1 + 2 + 3 = " + Calculate("1 + 2 + 3")); // 6
            Console.WriteLine("6 / 2 = " + Calculate("6 / 2")); // 3
            Console.WriteLine("11 + 23 = " + Calculate("11 + 23")); // 34
            Console.WriteLine("11.1 + 23 = " + Calculate("11.1 + 23")); // 34.1
            Console.WriteLine("1 + 1 * 3 = " + Calculate("1 + 1 * 3") + "\n");  // 4
            Console.WriteLine("Calculation with brackets\n");
            Console.WriteLine("( 11.5 + 15.4 ) + 10.1 = " + Calculate("( 11.5 + 15.4 ) + 10.1"));   //  37
            Console.WriteLine("23 - ( 29.3 - 12.5 ) = " + Calculate("23 - ( 29.3 - 12.5 )") + "\n");    // 6.2
            Console.WriteLine("Nested Calculation\n");
            Console.WriteLine("10 - ( 2 + 3 * ( 7 - 5 ) ) = " + Calculate("10 - ( 2 + 3 * ( 7 - 5 ) )"));   // 2
            Console.WriteLine("( ( 9 * 2 ) - ( 4 / 8 ) ) + ( 5 - 3 ) = " + Calculate("( ( 9 * 2 ) - ( 4 / 8 ) ) + ( 5 - 3 )"));   // 19.5
            Console.WriteLine("( 2 / 7 ) + ( ( 4 + 2 * 8 ) / 2.5 ) = " + Calculate("( 2 / 7 ) + ( ( 4 + 2 * 8 ) / 2.5 )") + "\n");    // 8.29
            Console.WriteLine("Additional features\n");
            Console.WriteLine("Negative numbers\n");
            Console.WriteLine("- 3 - 8 + 5 = " + Calculate("- 3 - 8 + 5")); // -6
            Console.WriteLine("- 7 * 9 + 3 / 2 = " + Calculate("- 7 * 9 + 3 / 2") + "\n");   // -61.5
            Console.WriteLine("Unrecognized characters\n");
            Console.WriteLine(".asdsad6.+1 + 9 = " + Calculate(".asdsad6.+1 + 9")); // 10.6
            Console.WriteLine("7asa9 + a3 / 4td = " + Calculate("7asa9 + a3 / 4td") + "\n");   // 79.75
            Console.WriteLine("No number before decimal point\n");
            Console.WriteLine(".5 + .4 + 2.8 = " + Calculate(".5 + .4 + 2.8")); // 3.7
            Console.WriteLine("55 + .2 * 9 / 4 = " + Calculate("55 + .2 * 9 / 4") + "\n");   // 55.45
            Console.WriteLine("Multiple operators between numbers\n");  // Only take first operator
            Console.WriteLine("*-/ 8 ++ 5 = " + Calculate("*-/ 8 ++ 5")); // 13
            Console.WriteLine("65.2.3 - 8 6...7 / 7 * 3.. = " + Calculate("65.2.3 - 8 6...7 / 7 * 3..")); // 28.07
            Console.WriteLine("-49 /**-/- + 2 / 3 ) - = " + Calculate("-49 /**-/- + 2 / 3 ) -") + "\n");   // -8.17
            Console.WriteLine("Multiplication using brackets\n");
            Console.WriteLine("2 ( 4 ) = " + Calculate("2 ( 4 )")); // 8
            Console.WriteLine("( 7 ) 9 = " + Calculate("( 7 ) 9 ")); // 63
            Console.WriteLine("((2 + 3)(8 - 2)2(5))) = " + Calculate("((2 + 3)(8 - 2)2(5)))") + "\n");   // 300

            Console.ReadLine();
        }

        public static double Calculate(string sum)
        {
            // Initialize stack to store values
            Stack<double> values = new Stack<double>();

            // Initialize stack to store operators
            Stack<char> operators = new Stack<char>();

            // Remove white space and allow only numbers and selected characters
            string allowedOperators = "+-*/().";
            string selectedOperators = allowedOperators.Substring(0, 4);
            string bracketsOperators = allowedOperators.Substring(4, 2);
            string newSum = "";
            for (int i = 0; i < sum.Length; i++)
            {
                if (sum[i] != ' ' && (allowedOperators.IndexOf(sum[i]) != -1 || (sum[i] >= '0' && sum[i] <= '9')))
                {
                    newSum = newSum + sum[i];
                }
            }

            // Set token for each char in newSum
            char[] token = newSum.ToCharArray();

            // Function to calculate starts
            for (int i = 0; i < token.Length; i++)
            {
                // Check if char is open brace '('
                if (token[i] == bracketsOperators[0])
                {
                    // Add mulitplication operator if there is no previous operator
                    if (values.Count > 0 && (operators.Count == 0 || (operators.Count > 0 && operators.Peek() != bracketsOperators[0] && selectedOperators.IndexOf(operators.Peek()) < 0)))
                    {
                        operators.Push('*');
                    }

                    // Push brace to stack operators
                    operators.Push(token[i]);
                }

                // Check if char is close brace ')'
                else if (token[i] == bracketsOperators[1] && operators.Contains(bracketsOperators[0]))
                {
                    // Immediately calculate the numbers within the brackets
                    while (operators.Peek() != bracketsOperators[0])
                    {
                        values.Push(DoCalculation(operators.Pop(), values.Pop(), values.Pop()));
                    }

                    // Remove open brace '(' from stack operators
                    operators.Pop();

                    // Support multiplication without the operator "*"
                    i++;
                    if (i < token.Length && (token[i] == bracketsOperators[0] || (token[i] >= '0' && token[i] <= '9')))
                    {
                        operators.Push('*');
                    }
                    i--;
                }

                // Check if char contains number
                else if (token[i] >= '0' && token[i] <= '9')
                {
                    // Initialize stringbuilder to store multi digit numbers
                    StringBuilder stringBuilder = new StringBuilder();

                    // Set decimal point flag
                    bool isDecimalExist = false;

                    // Store number in stringBuilder
                    while (i < token.Length && token[i] >= '0' && token[i] <= '9')
                    {
                        stringBuilder.Append(token[i++]);

                        // Add decimal point if available
                        while (i < token.Length && token[i] == '.')
                        {
                            // Check if decimal statement is correct. E.g. 1.1, 2.3
                            // Will skip if have multi decimal
                            // 1..1 --> 1.1
                            // 2.3.2 --> 2.32
                            if (!isDecimalExist)
                            {
                                stringBuilder.Append(token[i++]);
                                isDecimalExist = true;
                            }
                            else
                            {
                                i++;
                            }
                        }

                        // Support multiplication without the operator "*"
                        if (i < token.Length && token[i] == bracketsOperators[0])
                        {
                            operators.Push('*');
                        }
                    }
                    i--;

                    // Push number to stack value
                    values.Push(double.Parse(stringBuilder.ToString()));
                }

                // Check if char contains operators
                else if (selectedOperators.IndexOf(token[i]) != -1)
                {
                    // Check if no values exist
                    if (values.Count < 1)
                    {
                        // If char is substraction, consider this as a negative number
                        if (token[i] == '-')
                        {
                            values.Push(0);
                            operators.Push(token[i++]);
                        }
                        // Else consider this as a positive number
                        // Take only the first operator if there are multiple operators before a number
                        while (i < token.Length && selectedOperators.IndexOf(token[i]) != -1)
                        {
                            i++;
                        }
                        i--;
                        continue;
                    }

                    // Check if previous operators could be supersede
                    while (operators.Count > 0 && IsPriority(token[i], operators.Peek()))
                    {
                        values.Push(DoCalculation(operators.Pop(), values.Pop(), values.Pop()));
                    }

                    // Push operator to stack operators
                    operators.Push(token[i++]);

                    // Skip if there is is multiple operators without empty space. E.g ++, +-, *+/
                    // Take only the first operator
                    while (i < token.Length && selectedOperators.IndexOf(token[i]) != -1)
                    {
                        i++;
                    }
                    i--;
                }

                // Check if char is decimal '.'
                else if (token[i] == '.')
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append('0');
                    stringBuilder.Append(token[i++]);

                    // Skip any decimal point or operators after
                    while (i < token.Length && token[i] == '.')
                    {
                        i++;
                    }

                    // Add digit zero before decimal
                    while (i < token.Length && token[i] >= '0' && token[i] <= '9')
                    {
                        stringBuilder.Append(token[i++]);

                        // Skip any decimal point after
                        while (i < token.Length && token[i] == '.')
                        {
                            i++;
                        }
                    }
                    i--;

                    // Push values to stack values
                    values.Push(double.Parse(stringBuilder.ToString()));
                }
            }

            // Calculate all values
            while (operators.Count > 0 && values.Count > 1)
            {
                values.Push(DoCalculation(operators.Pop(), values.Pop(), values.Pop()));
            }

            // Return value on top of the operators stack 
            return values.Pop();
        }

        public static bool IsPriority(char operatr1, char operatr2)
        {
            // Skip if previous operator is brackets
            // Or if current operator supersede previous operator            
            // Multiplication "*" and Division "/" always supersede Addition "+" and Substraction "-"
            bool isPriority = operatr2 == '(' || operatr2 == ')' ||
                            ((operatr1 == '*' || operatr1 == '/') && (operatr2 == '+' || operatr2 == '-'));
            return !isPriority;
        }

        public static double DoCalculation(char operatr, double num2, double num1)
        {
            double result = 0.0;
            switch (operatr)
            {
                case '+':
                    result = num1 + num2;
                    break;
                case '-':
                    result = num1 - num2;
                    break;
                case '*':
                    result = num1 * num2;
                    break;
                case '/':
                    if (num2 == '0')
                    {
                        // Throw exception cannot divide 0
                        throw new System.NotSupportedException("Cannot divide by zero");
                    }
                    result = num1 / num2;
                    break;
            }
            return Math.Round(result, 2);
        }
    }
}
