using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Pipe4Net;

namespace Pipe4NetInAction
{
    class Program
    {
        static void Main(string[] args)
        {
            var array = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
            var array2 = array.DeepCopy();
            var array3 = array.ShallowCopy();
            array[2] = 11;

            array.ForEach(x => Console.Write(x + "\t"));
            Console.WriteLine(Environment.NewLine);

            array2.ForEach(x => Console.Write(x + "\t"));
            Console.WriteLine(Environment.NewLine);

            array3.ForEach(x => Console.Write(x + "\t"));
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Are same by reference: " + array.AreSameByReference(array3));
            Console.WriteLine("Are same by reference: " + array.AreSameByReference(array2));

            Console.WriteLine("Are same by value: " + array.AreSameByValue(array3, (a,b) => a == b));
            Console.WriteLine("Are same by value: " + array.AreSameByValue(array2, (a, b) => a == b));
            Console.ReadKey();
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
