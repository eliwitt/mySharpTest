using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace mySharpTest
{
    class Foo {
        public int x = 0;
        public int y = 0;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Foo a, b;   // ref types
            int c, d;   // value types

            // value copying
            c = 4;
            d = c;
            c = 5;      // change c after assignment

            // ref copying
            a = new Foo();
            b = a;
            a.x = 1;    // change after assignment
            a.y = 2;

            // print out contents of b and d
            Console.WriteLine("b.x={0}, d={1}", b.x, d);
            Console.WriteLine("{0}", "+".PadRight(19, '+'));
            using (DisposableFoo myFoo = new DisposableFoo())
            {
                Console.WriteLine("In the using block");
            }
            Console.WriteLine("{0}", "+".PadRight(19, '+'));
            DisposableFoo myTestFoo = new DisposableFoo();
            myTestFoo.DisInt = 5;
            myTestFoo.theMethod("Called by main");
            Console.WriteLine("{0}", "+".PadRight(19, '+'));
            DisposableFoo myCopyFoo;
            myCopyFoo = myTestFoo.DeepClone();
            Console.WriteLine("Copied {0}", myCopyFoo.DisInt);
            myCopyFoo.DisInt = 10;
            Console.WriteLine("Org = {0}  Copy = {1}", myTestFoo.DisInt, myCopyFoo.DisInt);
            Console.WriteLine("{0}", "+".PadRight(19, '+'));
        }
    }

    [Serializable]
    public class DisposableFoo : IDisposable
    {
        private int myInt;
        public int DisInt 
        {
            get { return myInt; }

            set { myInt = value; }
        }

        public DisposableFoo DeepClone() 
        { 
            Console.WriteLine("In the copy method");

            using (var ms = new MemoryStream()) 
            { 
                var formatter = new BinaryFormatter(); 
                formatter.Serialize(ms, this); 
                ms.Position = 0; 
                return formatter.Deserialize(ms) as DisposableFoo; 
            } 
        }
 
        public DisposableFoo()
        {
            Console.WriteLine("In the constructor");
            myInt = 0;
        }

        public void theMethod(string theStg)
        {
            Console.WriteLine("In theMethod " + theStg);
        }

        private void CleanUp()
        {
            Console.WriteLine("In CleanUp");
        }

        ~DisposableFoo()
        {
            CleanUp();
            Console.WriteLine("Client did not call Dispose().");
        }

        #region IDisposable Members

        public void Dispose()
        {
            Console.WriteLine("In Dispose");
            CleanUp();
            GC.SuppressFinalize(this);
            Console.WriteLine("Finished with object");
        }

        #endregion
    }
}
