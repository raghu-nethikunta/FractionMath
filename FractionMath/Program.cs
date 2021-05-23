using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractionMath
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "? 2_3/8 + 9/8";
            Stack<FractionMath> st = new Stack<FractionMath>();
            string op = string.Empty;
            string[] inputParams = input.Split(new char[] { ' ' });
            foreach (string param in inputParams)
            {
                switch (param)
                {
                    case "?":
                        break;
                    case "+":
                        op = "+";
                        break;
                    case "-":
                        op = "+";
                        break;
                    case "*":
                        op = "*";
                        break;
                    case "/":
                        op = "/";
                        break;
                    default:
                        st.Push(FractionMath.GetFraction(param));
                        break;

                }
            }

            switch (op)
            {
                case "+":
                    Console.WriteLine(st.Pop() + st.Pop());
                    break;
                case "-":
                    Console.WriteLine(st.Pop() - st.Pop());
                    break;
                case "*":
                    Console.WriteLine(st.Pop() * st.Pop());
                    break;
                case "/":
                    Console.WriteLine(st.Pop() / st.Pop());
                    break;
                default:
                    break;
            }
            Console.ReadLine();
        }

    }
}
