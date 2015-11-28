using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.Logic
{
    public class PathManager
    {
        public Tuple<int, int> FindPathsIntersect(AbstractPath first, AbstractPath second)
        {
            return (
                from firstPathCoords in first.GetPathAsTupleSeries()
                from secondPathCoords in second.GetPathAsTupleSeries()
                where compareCoords(firstPathCoords, secondPathCoords)
                select firstPathCoords)
                .FirstOrDefault();
        }
        
        // seradit tak aby cesta sla z levyho horniho rohu do pravyho spodniho
        // aby se nestalo, ze budou dva stavy se stejnyma cestama vyhodnoceny jako stejny
        // spis v pathmanageru pri joinovani
        public JoinedPath JoinPaths(AbstractPath first, AbstractPath second, Tuple<int, int> intersect)
        {
            if (intersect == null)
                throw new ArgumentNullException("intersect", "Intersect can't be null");

            if (first is SimplePath && second is SimplePath)
            {
                if ((first as SimplePath).PathType.Equals((second as SimplePath).PathType))
                    throw new ArgumentException("Can't join simple paths of the same type");
            }
            
            // alg spojeni - jit z prvni dokud se nedostaneme na intersect
            // napojit druhou, pokud je druha joined a intersect neni to prvni / posledni, 
            // tak napojit obe vetve
            // dokoncit prvni

            var path = new LinkedList<Tuple<int, int>>();
            foreach (var point in first.GetPathAsTupleSeries())
            {
                path.AddLast(point);
                if (compareCoords(point, intersect))
                {
                    foreach (var secondPathPoint in GetSubPathAfterIntersect(second, point))
                        path.AddLast(secondPathPoint);

                    foreach (var secondPathPoint in GetSubPathBeforeIntersect(second, point))
                        path.AddLast(secondPathPoint);
                }
            }

            // udelat kontrolu, ze spojene cesty jsou na sebe kolme

            // taky kontrola, ze cesty vubec intersect obsahuji

            return new JoinedPath {Path = path};
        }

        private LinkedList<Tuple<int, int>> GetSubPathBeforeIntersect(AbstractPath path, Tuple<int, int> intersect)
        {
            var subPath = new LinkedList<Tuple<int, int>>();
            foreach (var point in path.GetPathAsTupleSeries().TakeWhile(point => !compareCoords(point, intersect)))
                subPath.AddLast(point);
            return subPath;
        }

        private LinkedList<Tuple<int, int>> GetSubPathAfterIntersect(AbstractPath path, Tuple<int, int> intersect)
        {
            var subPath = new LinkedList<Tuple<int, int>>();
            var intersectReached = false;
            foreach (var point in path.GetPathAsTupleSeries())
            {
                if (intersectReached)
                    subPath.AddLast(point);

                if (compareCoords(point, intersect))
                    intersectReached = true;
            }
            return subPath;
        }

        private bool compareCoords(Tuple<int, int> first, Tuple<int, int> second)
        {
            return first.Item1 == second.Item1 && first.Item2 == second.Item2;
        }
    }
}
