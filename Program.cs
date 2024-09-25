using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assgn01_Calculator
{
    public class Numbers
    {
        public int Number { get; set; }
    }
    internal class Program
    {
        static void Calculate(string input)
        {
            //Numbers num = new Numbers();
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
