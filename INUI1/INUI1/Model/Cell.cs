using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.Model
{
    public class Cell : INotifyPropertyChanged
    {
        private int _value;
        public int Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
        private bool _inPath;
        public bool InPath
        {
            get { return _inPath; }
            set { SetProperty(ref _inPath, value); }
        }

        public Cell(int value, bool inPath)
        {
            Value = value;
            InPath = inPath;
        }
        public override string ToString()
        {
            return (InPath ? "X" : "_") + ((Value == 0) ? "" : Value.ToString());
        }

        #region INotifyPropertyChanged members

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
