using INUI1.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Cell[,] _matrix = new Cell[,] { { new Cell(0, false), new Cell(2, true), new Cell(0, false) }, { new Cell(0, false), new Cell(0, true), new Cell(2, true) } };
        public Cell[,] Matrix
        {
            get { return _matrix; }
            set { SetProperty(ref _matrix, value); }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
