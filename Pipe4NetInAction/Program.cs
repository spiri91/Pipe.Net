using Pipe4Net;
using System;

namespace Pipe4NetInAction
{
    class Program
    {
        static void Main(string[] args)
        {

            Return33().Pipe(x => x.ToString(), WriteR);


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

            Console.WriteLine("Are same by reference: " + array.IsSameByReference(array3));
            Console.WriteLine("Are same by reference: " + array.IsSameByReference(array2));

            Console.WriteLine("Are same by value: " + array.IsSameByValue(array3, (a,b) => a == b));
            Console.WriteLine("Are same by value: " + array.IsSameByValue(array2, (a, b) => a == b));


            "mamaliga".PipeResultTo((x) => Write(x, "bla", "bla"));

            ReturnTrue().IfTrue(() => Console.WriteLine("True")).Else(() => Console.WriteLine("False"));

            Console.ReadKey();
        }

        static string Write(string val, string val2, string val3)
        {
            Console.WriteLine(val);

            return null;
        }

        static string WriteR(string val)
        {
            Console.WriteLine(val);

            return val;
        }

        static int Return33() => 33;

        static bool ReturnTrue() => true;
    }
}
