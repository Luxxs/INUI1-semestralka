using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using INUI1.Model;

namespace INUI1.Logic
{
    public class StateGenerator
    {
        private Cell[,] _cells { get; set; }

        public StateGenerator(Cell[,] cells)
        {
            _cells = cells;
        }

        public void OptimisticGeneration(Dictionary<string, State> states, Tuple<int, int> numberCoordinates)
        {
            var pathLenght = _cells[numberCoordinates.Item2, numberCoordinates.Item1].Value;
            if(pathLenght <= 1) return;

            var stateWithHorPath = CreateStateWithSimpleHorizontalPath(numberCoordinates, pathLenght, pathLenght/2);
            if(IsPathValid(stateWithHorPath.Path) && !states.ContainsKey(stateWithHorPath.Hash))
                states.Add(stateWithHorPath.Hash, stateWithHorPath);

            var stateWithVertPath = CreateStateWithSimpleVerticalPath(numberCoordinates, pathLenght, pathLenght / 2);
            if (IsPathValid(stateWithVertPath.Path) && !states.ContainsKey(stateWithVertPath.Hash))
                states.Add(stateWithVertPath.Hash, stateWithVertPath);

        }

        public void RealisticGeneration(Dictionary<string, State> states, Tuple<int, int> numberCoordinates)
        {
            var pathLenght = _cells[numberCoordinates.Item2, numberCoordinates.Item1].Value;
            if (pathLenght <= 1) return;

            for (int i = 0; i <= pathLenght/2; i++)
            {
                var stateWithHorPath = CreateStateWithSimpleHorizontalPath(numberCoordinates, pathLenght, pathLenght/2 - i);
                if (IsPathValid(stateWithHorPath.Path) && !states.ContainsKey(stateWithHorPath.Hash))
                    states.Add(stateWithHorPath.Hash, stateWithHorPath);

                var stateWithVertPath = CreateStateWithSimpleVerticalPath(numberCoordinates, pathLenght, pathLenght / 2 - i);
                if (IsPathValid(stateWithVertPath.Path) && !states.ContainsKey(stateWithVertPath.Hash))
                    states.Add(stateWithVertPath.Hash, stateWithVertPath);
            }
        }

        public void PesimisticGeneration(Dictionary<string, State> states, Tuple<int, int> numberCoordinates)
        {
            var pathLenght = _cells[numberCoordinates.Item2, numberCoordinates.Item1].Value;
            if (pathLenght <= 1) return;

            for (int i = 0; i <= pathLenght; i++)
            {
                var stateWithHorPath = CreateStateWithSimpleHorizontalPath(numberCoordinates, pathLenght, pathLenght - i);
                if (IsPathValid(stateWithHorPath.Path) && !states.ContainsKey(stateWithHorPath.Hash))
                    states.Add(stateWithHorPath.Hash, stateWithHorPath);

                var stateWithVertPath = CreateStateWithSimpleVerticalPath(numberCoordinates, pathLenght, pathLenght - i);
                if (IsPathValid(stateWithVertPath.Path) && !states.ContainsKey(stateWithVertPath.Hash))
                    states.Add(stateWithVertPath.Hash, stateWithVertPath);
            }
        }

        private State CreateStateWithSimpleHorizontalPath(Tuple<int, int> centre, int lenght, int offset)
        {
            var points = new LinkedList<Tuple<int, int>>();
            for (int i = 0; i < lenght; i++)
            {
                points.AddLast(new Tuple<int, int>(centre.Item1 - offset + i, centre.Item2));
            }

            return new State(new Path {Points = points});
        }

        private State CreateStateWithSimpleVerticalPath(Tuple<int, int> centre, int lenght, int offset)
        {
            var points = new LinkedList<Tuple<int, int>>();
            for (int i = 0; i < lenght; i++)
            {
                points.AddLast(new Tuple<int, int>(centre.Item1, centre.Item2 - offset + i));
            }

            return new State(new Path { Points = points });
        }

        private bool IsPathValid(Path path)
        {
            foreach (var point in path.Points)
            {
                if (point.Item1 < 0 || point.Item1 > _cells.GetLength(0)) return false;
                if (point.Item2 < 0 || point.Item2 > _cells.GetLength(1)) return false;
            }
            return true;
        }
    }
}
