using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Pipe4Net
{
    public class Option<T> : IEnumerable<T>
    {
        public static Option<T> From<T>(T value) => new Option<T>(value);
        public static Option<T> None<T>() => default(T);

        private Option() { }

        public bool HasValue { get; }

        readonly T _value;

        public T Value
        {
            get
            {
                if (!HasValue) throw new InvalidOperationException();
                
                return _value;
            }
        }

        internal Option(T value)
        {
            HasValue = value != null;
            _value = value;
        }

        public static implicit operator Option<T>(T value) => new Option<T>(value);

        public IEnumerator<T> GetEnumerator()
        {
            if (HasValue) yield return _value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class PipeIt
    {
        public static void PipeWith<T>(this T obj, Action<T> action) => action(obj);
      
        public static T Pipe<T>(this T obj, Func<T, T> func) => func(obj);

        public static Option<TR> Pipe<T,TR>(this T obj, Func<T, Option<TR>> func) => func(obj);
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
