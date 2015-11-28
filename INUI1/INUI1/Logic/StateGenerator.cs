using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.Logic
{
    public class StateGenerator
    {
        public Setup Setup { get; set; }

        public StateGenerator(Setup setup)
        {
            Setup = setup;
        }

        public void OptimisticGeneration(Dictionary<string, State> states, Tuple<int, int> numberCoordinates)
        {
            var pathLenght = Setup.Board[numberCoordinates.Item1, numberCoordinates.Item2];
            if(pathLenght <= 1) return;

            var stateWithHorPath = CreateStateWithSimpleHorizontalPath(numberCoordinates, pathLenght, pathLenght/2);
            if(IsSimplePathValid(stateWithHorPath.Path as SimplePath) && !states.ContainsKey(stateWithHorPath.Hash))
                states.Add(stateWithHorPath.Hash, stateWithHorPath);

            var stateWithVertPath = CreateStateWithSimpleVerticalPath(numberCoordinates, pathLenght, pathLenght / 2);
            if (IsSimplePathValid(stateWithVertPath.Path as SimplePath) && !states.ContainsKey(stateWithVertPath.Hash))
                states.Add(stateWithVertPath.Hash, stateWithVertPath);

        }

        public void RealisticGeneration(Dictionary<string, State> states, Tuple<int, int> numberCoordinates)
        {
            var pathLenght = Setup.Board[numberCoordinates.Item1, numberCoordinates.Item2];
            if (pathLenght <= 1) return;

            for (int i = 0; i <= pathLenght/2; i++)
            {
                var stateWithHorPath = CreateStateWithSimpleHorizontalPath(numberCoordinates, pathLenght, pathLenght/2 - i);
                if (IsSimplePathValid(stateWithHorPath.Path as SimplePath) && !states.ContainsKey(stateWithHorPath.Hash))
                    states.Add(stateWithHorPath.Hash, stateWithHorPath);

                var stateWithVertPath = CreateStateWithSimpleVerticalPath(numberCoordinates, pathLenght, pathLenght / 2 - i);
                if (IsSimplePathValid(stateWithVertPath.Path as SimplePath) && !states.ContainsKey(stateWithVertPath.Hash))
                    states.Add(stateWithVertPath.Hash, stateWithVertPath);
            }
        }

        public void PesimisticGeneration(Dictionary<string, State> states, Tuple<int, int> numberCoordinates)
        {
            var pathLenght = Setup.Board[numberCoordinates.Item1, numberCoordinates.Item2];
            if (pathLenght <= 1) return;

            for (int i = 0; i <= pathLenght; i++)
            {
                var stateWithHorPath = CreateStateWithSimpleHorizontalPath(numberCoordinates, pathLenght, pathLenght - i);
                if (IsSimplePathValid(stateWithHorPath.Path as SimplePath) && !states.ContainsKey(stateWithHorPath.Hash))
                    states.Add(stateWithHorPath.Hash, stateWithHorPath);

                var stateWithVertPath = CreateStateWithSimpleVerticalPath(numberCoordinates, pathLenght, pathLenght - i);
                if (IsSimplePathValid(stateWithVertPath.Path as SimplePath) && !states.ContainsKey(stateWithVertPath.Hash))
                    states.Add(stateWithVertPath.Hash, stateWithVertPath);
            }
        }

        private State CreateStateWithSimpleHorizontalPath(Tuple<int, int> centre, int lenght, int offset)
        {
            var horizontalPath =
                new SimplePath(
                    SimplePathType.Horizontal,
                    new Tuple<int, int>(centre.Item1 - offset, centre.Item2),
                    new Tuple<int, int>(centre.Item1 - offset + lenght - 1, centre.Item2));
            return new State(horizontalPath);
        }

        private State CreateStateWithSimpleVerticalPath(Tuple<int, int> centre, int lenght, int offset)
        {
            var horizontalPath =
                new SimplePath(
                    SimplePathType.Vertical,
                    new Tuple<int, int>(centre.Item1, centre.Item2 - offset),
                    new Tuple<int, int>(centre.Item1, centre.Item2 - offset + lenght - 1));
            return new State(horizontalPath);
        }

        private bool IsSimplePathValid(SimplePath path)
        {
            if (path.Start.Item1 < 0 || path.Start.Item1 > Setup.Board.GetLength(0)) return false;
            if (path.Start.Item2 < 0 || path.Start.Item2 > Setup.Board.GetLength(1)) return false;
            return true;
        }
    }
}
