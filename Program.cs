using System;
using System.Collections.Generic;

namespace Lab_11
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Complex(2, 3);
            var b = new Complex(1, 4);
            var c = new Complex(5, 3);
            var d = new Complex(3, 7);
            var e = new Complex(0, 0);

            var arr1 = new Complex[] {a, b};
            var arr2 = new Complex[] {c, d};
            var vec1 = new Vector<Complex>(arr1);
            var vec2 = new Vector<Complex>(arr2);

            a.DivideByZeroEvent += DbZEvent;

            foreach (var x in Vector<Complex>.Orthogonal(new List<Vector<Complex>>{vec1, vec2}))
            {
                Console.WriteLine(x);
            }
        }

        public static void DbZEvent(object obj, DivideByZero args)
        {
            throw new DivideByZeroException("Divide by zero");
        } 
    }
}
