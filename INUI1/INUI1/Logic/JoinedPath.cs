using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.Logic
{
    public class JoinedPath : AbstractPath
    {
        public LinkedList<Tuple<int, int>> Path { get; set; }

        private string _toString;
        
        public override string ToString()
        {
            if (_toString != null) return _toString;
            _toString = "(";
            foreach (var coord in Path)
            {
                _toString += string.Format("[{0};{1}]-", coord.Item1, coord.Item2);
            }
            _toString = _toString.Remove(_toString.Length - 1);
            _toString += ")";
            return _toString;
        }

        public override LinkedList<Tuple<int, int>> GetPathAsTupleSeries()
        {
            return Path;
        }
    }
}
