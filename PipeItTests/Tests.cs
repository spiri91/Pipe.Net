using System;
using System.Collections.Generic;
using System.Linq;
using Pipe4Net;
using Xunit;

public class Tests
{
    [Fact]
    public void Should_Pipe_Value_With_No_Errors()
    {
        var testObj = "mamaliga";

        testObj.EndWith(Console.WriteLine);
    }

    [Fact]
    public void Should_Pipe_Value_And_Return_The_Same_Value()
    {
        var testObj = "mamaliga";

        var testObjReturned = testObj.PipeResultTo(x => x);

        Assert.Equal(testObjReturned, testObj);
    }

    [Fact]
    public void Should_Pipe_Value_And_Return_Value_Modified()
    {
        var testObj = "mamaliga";
        var ending = "mood";

        var testObjReturned = testObj.PipeResultTo(x => x + ending);

        Assert.Equal(testObjReturned, testObj + ending);
    }

    [Fact]
    public void Should_Pipe_Value_And_Return_Option()
    {
        var testObj = "mamaliga";

        var objReturned = testObj.PipeResultTo(x => Option<string>.From(x));

        Assert.Equal(testObj, objReturned.Value);
    }

    [Fact]
    public void Should_Pipe_Default_Value_Of_Int()
    {
        var defaultOfInt = default(int);

        var objReturned = defaultOfInt.PipeResultTo(x => Option<int>.None(default(int)));

        Assert.Equal(defaultOfInt, objReturned.Value);
    }

    [Fact]
    public void Should_Have_Same_Result_When_Piped()
    {
        var result = IncrementA(IncrementB(IncrementC(1)));

        var pipedResult = 1.PipeResultTo(IncrementC).PipeResultTo(IncrementB).PipeResultTo(IncrementC);

        Assert.Equal(result, pipedResult);

        var pipedResult2 = IncrementC(1).PipeResultTo(IncrementB).PipeResultTo(IncrementA);

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

        testArray = testArray.Concat(new[] {11}).ToArray();

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

        testArray = testArray.Concat(new[] {11}).ToArray();

        Assert.False(testArray.IsSameByValueAndIndex(clonedArray, (i, i1) => i1 == i));
    }

    [Fact]
    public void Should_Return_Null_From_Option_Of_Null()
    {
        var result = Option<Object>.None();

        Assert.True(null == result.Value);
    }

    [Fact]
    public void Should_Remove_Nulls_From_Array()
    {
        var objArray = new object[] {null, null, new object(), null, new object(), null};

        var withoutNull = objArray.RemoveNulls();

        foreach (var o in withoutNull)
        {
            Assert.True(o != null);
        }
    }

    [Fact]
    public void Should_Add_Element_To_Array()
    {
        var arrayOfInts = new int[] {1, 2, 3, 4, 5};
        var arrayOfIntsWithAdd = arrayOfInts.AddElement(6);

        Assert.Contains(6, arrayOfIntsWithAdd);
    }

    [Fact]
    public void Should_Add_Elements_To_Array()
    {
        var arrayOfInts = new int[] {1, 2, 3, 4, 5};
        var arrayToBeAdded = new int[] {6, 7, 8};

        var arrayOfIntsWithAdd = arrayOfInts.AddElements(arrayToBeAdded);
        foreach (var i in arrayOfIntsWithAdd)
        {
            Assert.Contains(i, arrayOfIntsWithAdd);
        }
    }

    [Fact]
    public void Should_Remove_Element_From_Array()
    {
        IEnumerable<int> arrayOfInts = new int[] {1, 2, 3, 4, 5, 6};
        var elementToBeRemoved = 3;

        arrayOfInts = arrayOfInts.RemoveElement(elementToBeRemoved);

        Assert.DoesNotContain(elementToBeRemoved, arrayOfInts);
    }

    [Fact]
    public void Should_Remove_Elements_From_Array()
    {
        IEnumerable<int> arrayOfInts = new int[] {1, 2, 3, 4, 5, 6};
        var intsToBeRemoved = new int[] {4, 5, 6, 7};

        arrayOfInts = arrayOfInts.RemoveElements(intsToBeRemoved);

        foreach (var i in intsToBeRemoved)
        {
            Assert.DoesNotContain(i, arrayOfInts);
        }
    }

    [Fact]
    public void Should_Run_IfTrue_Branch()
    {
        var i = 0;
        Func<bool> ac = () => true;

        ac().IfTrue(() => i++).Else(() => i--);

        Assert.True(i == 1);
        Assert.False(i == 0);
        Assert.False(i == -1);
    }

    [Fact]
    public void Should_Run_ElseBranch()
    {
        var i = 0;
        Func<bool> ac = () => false;

        ac().IfTrue(() => i++).Else(() => i--);

        Assert.True(i == -1);
        Assert.False(i == 0);
        Assert.False(i == 1);
    }

    [Fact]
    public void Should_For_Each_With_Index()
    {
        IEnumerable<int> arrayOfInts = new int[] {1, 2, 3, 4, 5, 6};
        var index = 0;
        var action = new Action<int, int>((element, i) =>
        {
            Assert.True(element == arrayOfInts.ElementAt(index));
            Assert.True(index == i);

            index++;
        });

        arrayOfInts.ForEachWithIndex(action);
    }

    [Fact]
    public void Should_Split_By()
    {
        IEnumerable<int> arrayOfInts = new int[] {1, 2, 3, 4, 5, 6, 7};
        int[][] result = new int[][]
        {
            new int[] {1, 2, 3},
            new int[] {4, 5, 6},
            new int[] {7, 0, 0}
        };

        var possibleResult = arrayOfInts.Split(3);

        for (var i = 0; i < result.Count(); i++)
        {
            var bla = result.ElementAt(i);

            for (var j = 0; j < bla.Count(); j++)
            {
                Assert.True(result[i][j] == possibleResult[i][j]);
            }
        }
    }

    [Fact]
    public void Should_Create_For_Loop_From_Int()
    {
        var bla = 6;
        var index = 0;
        var counter = new[] {0, 1, 2, 3, 4, 5};
        var action = new Action(() =>
        {
            Assert.True(index == counter[index]);
            index++;
        });

        bla.GenerateForLoop(action);
    }

    [Fact]
    public void Should_Create_For_Loop_From_Int_With_Index()
    {
        var foo = 10;
        var eta = 0;
        foo.GenerateForLoopWithIndex((i) =>
        {
            Assert.True(i == eta);
            eta++;
        });
    }

    [Fact]
    public void Should_Pipe_Int_To_Function_That_Accepts_A_String()
    {
        var desiredValud = "33Test";

        var result = ReturnInt(33).Pipe(x => x.ToString(), AppendTestToString);

        Assert.True(desiredValud == result);
    }

    [Fact]
    public void Should_Pipe_Int_To_Function_That_Accepts_A_String_Return_Option()
    {
        var desiredValud = "33Test";

        var result = ReturnInt(33).Pipe(x => x.ToString(), AppendTestToStringReturnMonad);

        Assert.True(result.Value == desiredValud);
    }

    [Fact]
    public void Should_Pipe_Int_To_Function_That_Accepts_A_String_Returns_Void()
    {
        ReturnInt(33).Pipe(x => x.ToString(), ReceiveString);
    }

    private int ReturnInt(int a) => a;

    private string AppendTestToString(string val) => val + "Test";

    private Option<string> AppendTestToStringReturnMonad(string val) => Option<string>.From(val + "Test");

    private void ReceiveString(string val)
    {
    }
}