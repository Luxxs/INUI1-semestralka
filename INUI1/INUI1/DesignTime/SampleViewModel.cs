using INUI1.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.DesignTime
{
    class SampleViewModel
    {
        public static ObservableCollection<Cell> State
        {
            get
            {
                return new ObservableCollection<Cell>() { new Cell(0, false), new Cell(2, true) , new Cell(0, false), new Cell(0, false), new Cell(0, true), new Cell(2, true) };
            }
        }
    }
}
