using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_11
{
    class Vector<T> where T : IComparable, new()
    {
        private readonly List<T> _parameters;

        public Vector()
        {
            _parameters = new List<T>();
        }

        public Vector(IEnumerable<T> parameters) : this()
        {
            _parameters.AddRange(parameters);
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            foreach (var x in _parameters)
            {
                str.Append(x).Append(", ");
            }

            str.Remove(str.Length - 2, 2);

            return str.ToString();
        }

        public T this[int i] => _parameters[i];

        public int Size()
        {
            return _parameters.Count;
        }

        public T[] ToArray()
        {
            return _parameters.ToArray();
        }

        public static Vector<T> operator +(Vector<T> a, Vector<T> b)
        {
            if (a.Size() != b.Size())
            {
                throw new ArgumentException("The number of elements in the vectors doesn't match!");
            }

            var c = new Vector<T>();

            for (var i = 0; i < a.Size(); i++)
            {
                var x = (dynamic) a[i];
                var y = (dynamic) b[i];

                c._parameters.Add(x + y);
            }

            return c;
        }

        public static Vector<T> operator -(Vector<T> a, Vector<T> b)
        {
            return a + (-1) * b;
        }

        public static Vector<T> operator *(double a, Vector<T> b)
        {
            var c = new Vector<T>();

            for (var i = 0; i < b.Size(); i++)
            {
                var y = (dynamic)b[i];

                c._parameters.Add(a * y);
            }

            return c;
        }

        public static Vector<T> operator *(Vector<T> a, T b)
        {
            return b * a;
        }

        public static Vector<T> operator *(T a, Vector<T> b)
        {
            var c = new Vector<T>();

            for (var i = 0; i < b.Size(); i++)
            {
                var y = (dynamic)b[i];

                c._parameters.Add(a * y);
            }

            return c;
        }

        public static Vector<T> operator *(Vector<T> a, double b)
        {
            return b * a;
        }

        public static T operator *(Vector<T> a, Vector<T> b)
        {
            if (a.Size() != b.Size())
            {
                throw new ArgumentException("The number of elements in the vectors doesn't match!");
            }

            var res = new T();

            for (var i = 0; i < a.Size(); i++)
            {
                var x = (dynamic)a[i];
                var y = (dynamic)b[i];

                if (res is Complex)
                {
                    y = y.Conjugate();
                }

                res += x * y;
            }

            return res;
        }

        public int CompareTo(object obj)
        {
            if (obj is null)
                throw new ArgumentNullException();

            if (!(obj is Vector<T> vec))
                throw new ArgumentException("Argument is not Vector");

            if (Modulus() > vec.Modulus())
                return 1;
            if (Modulus() < vec.Modulus())
                return -1;

            return 0;
        }

        public static bool operator >(Vector<T> a, Vector<T> b)
        {
            return a.CompareTo(b) == 1;
        }

        public static bool operator <(Vector<T> a, Vector<T> b)
        {
            return a.CompareTo(b) == -1;
        }

        public static bool operator >=(Vector<T> a, Vector<T> b)
        {
            return a.CompareTo(b) != -1;
        }

        public static bool operator <=(Vector<T> a, Vector<T> b)
        {
            return a.CompareTo(b) != 1;
        }

        public static bool operator ==(Vector<T> a, Vector<T> b)
        {
            return a?.CompareTo(b) == 0;
        }

        public static bool operator !=(Vector<T> a, Vector<T> b)
        {
            return a?.CompareTo(b) != 0;
        }

        public double Modulus()
        {
            var res = 0.0;
            var obj = new T();
            var isComplex = obj is Complex;

            foreach (var y in _parameters.Cast<dynamic>())
            {
                if (isComplex)
                {
                    res += y.Modulus() * y.Modulus();
                }
                else
                {
                    res += y * y;
                }
            }

            return Math.Sqrt(res);
        }

        public static List<Vector<T>> Orthogonal(List<Vector<T>> a)
        {
            if (a.Count == 0)
                return new List<Vector<T>>();
            if (a[0].Size() == 0)
                throw new ArgumentException("Empty vector!");

            for (var i = 1; i < a.Count; i++)
                if (a[i].Size() != a[i-1].Size())
                    throw new ArgumentException("The number of elements in the vectors doesn't match!");

            var res = new List<Vector<T>> {a[0]};

            for (var i = 1; i < a.Count; i++)
            {
                var current = a[i];

                current = res.Aggregate(current, (current1, x) => current1 - Proj(a[i], x));

                res.Add(current);
            }

            return res;
        }

        public static Vector<T> Proj(Vector<T> a, Vector<T> b)
        {
            var x = (dynamic) a * b;
            var y = (dynamic) b * b;
            var coefficient = x / y;

            return coefficient * b;
        }
    }

}
