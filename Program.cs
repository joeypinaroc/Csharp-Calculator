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
 */
namespace Assgn01_Calculator
{
    internal class Program
    {
        static double Calculate(string input)
        {
            Stack<double> numbers = new Stack<double>(); //Stack to store numbers from input expression
            Stack<char> operators = new Stack<char>(); //Stack to store operators/parentheses from input exp            

            char[] chars = input.ToCharArray(); //split input expression into a char array
            for(int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == ' ') continue; //if char is whitespace, skip

                if (char.IsDigit(chars[i])) //check if current char is a digit
                {
                    /* Reference 1 used a StringBuilder to create multi-digit numbers */
                    StringBuilder sb = new StringBuilder();
                    while(i < chars.Length && char.IsDigit(chars[i]))
                    {
                        sb.Append(chars[i++]);
                    }
                    numbers.Push(int.Parse(sb.ToString())); //convert multi-digit string to int and add to numbers list
                    i--; //since i is now on the next digit, decrease i to correct the offset
                }

                else if (chars[i] == '+' || chars[i] == '-' || chars[i] == '*' || chars[i] == '/')
                {
                    while(operators.Count > 0)
                    {
                        numbers.Push(ApplyOperator(operators.Pop(), numbers.Pop(), numbers.Pop()));
                    }
                    operators.Push(chars[i]);
                    //numbers.Push(ApplyOperator(operators.Pop(), numbers.Pop(), numbers.Pop())); //apply current operator to the last 2 numbers on top of Stack
                }
            }
            PrintIntList(numbers);
            PrintCharList(operators);
            while(operators.Count > 0)
            {
                numbers.Push(ApplyOperator(operators.Pop(),numbers.Pop(), numbers.Pop()));
            }
            return numbers.Pop();

        }
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
        static void PrintIntList(Stack<double> list)
        {
            foreach(var item in list)
            {
                Console.WriteLine(item);
            }
        }
        static void PrintCharList(Stack<char> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Calculator: ");
            Console.Write("Input: ");
            string input = (Console.ReadLine());

            Calculate(input);
           
        }
    }
}
