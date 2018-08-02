using System;
using System.Linq;
using Pipe4Net;
using Xunit;

namespace PipeItTests
{
    public class Tests
    {
        [Fact]
        public void Should_Pipe_Value_With_No_Errors()
        {
            var testObj = "mamaliga";

            testObj.PipeWith(Console.WriteLine);
        }

        [Fact]
        public void Should_Pipe_Value_And_Return_The_Same_Value()
        {
            var testObj = "mamaliga";

            var testObjReturned = testObj.Pipe(x => x);

            Assert.Equal(testObjReturned, testObj);
        }

        [Fact]
        public void Should_Pipe_Value_And_Return_Value_Modified()
        {
            var testObj = "mamaliga";
            var ending = "mood";

            var testObjReturned = testObj.Pipe(x => x + ending);

            Assert.Equal(testObjReturned, testObj + ending);
        }

        [Fact]
        public void Should_Pipe_Value_And_Return_Option()
        {
            var testObj = "mamaliga";

            var objReturned = testObj.Pipe(x => Option<string>.From(x));

            Assert.Equal(testObj, objReturned.Value);
        }

        [Fact]
        public void Should_Pipe_Default_Value_Of_Int()
        {
            var defaultOfInt = default(int);

            var objReturned = defaultOfInt.Pipe(x => Option<int>.None<int>());

            Assert.Equal(defaultOfInt, objReturned);
        }

        [Fact]
        public void Should_Have_Same_Result_When_Piped()
        {
            var result = IncrementA(IncrementB(IncrementC(1)));

            var pipedResult = 1.Pipe(IncrementC).Pipe(IncrementB).Pipe(IncrementC);

            Assert.Equal(result,pipedResult);

            var pipedResult2 = IncrementC(1).Pipe(IncrementB).Pipe(IncrementA);

            Assert.Equal(result, pipedResult2);
        }

        private int IncrementA(int val) => val++;

        private int IncrementB(int val) => val++;

        private int IncrementC(int val) => val++;

        [Fact]
        public void Should_Deep_Copy_Array()
        {
            var testArray = Enumerable.Range(1, 10).ToArray();

            var clonedArray = testArray.DeepCopy();

            Assert.False(ReferenceEquals(testArray, clonedArray));

            for (var i = 0; i < testArray.Length; i++) Assert.True(testArray[i] == clonedArray[i]);
        }

        [Fact]
        public void Should_Shallow_Copy_Array()
        {
            var testArray = Enumerable.Range(1, 10).ToArray();

            var clonedArray = testArray.ShallowCopy();
            var deepClonedArray = testArray.DeepCopy();

            Assert.True(ReferenceEquals(testArray, clonedArray));
            Assert.False(ReferenceEquals(testArray, deepClonedArray));
        }

        [Fact]
        public void Should_Compare_Arrays_By_Values()
        {
            var testArray = Enumerable.Range(1, 10).ToArray();

            var clonedArray = testArray.DeepCopy().Reverse();

            Assert.True(testArray.IsSameByValue(clonedArray, (i, i1) => i == i1));
            Assert.False(testArray.IsSameByReference(clonedArray));

            testArray = testArray.Concat(new [] { 11 }).ToArray();

            Assert.False(testArray.IsSameByValue(clonedArray, (i, i1) => i == i1));
        }

        [Fact]
        public void Should_Compare_Arrays_By_Reference()
        {
            var testArray = Enumerable.Range(1, 10).ToArray();

            var clonedArray = testArray.ShallowCopy();
            var deepClonedArray = testArray.DeepCopy();

            Assert.True(testArray.IsSameByReference(clonedArray));
            Assert.False(testArray.IsSameByReference(deepClonedArray));
        }

        [Fact]
        public void Should_Compare_Arrays_By_Value_And_Index()
        {
            var testArray = Enumerable.Range(1, 10).ToArray();

            var clonedArray = testArray.DeepCopy();

            Assert.True(testArray.IsSameByValueAndIndex(clonedArray, (i, i1) => i1 == i));
            Assert.False(testArray.IsSameByReference(clonedArray));

            testArray = testArray.Concat(new [] {11}).ToArray();

            Assert.False(testArray.IsSameByValueAndIndex(clonedArray, (i, i1) => i1 == i));
        }

    }
}
