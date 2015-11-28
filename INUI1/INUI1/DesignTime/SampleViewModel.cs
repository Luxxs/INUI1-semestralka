using INUI1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.DesignTime
{
    class SampleViewModel
    {
        public static Cell[,] Matrix
        {
            get
            {
                return new Cell[,] { { new Cell(0, false), new Cell(2, true), new Cell(0, false) }, { new Cell(0, false), new Cell(0, true), new Cell(2, true) } };
            }
        }
    }
}
