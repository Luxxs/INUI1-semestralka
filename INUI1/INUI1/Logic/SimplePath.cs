using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.Logic
{
    public enum SimplePathType { Horizontal, Vertical }

    public class SimplePath : AbstractPath
    {
        public SimplePathType PathType { get; set; }
        public Tuple<int, int> Start { get; set; }
        public Tuple<int, int> End { get; set; }

        public SimplePath(SimplePathType pathType)
        {
            PathType = pathType;
        }

        public SimplePath(SimplePathType pathType, Tuple<int, int> start, Tuple<int, int> end)
        {
            PathType = pathType;
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return string.Format("([{0};{1}]-[{2};{3}])", Start.Item1, Start.Item2, End.Item1, End.Item2);
        }

        public override LinkedList<Tuple<int, int>> GetPathAsTupleSeries()
        {
            var list = new LinkedList<Tuple<int, int>>();
            list.AddFirst(Start);
            list.AddLast(End);
            return list;
        }
    }
}
