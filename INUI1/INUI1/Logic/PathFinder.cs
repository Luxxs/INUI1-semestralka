using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INUI1.Model;

namespace INUI1.Logic
{
    class PathFinder
    {
        private Cell[,] _cells;

        /* Seznam dvojic dvojice - slovnik. 
         * Dvojice je souradnice, okolo ktere vznikla cesta a samotna cesta
         * Jsou to ty zakladni cesty, ktere se budou spojovat.
         * 
         * Pro lepsi prehlednost by bylo mozne zdedit Dictionary a doplnit mu field description 
         * (kde by byla souradnice, okolo ktere cesty vznikaly),
         * pak bychom meli jen LinkedList<DictionaryWithDescription> _baseDictionaries
         */
        private LinkedList<Tuple<Tuple<int, int>, Dictionary<string, Path>>> _baseDictionaries;

        // pouziva se k vygenerovani cest okolo cisel
        private PathGenerator _generator;

        // pouziva se ke kontrole, ze se cesty protinaji a ke spojovani cest
        private PathManager _manager;

        // pocet cisel v Setupu
        int _numbersCount = 0;

        // maximum "kol" (v kole se spojuji cesty)
        int _maxRounds = 0;

        public PathFinder(Cell[,] cells)
        {
            _cells = cells;
            _generator = new PathGenerator(_cells);
            _manager = new PathManager();

            _baseDictionaries = new LinkedList<Tuple<Tuple<int, int>, Dictionary<string, Path>>>();

            Init();
        }

        /* alg: 
             * list slovniku, slovnik je mnozina stavu kolem jednoho cisla
             * vygenerovat optimisticke stavy (do simplePaths)
             * najit spolecne cesty a dat je do nextRound
             * z tech co byly nextRound se stanou thisRound
             * pokracovat dokud se neudela log2(n) kol (dvojice, ctverice, atd)
             * 
             * pokud optimitstickym nic nenalezeno, pokracovat realistickym a pri nejhorsim pesimistickym (komplet) generovanim
             */
        public Path FindPath()
        {
            // nejdrive hledame pres optimistic
            var resultOptimistic = CombineStatesUntilPathIsFound(_baseDictionaries.Select(tupleCoordDict => tupleCoordDict.Item2).ToList());
            if (resultOptimistic != null)
                return resultOptimistic;
            
            // nenaslo se pres optimistic
            // rozsirime _baseDictionaries
            foreach (var tupleCoordDict in _baseDictionaries)
                _generator.RealisticGeneration(tupleCoordDict.Item2, tupleCoordDict.Item1);

            // mozna optimalizace - znovu se prohledavaji ty co v optimistic 
            // tady to ma vliv minimalni, u pesimistic uz to ale je vykonostni problem
            // u pesimistic se prohledavaji znovu ty, co u optimistic
            var resultRealistic = CombineStatesUntilPathIsFound(_baseDictionaries.Select(tupleCoordDict => tupleCoordDict.Item2).ToList());
            if (resultRealistic != null)
                return resultRealistic;


            // nenaslo se pres realistic
            // rozsirime _baseDictionaries (tentokrat vsechny cesty okolo cisel)
            foreach (var tupleCoordDict in _baseDictionaries)
                _generator.PesimisticGeneration(tupleCoordDict.Item2, tupleCoordDict.Item1);

            var resultPesimistic = CombineStatesUntilPathIsFound(_baseDictionaries.Select(tupleCoordDict => tupleCoordDict.Item2).ToList());
            if (resultPesimistic != null)
                return resultPesimistic;

            // vubec se nenaslo
            return null;
        }

        private void Init()
        {
            for (int y = 0; y < _cells.GetLength(0); y++)
            {
                for (int x = 0; x < _cells.GetLength(1); x++)
                {
                    if (_cells[x, y].InPath)
                    {
                        var dict = new Dictionary<string, Path>();
                        var coord = new Tuple<int, int>(x, y);
                        _baseDictionaries.AddLast(new Tuple<Tuple<int, int>, Dictionary<string, Path>>(coord, dict));
                        _generator.OptimisticGeneration(dict, coord);
                        _numbersCount++;
                    }
                }
            }

            // maximalne pocet kol je roven log2(n)
            // v prvnim kole vytvarime dvojice, pak ctverice, pak osmice atd.
            // zaokrouhledni nahoru proto, aby se prosly vsechny skupiny
            _maxRounds = (int)Math.Ceiling(Math.Log(_numbersCount, 2));
        }

        /// <summary>
        /// Hleda cestu mezi cisly postup spojovanim.
        /// </summary>
        /// <param name="firstRound"></param>
        /// <returns>Nalezena cesta nebo null, pokud cestu nenajde.</returns>
        private Path CombineStatesUntilPathIsFound(List<Dictionary<string, Path>> firstRound)
        {
            var thisRound = firstRound;
            var nextRound = new List<Dictionary<string, Path>>();
            var roundCount = 0;


            // na princip bubblesortu prochazime vsechny stavy ve skupine 
            // a zkousime je spojit se vsema stavama z ostatnich skupin
            do
            {
                for (int i = 0; i < thisRound.Count; i++)
                {
                    for (int j = i; j < thisRound.Count; j++)
                    {
                        foreach (var stateI in thisRound[i])
                        {
                            // slovnik predstavujuci skupinu vzniklou spojenim dvou "jednodussich" skupin
                            // napriklad dvojice vznikla spojenim dvou jednoduch cest
                            var dict = new Dictionary<string, Path>();
                            foreach (var stateJ in thisRound[j])
                            {
                                var intersect = _manager.FindPathsIntersect(stateI.Value, stateJ.Value);
                                if (intersect != null)
                                {
                                    var path = _manager.JoinPaths(stateI.Value, stateJ.Value, intersect);
                                    dict.Add(path.ToString(), path);
                                }
                            }
                            // pokud vznikly nejake cesty, tak je v pristim kole budeme spojovat
                            if (dict.Count > 0)
                                nextRound.Add(dict);
                        }
                    }
                }

                // priprava na dalsi kolo
                thisRound = nextRound;
                nextRound = new List<Dictionary<string, Path>>();
                roundCount++;
            } 
            // pokracujeme, pokud jsme neprojeli vsechny kole a mame v pristime kole co spojovat
            while (roundCount <= _maxRounds && thisRound.Count > 0);

            // pokud jsme projeli vsechna kola...
            if (roundCount == _maxRounds)
            {
                // ...a pokud jsme nasli cestu, tak bude v nextRound[0].First()
                if (nextRound.Count > 0 && nextRound[0].First().Value != null)
                    return nextRound[0].First().Value;
            }
            
            return null;
        }
    }
}
