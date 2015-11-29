using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using INUI1.Model;

namespace INUI1.Logic
{
    public class PathGenerator
    {
        private Cell[,] _cells { get; set; }

        public PathGenerator(Cell[,] cells)
        {
            _cells = cells;
        }

        public void OptimisticGeneration(Dictionary<string, Path> paths, Tuple<int, int> numberCoordinates)
        {
            var pathLenght = _cells[numberCoordinates.Item2, numberCoordinates.Item1].Value;
            if(pathLenght <= 1) return;

            var horizontalPath = CreateStateWithSimpleHorizontalPath(numberCoordinates, pathLenght, pathLenght/2);
            if (IsPathValid(horizontalPath) && !paths.ContainsKey(horizontalPath.ToString()))
                paths.Add(horizontalPath.ToString(), horizontalPath);

            var verticalPath = CreateStateWithSimpleVerticalPath(numberCoordinates, pathLenght, pathLenght / 2);
            if (IsPathValid(verticalPath) && !paths.ContainsKey(verticalPath.ToString()))
                paths.Add(verticalPath.ToString(), verticalPath);

        }

        public void RealisticGeneration(Dictionary<string, Path> paths, Tuple<int, int> numberCoordinates)
        {
            var pathLenght = _cells[numberCoordinates.Item2, numberCoordinates.Item1].Value;
            if (pathLenght <= 1) return;

            for (int i = 0; i <= pathLenght/2; i++)
            {
                var horizontalPath = CreateStateWithSimpleHorizontalPath(numberCoordinates, pathLenght, pathLenght / 2 - i);
                if (IsPathValid(horizontalPath) && !paths.ContainsKey(horizontalPath.ToString()))
                    paths.Add(horizontalPath.ToString(), horizontalPath);

                var verticalPath = CreateStateWithSimpleVerticalPath(numberCoordinates, pathLenght, pathLenght / 2 - i);
                if (IsPathValid(verticalPath) && !paths.ContainsKey(verticalPath.ToString()))
                    paths.Add(verticalPath.ToString(), verticalPath);
            }
        }

        public void PesimisticGeneration(Dictionary<string, Path> paths, Tuple<int, int> numberCoordinates)
        {
            var pathLenght = _cells[numberCoordinates.Item2, numberCoordinates.Item1].Value;
            if (pathLenght <= 1) return;

            for (int i = 0; i <= pathLenght; i++)
            {
                var horizontalPath = CreateStateWithSimpleHorizontalPath(numberCoordinates, pathLenght, pathLenght - i);
                if (IsPathValid(horizontalPath) && !paths.ContainsKey(horizontalPath.ToString()))
                    paths.Add(horizontalPath.ToString(), horizontalPath);

                var verticalPath = CreateStateWithSimpleVerticalPath(numberCoordinates, pathLenght, pathLenght - i);
                if (IsPathValid(verticalPath) && !paths.ContainsKey(verticalPath.ToString()))
                    paths.Add(verticalPath.ToString(), verticalPath);
            }
        }

        private Path CreateStateWithSimpleHorizontalPath(Tuple<int, int> centre, int lenght, int offset)
        {
            var points = new LinkedList<Tuple<int, int>>();
            for (int i = 0; i < lenght; i++)
            {
                points.AddLast(new Tuple<int, int>(centre.Item1 - offset + i, centre.Item2));
            }

            return new Path {Points = points};
        }

        private Path CreateStateWithSimpleVerticalPath(Tuple<int, int> centre, int lenght, int offset)
        {
            var points = new LinkedList<Tuple<int, int>>();
            for (int i = 0; i < lenght; i++)
            {
                points.AddLast(new Tuple<int, int>(centre.Item1, centre.Item2 - offset + i));
            }

            return new Path { Points = points };
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
