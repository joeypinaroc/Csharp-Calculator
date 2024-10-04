using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Followed hint from the instructions document to parse input string
 * and check each character. I planned to store each char in a List.
 * The logic for traversing the lists and evaluating a math expression 
 * is taken from the following reference:
 *      1. https://www.geeksforgeeks.org/expression-evaluation/
 * Reference 1 used Stacks instead of Lists, to conveniently pop() the elements 
 * on top of a char list while traversing the Stack and performing the math operations.
 * 
 * Reference 2 below is from stackoverflow showing a ConvertToDouble method which I 
 * used to convert decimal numbers to double
 *      2. https://stackoverflow.com/questions/11399439/converting-string-to-double-in-c-sharp
 */
namespace Assgn01_Calculator
{
    internal class Program
    {
        static double Calculate(string input)
        {
            char[] chars = input.ToCharArray(); //split input expression into a char array

            Stack<double> numbers = new Stack<double>(); //Stack to store numbers from input expression
            Stack<char> operators = new Stack<char>(); //Stack to store operators/parentheses from input exp            
            
            for(int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == ' ') continue; //if char is whitespace, skip

                if (char.IsDigit(chars[i]) || chars[i] == '.') //check if current char is a digit OR a decimal point
                {
                    /* Reference 1 used a StringBuilder for multi-digit numbers */
                    StringBuilder sb = new StringBuilder();
                    bool hasDecimal = false; //to track if a decimal point is already found

                    while(i < chars.Length && (char.IsDigit(chars[i]) || (chars[i] == '.' && !hasDecimal)))
                    {
                        if (chars[i] == '.')
                        {
                            hasDecimal = true; //set flag once decimal is encountered
                        }
                        sb.Append(chars[i++]);
                    }
                    numbers.Push(ConvertStringToDouble(sb.ToString())); //convert multi-digit string to number and add to numbers stack
                    i--; //since i is now on the next digit, decrease i to correct the offset
                }

                else if (chars[i] == '(') //if char is opening bracket, push to operators
                {
                    operators.Push(chars[i]);
                }

                else if (chars[i] == ')') //if char is closing bracket, solve entire bracket
                {
                    /* keep doing ApplyOperator method inside bracket until all operators are popped */
                    while (operators.Peek() != '(') 
                    {
                        numbers.Push(ApplyOperator(operators.Pop(), numbers.Pop(), numbers.Pop()));
                    }
                    operators.Pop(); //pop opener bracket after solving bracket
                }

                else if (chars[i] == '+' || chars[i] == '-' || chars[i] == '*' || chars[i] == '/')
                {
                    /* if operators stack is empty, push current op to stack
                     * if the next op encountered has more priority(precedence) than the last pushed op,
                     * apply that operation on the last two numbers on the numbers stack */
                    while(operators.Count > 0 && HasPrecedence(chars[i], operators.Peek()))
                    {
                        numbers.Push(ApplyOperator(operators.Pop(), numbers.Pop(), numbers.Pop())); //apply current operator to the last 2 numbers on top of Stack
                    }
                    operators.Push(chars[i]);
                }
            }
            while(operators.Count > 0)
            {
                numbers.Push(ApplyOperator(operators.Pop(),numbers.Pop(), numbers.Pop()));
            }
            return numbers.Pop();
        }
        /* From Ref 1, method to compare if current op has precedence over op on top of stack */
        static bool HasPrecedence(char op1, char op2) 
        {
            if(op2 == '(' || op2 == ')')
            {
                return false;
            }
            if ((op1 == '*' || op1 == '/') && (op2 == '+' || op2 == '-'))
            {
                return false;
            }
            else return true;
        }
        /* From Ref 1, method to apply an operation to number b, the last number pushed to the numbers stack
         * and number a, the 2nd to the last number pushed to the numbers stack. 
         * Each return from ApplyOperator will be pushed to the numbers stack as seen in the Calculate() method, which
         * essentially mimics an expression tree as shown in the instructions document. Solving 1 operation and 2 numbers 
         * at a time.*/
        static double ApplyOperator(char op, double b, double a)
        {
            switch (op)
            {
                case '+': 
                    return a + b;
                case '-': 
                    return a - b;
                case '*': 
                    return a * b;
                case '/': 
                    if(b == 0)
                    {
                        throw new Exception("Cannot be divided by zero");
                    }
                    return a / b;
            }
            return 0;
        }
        /* Made method to convert input string, which can be decimal numbers, to double
         * from Reference 2*/
        static double ConvertStringToDouble(string str)
        {
            if (str == null) return 0;
            else
            {
                double output;
                double.TryParse(str, out output);
                if(double.IsNaN(output))
                {
                    return 0;
                }
                return output;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Calculator: ");
            Console.Write("Input: ");
            string input = (Console.ReadLine());

            double answer = Calculate(input);
            Console.WriteLine("Answer: " + answer);
           
        }
    }
}
