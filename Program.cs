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
 */
namespace Assgn01_Calculator
{
    internal class Program
    {
        static void Calculate(string input)
        {
            List<int> numbers = new List<int>(); // List to store numbers from input expression
            List<char> operators = new List<char>(); // List to store operators/parentheses from input exp            

            char[] chars = input.ToCharArray(); // split input expression into a char array
            for(int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == ' ') continue; // if char is whitespace, skip

                if (char.IsDigit(chars[i])) // check if current char is a digit
                {
                    /* Reference 1 used a StringBuilder to create multi-digit numbers */
                    StringBuilder sb = new StringBuilder();
                    while(i < chars.Length && char.IsDigit(chars[i]))
                    {
                        sb.Append(chars[i++]);
                    }
                    numbers.Add(int.Parse(sb.ToString())); //convert multi-digit string to int and add to numbers list
                    i--; // since i is now on the next digit, decrease i to correct the offset
                }

                else if (chars[i] == '+' || chars[i] == '-' || chars[i] == '*' || chars[i] == '/')
                {
                    operators.Add(chars[i]);
                }
            }
            PrintIntList(numbers);
            PrintCharList(operators);

        }
        static void PrintIntList(List<int> list)
        {
            foreach(var item in list)
            {
                Console.WriteLine(item);
            }
        }
        static void PrintCharList(List<char> list)
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
