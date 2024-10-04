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
 *
 * The plan is to:
 *  - take input expression as string
 *  - parse each char of the string and check if it is a number or an operator
 *  - use 2 stacks to store numbers and operators
 *  - once a number is encountered, push it to the numbers stack
 *  - if an operator, or a parenthesis, is encountered, and the operators stack is empty, push that operator 
 *    to the operators stack
 *  - if another operator is encountered, check if it has more priority(precedence) than the last pushed operator
 *      - if it has precedence, push it to operators stack
 *      - if not, meaning the last pushed operator has precedence than the current, then proceed to perform that operation first
 *  - once the entire expression has been parsed, proceed to performing the operations remaining
 *  - performing the operation would involve the operator on top of the operators stack, and the last 2 numbers on top of the numbers stack
 *      - pop the operators stack, and pop the numbers stack twice, and perform this operation
 *  - each result from this would be pushed to the numbers stack, and so on, and return the last remaining number in the numbers stack
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

                /* If char is '-' and is either the 1st char in the string,  is following an operator, or is following a whitespace, then it is a negative
                 * number sign. */
                if (chars[i] == '-' && (i == 0 || (chars[i-1] == '+' || chars[i-1] == '-' || chars[i-1] == '*' || chars[i-1] == '/') || chars[i-1] == '(' || chars[i-1] == ' '))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(chars[i++]); //add - sign
                    bool hasDecimal = false; //track decimal

                    while(i < chars.Length && (char.IsDigit(chars[i]) || (chars[i] == '.' && !hasDecimal)))
                    {
                        if (chars[i] == '.')
                        {
                            hasDecimal = true;
                        }
                        sb.Append(chars[i++]);
                    }
                    numbers.Push(ConvertStringToDouble(sb.ToString()));
                    i--;
                }

                else if (chars[i] == '(') //if char is opening bracket, push to operators
                {
                    operators.Push(chars[i]);
                }

                else if (chars[i] == ')') //if char is closing bracket, solve entire bracket
                {
                    /* Keep doing ApplyOperator method inside bracket until all operators are popped */
                    while (operators.Peek() != '(') 
                    {
                        numbers.Push(ApplyOperator(operators.Pop(), numbers.Pop(), numbers.Pop()));
                    }
                    operators.Pop(); //pop opener bracket after solving bracket
                }

                else if (chars[i] == '+' || chars[i] == '-' || chars[i] == '*' || chars[i] == '/')
                {
                    /* If operators stack is empty, push current op to stack
                     * If the next op encountered has more priority(precedence) than the last pushed op, push that op to top of operators stack 
                     * If the next op has less priority, then perform ApplyOperator on the last pushed op, which should have more precedence than the current op*/
                    while (operators.Count > 0 && HasPrecedence(chars[i], operators.Peek()))
                    {
                        numbers.Push(ApplyOperator(operators.Pop(), numbers.Pop(), numbers.Pop())); //apply current operator to the last 2 numbers on top of Stack
                    }
                    operators.Push(chars[i]);
                }
            }
            /* At this point, the entire input expression has been parsed and all the chars are in stacks. Now while there are ops in the operators stack,
             * perform ApplyOperator() */
            while(operators.Count > 0)
            {
                numbers.Push(ApplyOperator(operators.Pop(),numbers.Pop(), numbers.Pop()));
            }
            return numbers.Pop();
        }
        /* From Ref 1: method to compare if current op has precedence over op on top of stack */
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
        /* From Ref 1: method to apply an operation to number b, the last number pushed to the numbers stack
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
