using System;
using System.Collections.Generic;
using System.Linq;

namespace Pipe4Net
{
    public static class PipeIt
    {
        public static void Pipe<T>(this T obj, Action<T> action) => action(obj);
      
        public static T PipeReturn<T>(this T obj, Func<T, T> func) => func(obj);
    }

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            foreach (var v in array) action(v);
        }

        public static T[] DeepCopy<T>(this T[] array)
        {
            var length = array.Count();

            var clone = new T[length];

            for (var i = 0; i < length; i++)
                clone[i] = array[i];

            return clone;
        }

        public static IEnumerable<T> ShallowCopy<T>(this IEnumerable<T> array) => array;

        public static bool AreSameByValue<T>(this IEnumerable<T> array, IEnumerable<T> arrayToCompare, Func<T, T, bool> compareFunction)
        {
            if (array.Count() != arrayToCompare.Count()) return false;

            foreach (var v in array)
            {
                if (arrayToCompare.Contains(v)) continue;

                return false;
            }

            return true;
        }

        public static bool AreSameByValueAndIndex<T>(this IEnumerable<T> array, IEnumerable<T> arrayToCompare, Func<T, T, bool> compareFunction)
        {
            if (array.Count() != arrayToCompare.Count()) return false;

            for (var i = 0; i < array.Count(); i++)
            {
                if(compareFunction(array.ElementAt(i), arrayToCompare.ElementAt(i)))
                    break;

                return false;
            }

            return true;
        }

        public static bool AreSameByReference<T>(this IEnumerable<T> array, IEnumerable<T> arrayToCompare) => ReferenceEquals(array, arrayToCompare);
    }
}
