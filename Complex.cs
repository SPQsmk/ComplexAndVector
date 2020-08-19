using System;

namespace Lab_11
{
    class Complex : IComparable
    {
        private double _re;
        private double _im;
        private const double Eps = 1e-10;
        
        public Complex(double re, double im)
        {
            _re = re;
            _im = im;
        }

        public Complex() : this(0, 0)
        {
        }

        public Complex(Complex z) : this(z._re, z._im)
        {
        }

        public override string ToString()
        {
            var reZ = Math.Round(_re, 2);
            var imZ = Math.Round(_im, 2);

            if (Math.Abs(_re) < Eps && Math.Abs(_im) < Eps)
                return "0";
            if (Math.Abs(_re) < Eps)
                return $"{imZ}i";
            if (Math.Abs(_im) < Eps)
                return $"{reZ}";

            var sign = _im > 0 ? "+" : "";

            if (Math.Abs(_im - 1) < Eps)
                return $"{reZ}+i";
            if (Math.Abs(_im + 1) < Eps)
                return $"{reZ}-i";

            return $"{reZ}{sign}{imZ}i";
        }

        public int CompareTo(object obj)
        {
            if (obj is null)
                throw new ArgumentNullException();

            if (!(obj is Complex complex))
                throw new ArgumentException("Argument is not Vector");

            if (Modulus() > complex.Modulus())
                return 1;
            if (Modulus() < complex.Modulus())
                return -1;

            return 0;
        }

        public static Complex operator +(Complex a, Complex b)
        {
            var reZ = a._re + b._re;
            var imZ = a._im + b._im; 

            return new Complex(reZ, imZ);
        }

        public static Complex operator +(double a, Complex b)
        {
            return new Complex(b._re + a, b._im);
        }

        public static Complex operator +(Complex a, double b)
        {
            return new Complex(a._re + b, a._im);
        }

        public static Complex operator -(Complex a, Complex b)
        {
            return a + (-1) * b;
        }

        public static Complex operator -(double a, Complex b)
        {
            return a + (-1) * b;
        }

        public static Complex operator -(Complex a, double b)
        {
            return a + (-1) * b;
        }

        public static Complex operator *(Complex a, Complex b)
        {
            var reZ = a._re * b._re - a._im * b._im;
            var imZ = a._re * b._im + a._im * b._re;

            return new Complex(reZ, imZ);
        }

        public static Complex operator *(double a, Complex b)
        {
            var reZ = a * b._re;
            var imZ = a * b._im;

            return new Complex(reZ, imZ);
        }

        public static Complex operator *(Complex a, double b)
        {
            return b * a;
        }

        public static Complex operator /(Complex a, Complex b)
        {
            CheckForZero(a, b);

            var z = a * b.Conjugate();

            return Division(z, b);
        }

        public static Complex operator /(double a, Complex b)
        {
            CheckForZero(new Complex(a, 0), b);

            var z = a * b.Conjugate();

            return Division(z, b);
        }

        public static Complex operator /(Complex a, double b)
        {
            CheckForZero(a, new Complex(b, 0));

            var reZ = a._re / b;
            var imZ = a._im / b;

            return new Complex(reZ, imZ);
        }

        public static Complex Division(Complex a, Complex b)
        {
            var divider = b._re * b._re + b._im * b._im;

            a._re /= divider;
            a._im /= divider;

            return a;
        }

        private static void CheckForZero(Complex a, Complex b)
        {
            if (!(b.Modulus() < Eps)) 
                return;

            var args = new DivideByZero { Dividend = a, Divider = b };

            a.DivideByZeroEvent?.Invoke(a, args);
        }

        public event EventHandler<DivideByZero> DivideByZeroEvent;

        public Complex Conjugate()
        {
            return new Complex(_re, -_im);
        }

        public double Modulus()
        {
            return Math.Sqrt(_re * _re + _im * _im);
        }

        public double Argument()
        {
            if (_re > 0 && _im < Eps)
                return 0;
            if (_re < 0 && _im < Eps)
                return Math.PI;
            if (_re < Eps && _im > 0)
                return Math.PI * 0.5;
            if (_re < Eps && _im < 0)
                return -Math.PI * 0.5;
            if (_re < 0 && _im > 0)
                return Math.PI - Math.Atan(Math.Abs(_im / _re));
            if (_re < 0 && _im < 0)
                return -Math.PI + Math.Atan(Math.Abs(_im / _re));

            return Math.Atan(_im / _re);
        }

        public Complex Pow(int a)
        {
            if (a <= 0)
                throw new ArgumentException("The degree of the Complex number must be a natural number!");

            var powMod = Math.Pow(Modulus(), a);
            var arg = Argument();
            var reZ = powMod * Math.Cos(arg * a);
            var imZ = powMod * Math.Sin(arg * a);

            return new Complex(reZ, imZ);
        }

        public Complex[] Root(int a)
        {
            if (a <= 1)
                throw new ArgumentException("The root must be more than one!");

            var sqrtMod = Math.Pow(Modulus(), (double) 1 / a);
            var arg = Argument();
            var numbers = new Complex[a];

            for (var i = 0; i < a; i++)
            {
                var reZ = sqrtMod * Math.Cos((arg + 2 * Math.PI * i) / a);
                var imZ = sqrtMod * Math.Sin((arg + 2 * Math.PI * i) / a);
                numbers[i] = new Complex(reZ, imZ);
            }

            return numbers;
        }
    }
}
