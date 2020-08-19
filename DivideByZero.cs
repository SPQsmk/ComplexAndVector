using System;

namespace Lab_11
{
    class DivideByZero : EventArgs
    {
        public Complex Dividend { get; set; }
        public Complex Divider { get; set; }
    }
}
