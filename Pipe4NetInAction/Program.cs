using System;
using System.Collections;
using System.Collections.Generic;
using Pipe4Net;

namespace Pipe4NetInAction
{
    class Program
    {
        static void Main(string[] args)
        {
            string baba = "Baba";

            baba.Pipe(Write);

            baba.PipeReturn(WriteR).Pipe(Write);

            IList<int> aa = new List<int>();

            Console.ReadKey();

            IEnumerable<int> bb = new List<int>();
        }

        static void Write(string val)
        {
            Console.WriteLine(val);
        }

        static string WriteR(string val)
        {
            Console.WriteLine(val);

            return val;
        }
    }
}
