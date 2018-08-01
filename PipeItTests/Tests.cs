using System;
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

            testObj.Pipe(Console.WriteLine);
        }

        [Fact]
        public void Should_Pipe_Value_And_Return_The_Same_Value()
        {
            var testObj = "mamaliga";

            var testObjReturned = testObj.PipeReturn(x => x);

            Assert.Equal(testObjReturned, testObj);
        }

        [Fact]
        public void Should_Pipe_Value_And_Return_Value_Modified()
        {
            var testObj = "mamaliga";
            var ending = "mood";

            var testObjReturned = testObj.PipeReturn(x => x + ending);

            Assert.Equal(testObjReturned, testObj + ending);
        }
    }
}
