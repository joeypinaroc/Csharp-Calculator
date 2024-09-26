using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assgn01_Calculator
{
    internal class Program
    {
        
        static void Calculate(string input)
        {
            /*
            foreach(char c in input)
            {
                int a;
                if (char.IsDigit(c))
                {
                    a = int.Parse(Convert.ToString(c)); //take each member of the string array

                    Console.WriteLine(a);
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    Console.WriteLine("Character");
                }
            }
            */
            List<int> numbers = new List<int>(); //List to store numbers from input expression
            List<char> operators = new List<char>(); //List to store operators/parentheses from input exp            

            char[] chars = input.ToCharArray(); //split input expression into a char array
            for(int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == ' ') continue; // if char is whitespace, skip
                if (char.IsDigit(chars[i]))
                {
                    numbers.Add(int.Parse(chars[i].ToString())); //convert i to int and add to numbers list
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
