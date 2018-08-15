namespace Pipe4Net
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Use Option.From<T>(T value)
    /// or
    /// Option.None<T>() Will have property hasValue = false, and will return the defaut value of T on .Value
    /// or
    /// Option.None<T>(T defaultValue) Will have property hasValue = false, and will return the value passed as defaultValue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Option<T> : IEnumerable<T>
    {
        public static Option<T> From(T value) => new Option<T>(value);

        public static Option<T> None() => new Option<T>(default, false);

        public static Option<T> None(T defaultValue) => new Option<T>(defaultValue, false);

        private Option() { }

        private Option(T value)
        {
            HasValue = value != null;
            _value = value;
        }

        private Option(T value, bool hasValue)
        {
            HasValue = hasValue;
            _value = value;
        }

        public bool HasValue { get; }

        private readonly T _value;

        public T Value => _value;

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
        public static Option<TR> Pipe<T, TR>(this T obj, Func<T, Option<TR>> func) => func(obj);

        /// <summary>
        /// Extension method that evaluates a bool if is true executes action , can be continues with .Else
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ElseBranch IfTrue(this bool condition, Action action)
        {
            if (true == condition) action();

            return new ElseBranch(condition);
        }
    }

    public sealed class ElseBranch
    {
        private readonly bool value;

        public ElseBranch(bool value)
        {
            this.value = value;
        }

        /// <summary>
        /// Chained with .IfTrue extension method on bool, this will run instead of IfTrue
        /// </summary>
        /// <param name="action"></param>
        public void Else(Action action)
        {
            if (false == value) action();
        }
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
                if (compareFunction(array.ElementAt(i), arrayToCompare.ElementAt(i)))
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

        /// <summary>
        /// Remove all null elements in an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static IEnumerable<T> RemoveNulls<T>(this IEnumerable<T> array)
        {
            IEnumerable<T> arrayWithoutNulls = new T[0];

            array.ForEach(x =>
            {
                if (x != null) arrayWithoutNulls = arrayWithoutNulls.AddElement(x);
            });

            return arrayWithoutNulls;
        }

        /// <summary>
        /// Returns a copy of the array containing the element obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> AddElement<T>(this IEnumerable<T> array, T obj)
        {
            array = array.Concat(new[] { obj });

            return array;
        }

        /// <summary>
        /// Returns a copy of the array containing the elements obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static IEnumerable<T> AddElements<T>(this IEnumerable<T> array, T[] objs)
        {
            array = array.Concat(objs);

            return array;
        }

        /// <summary>
        /// Returns a copy of the array without the element obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> RemoveElement<T>(this IEnumerable<T> array, T obj)
        {
            IEnumerable<T> temp = new T[0];

            array.ForEach(v =>
            {
                if (false == v.Equals(obj)) temp = temp.AddElement(v);
            });

            return temp;
        }

        /// <summary>
        /// Returns a copy of the array without the elements objs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static IEnumerable<T> RemoveElements<T>(this IEnumerable<T> array, T[] objs)
        {
            IEnumerable<T> temp = new T[0];

            array.ForEach(x =>
            {
                if (false == objs.Contains(x)) temp = temp.AddElement(x);
            });

            return temp;
        }

        /// <summary>
        /// Just like the normal .ForEach this injects the index also, so you don't have to use a normal for loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void ForEachWithIndex<T>(this IEnumerable<T> array, Action<T, int> action)
        {
            var arrLength = array.Count();
            for (var i = 0; i < arrLength; i++)
            {
                action(array.ElementAt(i), i);
            }
        }

        /// <summary>
        /// Will split an simple one direction array in a jagged array with length = splitByParameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="splitBy">The length of each subArray</param>
        /// <returns>A jagger array T[splitBy][splitBy]</returns>
        public static T[][] Split<T>(this IEnumerable<T> array, int splitBy)
        {
            T[][] _temp = new T[splitBy][];
            for (var x = 0; x < splitBy; x++)
            {
                _temp[x] = new T[splitBy];
            }

            var length = array.Count() - 1;
            var enumerator = array.GetEnumerator();
            var mod = (length / splitBy);
            var i = 0;
            var j = 0;
            while (enumerator.MoveNext())
            {
                _temp[i][j] = enumerator.Current;
                if (j == mod)
                {
                    i++;
                    j = 0;
                }
                else
                    j++;
            }

            return _temp;
        }

        /// <summary>
        /// Will execute an action for n number of times where n = int on which this extension method is invoked on
        /// </summary>
        /// <param name="count"></param>
        /// <param name="action"></param>
        public static void GenerateForLoop(this int count, Action action)
        {
            for (var i = 0; i < count; i++) action();
        }
    }
}