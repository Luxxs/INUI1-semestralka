using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.Model
{
    public class Cell
    {
        public int Value { get; set; }
        public bool InPath { get; set; }

        public Cell(int value, bool inPath)
        {
            Value = value;
            InPath = inPath;
        }
        public override string ToString()
        {
            if (Value == 0)
                return "";
            return Value.ToString();
        }
    }
}
