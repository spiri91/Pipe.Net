# Pipe.Net

## Inspired by PowerShell, 3 extension methods that will simplify your code Pipe<T> and PipeWith<T>
  
### var result = IncrementA(IncrementB(IncrementC(1)))
###      becomes 
### var pipedResult2 = IncrementC(1).Pipe(IncrementB).Pipe(IncrementA); 

### You will find another goodie in there , an Option<T> monad in case you need it. 
The code is pretty simple, i encourage you to play and extend it by your own needs
  
    public static class PipeIt
    {
        public static void PipeWith<T>(this T obj, Action<T> action) => action(obj);
      
        public static T Pipe<T>(this T obj, Func<T, T> func) => func(obj);

        public static Option<TR> Pipe<T,TR>(this T obj, Func<T, Option<TR>> func) => func(obj);
    }
    
 This is the main reason why this package exists. Look at the other tests also they are pretty intuitive
  
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
        
The test above perfectly represents what i want to achieve with this nuget package, i have reading code from right to left, or naming temporary variables, it just one of those things that bothers me in programming, and i think this way or writing code is more elegant: 

var result = IncrementA(IncrementB(IncrementC(1))) becomes var pipedResult2 = IncrementC(1).Pipe(IncrementB).Pipe(IncrementA); 

Awesome
  
## Some extensions methods on arrays that will make your code easier to read: 
 - .ForEach implementation for arrays
 - .DeepCopy
 - .ShallowCopy
 - .AreSameByValue
 - .AreSameByReference
 - .AreSameByValueAndIndex
