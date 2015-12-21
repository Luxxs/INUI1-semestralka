using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INUI1.Logic
{
    public class HledaniCesty
    {
        public int[,] matice { get; private set; }
        private List<Bod> vyznamneBody;
        private readonly bool[,] prazdnaCesta;

        public HledaniCesty(int[,] mat)
        {
            matice = mat;
            vyznamneBody = new List<Bod>();
            prazdnaCesta = new bool[matice.GetLength(0), matice.GetLength(1)];
            naplnCestu();
        }

        public bool[,] Vypocti()
        {
            if (matice == null)
            {
                throw new InvalidOperationException("Matice není nastavena");
            }

            vyznamneBody.Clear();
            naidiVyznamneBody();
            //Vygeneruje k bodum mozne cesty
            generujCesty();
            //Alg na katezsky soucin
            List<bool[,]> listCest = vytvorKompletniListCest();
            //Alg vyradi jen uz platne cesty
            List<bool[,]> listPlatnychCest = provedKontroly(listCest);
            listCest.Clear();   //Uklid

            if (listPlatnychCest.Count == 0)
            {
                throw new ArgumentException("Výsledek nenalezen");
            }
            return listPlatnychCest.ElementAt(0);
        }

        private List<bool[,]> provedKontroly(List<bool[,]> listCest)
        {
            //Alg vrati jen souvisle cesty
            List<bool[,]> listSouvislychCest = provedKOnttroluSouvislosti(listCest);
            //Alg vyradi cesty se cttvercem
            List<bool[,]> listPlatnychCest = kontrolaCvercu(listSouvislychCest);
            listSouvislychCest.Clear(); //Uklid
            //Alg prekontroluje delkycest
            List<bool[,]> listPom = kontrolaDelekCest(listPlatnychCest);

            return listPom;
        }

        private List<bool[,]> kontrolaCvercu(List<bool[,]> listSouvislychCest)
        {
            List<bool[,]> listPlatnychCest = new List<bool[,]>();
            foreach (var cesta in listSouvislychCest)
            {
                bool pridat = true;
                for (int i = 0; i < cesta.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j < cesta.GetLength(1) - 1; j++)
                    {
                        if (cesta[i, j] && cesta[i + 1, j] && cesta[i, j + 1] && cesta[i + 1, j + 1])
                        {
                            pridat = false;
                        }
                    }
                }

                if (pridat)
                {
                    listPlatnychCest.Add(cesta);
                }
            }
            return listPlatnychCest;
        }

        private List<bool[,]> kontrolaDelekCest(List<bool[,]> listPlatnychCest)
        {
            List<bool[,]> listPom = new List<bool[,]>();
            foreach (var cesta in listPlatnychCest)
            {
                bool pridat = true;
                foreach (var bod in vyznamneBody)
                {
                    if (!dpovidaDelkaCeste(bod, cesta))
                    {
                        pridat = false;
                    }
                }
                if (pridat)
                {
                    listPom.Add(cesta);
                }
            }
            return listPom;
        }

        private bool dpovidaDelkaCeste(Bod bod, bool[,] cesta)
        {
            int delkaCesty = 0;
            if ((bod.x - 1 < 0 || !cesta[bod.x - 1, bod.y]) && (bod.x + 1 == cesta.GetLength(0) || !cesta[bod.x + 1, bod.y]))
            {
                int y = bod.y;
                while (y - 1 >= 0 && cesta[bod.x, y - 1])
                {
                    y--;
                }
                while (y < cesta.GetLength(1) && cesta[bod.x, y])
                {
                    y++;
                    delkaCesty++;
                }
            }
            else if ((bod.y - 1 < 0 || !cesta[bod.x, bod.y - 1]) && (bod.y + 1 == cesta.GetLength(1) || !cesta[bod.x, bod.y + 1]))
            {
                int x = bod.x;
                while (x - 1 >= 0 && cesta[x - 1, bod.y])
                {
                    x--;
                }
                while (x < cesta.GetLength(0) && cesta[x, bod.y])
                {
                    x++;
                    delkaCesty++;
                }
            }
            else
            {
                int y = bod.y;
                while (y - 1 >= 0 && cesta[bod.x, y - 1])
                {
                    y--;
                }
                while (y < cesta.GetLength(1) && cesta[bod.x, y])
                {
                    y++;
                    delkaCesty++;
                }
                int x = bod.x;
                while (x - 1 >= 0 && cesta[x - 1, bod.y])
                {
                    x--;
                }
                while (x < cesta.GetLength(0) && cesta[x, bod.y])
                {
                    x++;
                    delkaCesty++;
                }
                return bod.hodnotaBodu * 2 == delkaCesty;
            }
            return bod.hodnotaBodu == delkaCesty;
        }

        private List<bool[,]> provedKOnttroluSouvislosti(List<bool[,]> listCest)
        {
            List<bool[,]> listSouvislych = new List<bool[,]>();
            foreach (var cesta in listCest)
            {
                int delkaCesty = 0;
                bool prvni = true;
                Bod prvniBod = new Bod(0, 0, 0);
                for (int i = 0; i < cesta.GetLength(0); i++)
                {
                    for (int j = 0; j < cesta.GetLength(1); j++)
                    {
                        if (cesta[i, j])
                        {
                            delkaCesty++;
                            if (prvni)
                            {
                                prvniBod = new Bod(i, j, 0);
                                prvni = false;
                            }
                        }
                    }
                }
                int pocetProjitych = vratDelkuSouvisleCesty(cesta, prvniBod);
                if (pocetProjitych == delkaCesty)
                {
                    listSouvislych.Add(cesta);
                }
            }

            return listSouvislych;
        }

        private int vratDelkuSouvisleCesty(bool[,] cesta, Bod prvniBod)
        {
            Stack<Tuple<int, int>> zasobnik = new Stack<Tuple<int, int>>();
            zasobnik.Push(new Tuple<int, int>(prvniBod.x, prvniBod.y));
            List<Tuple<int, int>> projite = new List<Tuple<int, int>>();

            while (zasobnik.Count > 0)
            {
                var bod = zasobnik.Pop();
                projite.Add(bod);
                if (bod.Item1 - 1 >= 0 && cesta[bod.Item1 - 1, bod.Item2])
                {
                    var pom = new Tuple<int, int>(bod.Item1 - 1, bod.Item2);
                    if (!projite.Contains(pom) && !zasobnik.Contains(pom))
                    {
                        zasobnik.Push(pom);
                    }
                }
                if (bod.Item1 + 1 < cesta.GetLength(0) && cesta[bod.Item1 + 1, bod.Item2])
                {
                    var pom = new Tuple<int, int>(bod.Item1 + 1, bod.Item2);
                    if (!projite.Contains(pom) && !zasobnik.Contains(pom))
                    {
                        zasobnik.Push(pom);
                    }
                }
                if (bod.Item2 - 1 >= 0 && cesta[bod.Item1, bod.Item2 - 1])
                {
                    var pom = new Tuple<int, int>(bod.Item1, bod.Item2 - 1);
                    if (!projite.Contains(pom) && !zasobnik.Contains(pom))
                    {
                        zasobnik.Push(pom);
                    }
                }
                if (bod.Item2 + 1 < cesta.GetLength(1) && cesta[bod.Item1, bod.Item2 + 1])
                {
                    var pom = new Tuple<int, int>(bod.Item1, bod.Item2 + 1);
                    if (!projite.Contains(pom) && !zasobnik.Contains(pom))
                    {
                        zasobnik.Push(pom);
                    }
                }
            }

            return projite.Count;
        }

        private List<bool[,]> vytvorKompletniListCest()
        {
            int pocet = vyznamneBody.Count;
            List<bool[,]> listCest = vyznamneBody.ElementAt(0).mozneCesty;
            for (int i = 1; i < pocet; i++)
            {
                List<bool[,]> listSpojeny = new List<bool[,]>();
                var list = vyznamneBody.ElementAt(i).mozneCesty;
                foreach (var cesta1 in listCest)
                {
                    foreach (var cesta2 in list)
                    {
                        bool[,] novaCesta = sjednoceniCest(cesta1, cesta2);
                        listSpojeny.Add(novaCesta);
                    }
                }
                listCest = listSpojeny;
            }
            return listCest;
        }

        private void generujCesty()
        {
            foreach (var vyznamnyBod in vyznamneBody)
            {
                bool svisle = true;
                bool vodorovne = true;
                foreach (var bod in vyznamneBody)
                {
                    if (vyznamnyBod != bod)
                    {
                        if (svisle && jeMozneSvisleNapojeni(vyznamnyBod, bod))
                        {
                            svisle = false;
                            generujSvisleCesty(vyznamnyBod);
                        }
                        if (vodorovne && jeMozneVodorovneNapojeni(vyznamnyBod, bod))
                        {
                            vodorovne = false;
                            generujVodorovneCesty(vyznamnyBod);
                        }
                    }
                }
            }
        }

        private bool[,] sjednoceniCest(bool[,] cesta1, bool[,] cesta2)
        {
            bool[,] vyslednaCesta = (bool[,])cesta1.Clone();
            for (int i = 0; i < cesta2.GetLength(0); i++)
            {
                for (int j = 0; j < cesta2.GetLength(1); j++)
                {
                    if (cesta2[i, j])
                    {
                        vyslednaCesta[i, j] = cesta2[i, j];
                    }
                }
            }
            return vyslednaCesta;
        }

        private void generujSvisleCesty(Bod bod)
        {
            for (int j = -(bod.hodnotaBodu - 1); j <= 0; j++)
            {
                if ((bod.y + j) >= 0 && (j + bod.y + bod.hodnotaBodu) <= matice.GetLength(1))
                {
                    bool[,] cesta = (bool[,])prazdnaCesta.Clone();
                    for (int k = 0; k < bod.hodnotaBodu; k++)
                    {
                        cesta[bod.x, j + k + bod.y] = true;
                    }
                    bod.mozneCesty.Add(cesta);
                }
            }
        }

        private void generujVodorovneCesty(Bod bod)
        {
            for (int j = -(bod.hodnotaBodu - 1); j <= 0; j++)
            {
                if ((bod.x + j) >= 0 && (j + bod.x + bod.hodnotaBodu) <= matice.GetLength(0))
                {
                    bool[,] cesta = (bool[,])prazdnaCesta.Clone();
                    for (int k = 0; k < bod.hodnotaBodu; k++)
                    {
                        cesta[bod.x + j + k, bod.y] = true;
                    }
                    bod.mozneCesty.Add(cesta);
                }
            }
        }

        private bool jeMozneSvisleNapojeni(Bod zkoumany, Bod kontrolovany)
        {
            if (Math.Abs(zkoumany.y - kontrolovany.y) >= zkoumany.hodnotaBodu)
            {
                return false;
            }
            if (Math.Abs(zkoumany.x - kontrolovany.x) >= kontrolovany.hodnotaBodu)
            {
                return false;
            }
            return true;
        }

        private bool jeMozneVodorovneNapojeni(Bod zkoumany, Bod kontrolovany)
        {
            if (Math.Abs(zkoumany.x - kontrolovany.x) >= zkoumany.hodnotaBodu)
            {
                return false;
            }
            if (Math.Abs(zkoumany.y - kontrolovany.y) >= kontrolovany.hodnotaBodu)
            {
                return false;
            }
            return true;
        }

        private bool kontrolaOkraju(Bod bod)
        {
            if (matice.GetLength(0) < bod.x && matice.GetLength(1) < bod.y && bod.y >= 0 && bod.x >= 0)
            {
                return true;
            }
            return false;
        }

        private void naplnCestu()
        {
            for (int i = 0; i < prazdnaCesta.GetLength(0); i++)
            {
                for (int j = 0; j < prazdnaCesta.GetLength(1); j++)
                {
                    prazdnaCesta[i, j] = false;
                }
            }
        }

        private void naidiVyznamneBody()
        {
            for (int i = 0; i < matice.GetLength(0); i++)
            {
                for (int j = 0; j < matice.GetLength(1); j++)
                {
                    if (matice[i, j] != 0)
                    {
                        vyznamneBody.Add(new Bod(i, j, matice[i, j]));
                    }
                }
            }
        }

        private class Bod
        {
            public int x { get; private set; }
            public int y { get; private set; }
            public List<bool[,]> mozneCesty { get; private set; }
            public int hodnotaBodu { get; private set; }

            public Bod(int x, int y, int hodnotaBodu)
            {
                this.x = x;
                this.y = y;
                this.hodnotaBodu = hodnotaBodu;
                mozneCesty = new List<bool[,]>();
            }
        }
    }
}
