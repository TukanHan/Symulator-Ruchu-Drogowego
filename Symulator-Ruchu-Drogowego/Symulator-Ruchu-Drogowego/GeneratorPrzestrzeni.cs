using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public struct Kwadrat
    {
        public Punkt<int> Pozycja { get; }
        public int Wysokosc { get; }
        public int Szerokosc { get; }

        public Kwadrat(Punkt<int> pozycja, int wysokosc, int szerokosc)
        {
            Pozycja = pozycja;
            Wysokosc = wysokosc;
            Szerokosc = szerokosc;
        }

        public Kwadrat(Punkt<int> punktA, Punkt<int> punktB)
        {
            Pozycja = punktA;
            Wysokosc = punktB.Y - punktA.Y + 1;
            Szerokosc = punktB.X - punktA.X + 1;
        }
    }

    public class GeneratorPrzestrzeni
    {    
        public List<Kwadrat> Budynki { get; private set; } = new List<Kwadrat>();
        public List<WierzcholekPieszych> WierzcholkiChodnikow { get; private set; } = new List<WierzcholekPieszych>();
        public List<KrawedzGrafu> Chodniki { get; private set; } = new List<KrawedzGrafu>();

        private Random generatorLosowosci = new Random();
        private int rozmiarMapyX;
        private int rozmiarMapyY;
        private bool[,] mapa;

        public GeneratorPrzestrzeni(int rozmiarMapyX, int rozmiarMapyY, GeneratorPolaczenSamochodow generatorPolaczen)
        {
            this.rozmiarMapyX = rozmiarMapyX*2;
            this.rozmiarMapyY = rozmiarMapyY*2;

            mapa = new bool[this.rozmiarMapyX, this.rozmiarMapyY];

            OdwzorujDroge(generatorPolaczen);
            GenerujChodniki();
            GenerujBudynki();
        }

        /// <summary>
        /// Przenoszenie na mapę drogi z generatoraPołączeń
        /// </summary>
        private void OdwzorujDroge(GeneratorPolaczenSamochodow generatorPolaczen)
        {
            foreach(KrawedzGrafu droga in generatorPolaczen.Drogi)
            {
                Punkt<int> punkt = (Punkt<int>)droga.WierzcholekA.Pozycja;

                for (int i = 0; i <= droga.DlugoscKrawedzi(); ++i)
                {
                    if ((punkt.Y + i) * 2 < 0 || (punkt.X + i) * 2 < 0 || (punkt.Y + i) * 2  >= rozmiarMapyY -1 || (punkt.X + i) * 2>= rozmiarMapyX -1)
                        continue;

                    if (droga.ZwrocRelacje() == Relacja.Pionowe)
                    {
                        mapa[punkt.X * 2, (punkt.Y + i) * 2] = true;
                        mapa[punkt.X * 2 + 1, (punkt.Y + i) * 2] = true;
                        mapa[punkt.X * 2, (punkt.Y + i) * 2 + 1] = true;
                        mapa[punkt.X * 2 + 1, (punkt.Y + i) * 2 + 1] = true;
                    }
                    else
                    {
                        mapa[(punkt.X + i) * 2, punkt.Y * 2] = true;
                        mapa[(punkt.X + i) * 2 + 1, punkt.Y * 2] = true;
                        mapa[(punkt.X + i) * 2, punkt.Y * 2 + 1] = true;
                        mapa[(punkt.X + i) * 2 + 1, punkt.Y * 2 + 1] = true;
                    }
                }
            }
        }

        /// <summary>
        /// Wybór chodników w miejscach optymalnych przez podział przestrzeni
        /// </summary>
        private void GenerujChodniki()
        {
            List<Kwadrat> kwadraty = ZbierzWolneKwadraty();

            foreach(Kwadrat kwadrat in kwadraty)
            {
                bool czyMogePionowo = kwadrat.Szerokosc > 2 ? true : false;
                bool czyMogePoziomo = kwadrat.Wysokosc > 2 ? true : false;

                if (czyMogePoziomo && czyMogePionowo)
                {
                    if ((rozmiarMapyX <= kwadrat.Pozycja.X + kwadrat.Szerokosc || kwadrat.Pozycja.X == 0) &&
                        !(rozmiarMapyY <= kwadrat.Pozycja.Y + kwadrat.Wysokosc || kwadrat.Pozycja.Y == 0))
                        czyMogePoziomo = false;
                    else if ((rozmiarMapyY <= kwadrat.Pozycja.Y + kwadrat.Wysokosc || kwadrat.Pozycja.Y == 0) &&
                        !(rozmiarMapyX <= kwadrat.Pozycja.X + kwadrat.Szerokosc || kwadrat.Pozycja.X == 0))
                        czyMogePionowo = false;
                    else
                    {
                        if (kwadrat.Wysokosc > kwadrat.Szerokosc)
                            czyMogePionowo = false;
                        else if (kwadrat.Szerokosc > kwadrat.Wysokosc)
                            czyMogePoziomo = false;
                        else
                        {
                            if (generatorLosowosci.Next(0, 2) == 0)
                                czyMogePionowo = false;
                            else
                                czyMogePoziomo = false;
                        }
                    }
                }

                if (czyMogePionowo)
                {
                    int x = (kwadrat.Szerokosc / 2) + kwadrat.Pozycja.X;
                    int y1 = kwadrat.Pozycja.Y;
                    int y2 = kwadrat.Wysokosc + kwadrat.Pozycja.Y;

                    BudujChodnik(new Punkt<double>(x, y1), new Punkt<double>(x, y2-1));
                }
                else if(czyMogePoziomo)
                {
                    int x1 = kwadrat.Pozycja.X;
                    int x2 = kwadrat.Szerokosc + kwadrat.Pozycja.X;
                    int y = (kwadrat.Wysokosc / 2) + kwadrat.Pozycja.Y;

                    BudujChodnik(new Punkt<double>(x1, y), new Punkt<double>(x2-1, y));
                }
            }
        }

        /// <summary>
        /// Generuje budynki w miejscach optymalnych przez podział przestrzeni
        /// </summary>
        private void GenerujBudynki()
        {      
            List<Kwadrat> kwadraty = ZbierzWolneKwadraty();

            while (kwadraty.Count>0)
            {
                for(int l=kwadraty.Count-1; l>=0; --l)
                {
                    Kwadrat kwadrat = kwadraty[l];

                    if(kwadrat.Wysokosc>=2 && kwadrat.Szerokosc>=2)
                    {
                        bool czyDziele = generatorLosowosci.Next(0, 2) == 1 ? true : false;
                        if (kwadrat.Wysokosc > 8 || kwadrat.Szerokosc > 8 || kwadrat.Wysokosc * kwadrat.Szerokosc > 16)
                            czyDziele = true;
                        else if (kwadrat.Wysokosc * kwadrat.Szerokosc <= 8 || (kwadrat.Szerokosc<=4 && kwadrat.Wysokosc<=4))
                            czyDziele = false;

                        if(czyDziele)
                        {
                            bool dzielePionowo =  true;
                            bool dzielePoziomo =  true;

                            if (kwadrat.Wysokosc > kwadrat.Szerokosc + 1)
                                dzielePionowo = false;
                            else if (kwadrat.Szerokosc > kwadrat.Wysokosc + 1)
                                dzielePoziomo = false;
                            else
                            {
                                if (generatorLosowosci.Next(0, 2) == 0)
                                    dzielePionowo = false;
                                else
                                    dzielePoziomo = false;
                            }

                            if(dzielePionowo)
                            {
                                kwadraty.Add(new Kwadrat(kwadrat.Pozycja, kwadrat.Wysokosc, kwadrat.Szerokosc - (kwadrat.Szerokosc / 2)));
                                kwadraty.Add(new Kwadrat(new Punkt<int>(kwadrat.Pozycja.X + (kwadrat.Szerokosc - (kwadrat.Szerokosc / 2)), kwadrat.Pozycja.Y), kwadrat.Wysokosc, kwadrat.Szerokosc / 2));
                            }
                            else if(dzielePoziomo)
                            {
                                kwadraty.Add(new Kwadrat(kwadrat.Pozycja, kwadrat.Wysokosc - (kwadrat.Wysokosc / 2), kwadrat.Szerokosc));
                                kwadraty.Add(new Kwadrat(new Punkt<int>(kwadrat.Pozycja.X, kwadrat.Pozycja.Y + (kwadrat.Wysokosc - (kwadrat.Wysokosc /2))), kwadrat.Wysokosc / 2, kwadrat.Szerokosc));
                            }

                            kwadraty.Remove(kwadrat);
                        }
                        else
                        {                           
                            Budynki.Add(kwadrat);
                            kwadraty.Remove(kwadrat);

                            for (int j = kwadrat.Pozycja.Y; j < kwadrat.Pozycja.Y + kwadrat.Wysokosc; ++j)
                                for (int i = kwadrat.Pozycja.X; i < kwadrat.Pozycja.X + kwadrat.Szerokosc; ++i)
                                    mapa[i, j] = true;
                        }
                    }
                    else                    
                        kwadraty.Remove(kwadrat);                    
                }
            }     
        }

        private void BudujChodnik(Punkt<double> punktA, Punkt<double> punktB)
        {
            WierzcholekPieszych wierzcholekA;
            if (punktA.X == 0)
                wierzcholekA = new WierzcholekPieszych(new Punkt<double>(punktA.X - 1, punktA.Y), TypWierzcholkaPieszych.PunktWejscia);
            else if(punktA.Y == 0)
                wierzcholekA = new WierzcholekPieszych(new Punkt<double>(punktA.X, punktA.Y - 1), TypWierzcholkaPieszych.PunktWejscia);
            else
                wierzcholekA = new WierzcholekPieszych(punktA, TypWierzcholkaPieszych.ChodnikPrzestrzeni);

            WierzcholekPieszych wierzcholekB;
            if (punktB.X == rozmiarMapyX -1)
                wierzcholekB = new WierzcholekPieszych(new Punkt<double>(punktB.X + 1, punktB.Y), TypWierzcholkaPieszych.PunktWejscia);
            else if (punktB.Y == rozmiarMapyY - 1)
                wierzcholekB = new WierzcholekPieszych(new Punkt<double>(punktB.X, punktB.Y + 1), TypWierzcholkaPieszych.PunktWejscia);
            else
                wierzcholekB = new WierzcholekPieszych(punktB, TypWierzcholkaPieszych.ChodnikPrzestrzeni);

            WierzcholkiChodnikow.Add(wierzcholekA);
            WierzcholkiChodnikow.Add(wierzcholekB);
            Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, wierzcholekB));

            ZaznaczNaMapie(mapa, new Kwadrat((Punkt<int>)punktA, (Punkt<int>)punktB));
        }

        private List<Kwadrat> ZbierzWolneKwadraty()
        {
            List<Kwadrat> kwadraty = new List<Kwadrat>();

            bool[,] kopiaMapy = new bool[rozmiarMapyX, rozmiarMapyY];
            Array.Copy(mapa, kopiaMapy, mapa.Length);

            for (int j = 0; j < rozmiarMapyY; ++j)            
                for (int i = 0; i < rozmiarMapyX; ++i)                
                    if (!kopiaMapy[i, j])                   
                        kwadraty.Add(ZbierzKwadrat(kopiaMapy, new Punkt<int>(i, j)));

            return kwadraty;
        }

        private Kwadrat ZbierzKwadrat(bool[,] kopiaMapy, Punkt<int> pozycja)
        {
            int szerokosc = pozycja.X;
            for (; szerokosc < rozmiarMapyX; ++szerokosc)
                if (kopiaMapy[szerokosc, pozycja.Y])
                    break;


            int wysokosc = pozycja.Y;
            bool czasWyjsc = false;
            for (; wysokosc < rozmiarMapyY && !czasWyjsc; ++wysokosc)
                for (int i = pozycja.X; i < szerokosc; ++i)
                    if (kopiaMapy[i, wysokosc])
                    {
                        wysokosc--;
                        czasWyjsc = true;
                    }


            Kwadrat kwadrat = new Kwadrat(pozycja, wysokosc - pozycja.Y, szerokosc - pozycja.X);
            ZaznaczNaMapie(kopiaMapy, kwadrat);

            return kwadrat;
        }

        private void ZaznaczNaMapie(bool[,] mapa, Kwadrat kwadrat)
        {
            for (int j = kwadrat.Pozycja.Y; j < kwadrat.Pozycja.Y + kwadrat.Wysokosc && j < rozmiarMapyY; ++j)
                for (int i = kwadrat.Pozycja.X; i < kwadrat.Pozycja.X + kwadrat.Szerokosc && i < rozmiarMapyX; ++i)
                    mapa[i, j] = true;
        }
    }
}
