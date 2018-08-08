namespace Pipe4Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Option<T> : IEnumerable<T>
    {
        public static Option<T> From<T>(T value) => new Option<T>(value);

        public static Option<T> None<T>() => new Option<T>(default(T));

        private Option() { }

        private Option(T value)
        {
            HasValue = value != null;
            _value = value;
        }

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

        public static implicit operator Option<T>(T value) => new Option<T>(value);

        public IEnumerator<T> GetEnumerator()
        {
            if (HasValue) yield return _value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class PipeIt
    {
        /// <summary>
        /// Pipes the object to anoter action without expecting a result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        public static void PipeWith<T>(this T obj, Action<T> action) => action(obj);

        /// <summary>
        /// Pipes the object to anoter function, function that returns a value of the same tipe as T
        /// Ex: calls a function that has a parameter of type string and returns a string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T Pipe<T>(this T obj, Func<T, T> func) => func(obj);

        /// <summary>
        /// Pipes the object to another function, function that returns an Options<T> monad
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Option<TR> Pipe<T,TR>(this T obj, Func<T, Option<TR>> func) => func(obj);
    }

    public static class IEnumerableExtensions
    {

        /// <summary>
        /// Just like .ForEach on a list this executes an action to each element of an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            foreach (var v in array) action(v);
        }

        /// <summary>
        /// This will deep copy an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] DeepCopy<T>(this T[] array)
        {
            var length = array.Count();

            var clone = new T[length];

            for (var i = 0; i < length; i++)
                clone[i] = array[i];

            return clone;
        }

        /// <summary>
        /// This wil copy the reference of an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static IEnumerable<T> ShallowCopy<T>(this IEnumerable<T> array) => array;

        /// <summary>
        /// Will compare two arrays by the values they contain, but not by the index of that values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="arrayToCompare"></param>
        /// <param name="compareFunction"></param>
        /// <returns></returns>
        public static bool IsSameByValue<T>(this IEnumerable<T> array, IEnumerable<T> arrayToCompare, Func<T, T, bool> compareFunction)
        {
            if (array.Count() != arrayToCompare.Count()) return false;

            foreach (var v in array)
            {
                if (arrayToCompare.Contains(v)) continue;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Will compare two arrays by values and index of that values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="arrayToCompare"></param>
        /// <param name="compareFunction"></param>
        /// <returns></returns>
        public static bool IsSameByValueAndIndex<T>(this IEnumerable<T> array, IEnumerable<T> arrayToCompare, Func<T, T, bool> compareFunction)
        {
            if (array.Count() != arrayToCompare.Count()) return false;

            for (var i = 0; i < array.Count(); i++)
            {
                if(compareFunction(array.ElementAt(i), arrayToCompare.ElementAt(i)))
                    continue;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Will check if two arrays are pointing to the same location in memory 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="arrayToCompare"></param>
        /// <returns></returns>
        public static bool IsSameByReference<T>(this IEnumerable<T> array, IEnumerable<T> arrayToCompare) => ReferenceEquals(array, arrayToCompare);
    }
}