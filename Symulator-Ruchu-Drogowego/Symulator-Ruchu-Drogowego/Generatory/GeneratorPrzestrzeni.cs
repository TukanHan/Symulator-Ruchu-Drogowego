using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public struct Prostokat
    {
        public Punkt<int> Pozycja { get; }
        public int Wysokosc { get; }
        public int Szerokosc { get; }

        public Prostokat(Punkt<int> pozycja, int wysokosc, int szerokosc)
        {
            Pozycja = pozycja;
            Wysokosc = wysokosc;
            Szerokosc = szerokosc;
        }

        public Prostokat(Punkt<int> punktA, Punkt<int> punktB)
        {
            Pozycja = punktA;
            Wysokosc = punktB.Y - punktA.Y + 1;
            Szerokosc = punktB.X - punktA.X + 1;
        }
    }

    public enum TypPrzestrzeni { Nic, Chodnik, Droga, Budynek }

    public class GeneratorPrzestrzeni
    {    
        public List<Prostokat> Budynki { get; private set; } = new List<Prostokat>();
        public List<WierzcholekChodnika> WierzcholkiChodnikow { get; private set; } = new List<WierzcholekChodnika>();
        public List<KrawedzGrafu> Chodniki { get; private set; } = new List<KrawedzGrafu>();
        public TypPrzestrzeni[,] Mapa { get; }

        private int rozmiarMapyX;
        private int rozmiarMapyY;

        public GeneratorPrzestrzeni(int rozmiarMapyX, int rozmiarMapyY, GeneratorPolaczenSamochodow generatorPolaczen)
        {
            this.rozmiarMapyX = rozmiarMapyX*2;
            this.rozmiarMapyY = rozmiarMapyY*2;

            Mapa = new TypPrzestrzeni[this.rozmiarMapyX, this.rozmiarMapyY];

            OdwzorujDroge(generatorPolaczen);
            GenerujChodniki();
            GenerujBudynki();
        }

        /// <summary>
        /// Przenoszenie na mapę drogi z generatoraPołączeń
        /// </summary>
        private void OdwzorujDroge(GeneratorPolaczenSamochodow generatorPolaczen)
        {
            foreach(WierzcholekDrogi wierzcholek in generatorPolaczen.WierzcholkiDrog)
            {
                Punkt<int> punkt = (Punkt<int>)wierzcholek.Pozycja;

                if (wierzcholek.TypWierzcholka != TypWierzcholkaSamochodow.PunktWejscia)
                {
                    Mapa[punkt.X * 2, punkt.Y * 2] = TypPrzestrzeni.Droga;
                    Mapa[punkt.X * 2 + 1, punkt.Y * 2] = TypPrzestrzeni.Droga;
                    Mapa[punkt.X * 2, punkt.Y * 2 + 1] = TypPrzestrzeni.Droga;
                    Mapa[punkt.X * 2 + 1, punkt.Y * 2 + 1] = TypPrzestrzeni.Droga;
                }            
            }
        }

        /// <summary>
        /// Wybór chodników w miejscach optymalnych przez podział przestrzeni
        /// </summary>
        private void GenerujChodniki()
        {
            List<Prostokat> kwadraty = ZbierzWolneKwadraty();

            foreach(Prostokat kwadrat in kwadraty)
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
                            if (GeneratorPoziomu.GeneratorLosowosci.Next(0, 2) == 0)
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
            List<Prostokat> kwadraty = ZbierzWolneKwadraty();

            while (kwadraty.Count>0)
            {
                for(int l=kwadraty.Count-1; l>=0; --l)
                {
                    Prostokat kwadrat = kwadraty[l];

                    if(kwadrat.Wysokosc>=2 && kwadrat.Szerokosc>=2)
                    {
                        bool czyDziele = GeneratorPoziomu.GeneratorLosowosci.Next(0, 2) == 1 ? true : false;
                        if (kwadrat.Wysokosc > 6 || kwadrat.Szerokosc > 6 || kwadrat.Wysokosc * kwadrat.Szerokosc > 16)
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
                                if (GeneratorPoziomu.GeneratorLosowosci.Next(0, 2) == 0)
                                    dzielePionowo = false;
                                else
                                    dzielePoziomo = false;
                            }

                            if(dzielePionowo)
                            {
                                kwadraty.Add(new Prostokat(kwadrat.Pozycja, kwadrat.Wysokosc, kwadrat.Szerokosc - (kwadrat.Szerokosc / 2)));
                                kwadraty.Add(new Prostokat(new Punkt<int>(kwadrat.Pozycja.X + (kwadrat.Szerokosc - (kwadrat.Szerokosc / 2)), kwadrat.Pozycja.Y), kwadrat.Wysokosc, kwadrat.Szerokosc / 2));
                            }
                            else if(dzielePoziomo)
                            {
                                kwadraty.Add(new Prostokat(kwadrat.Pozycja, kwadrat.Wysokosc - (kwadrat.Wysokosc / 2), kwadrat.Szerokosc));
                                kwadraty.Add(new Prostokat(new Punkt<int>(kwadrat.Pozycja.X, kwadrat.Pozycja.Y + (kwadrat.Wysokosc - (kwadrat.Wysokosc /2))), kwadrat.Wysokosc / 2, kwadrat.Szerokosc));
                            }

                            kwadraty.Remove(kwadrat);
                        }
                        else
                        {                           
                            Budynki.Add(kwadrat);
                            kwadraty.Remove(kwadrat);

                            ZaznaczNaMapie<TypPrzestrzeni>(Mapa, kwadrat, TypPrzestrzeni.Budynek);
                        }
                    }
                    else                    
                        kwadraty.Remove(kwadrat);                    
                }
            }     
        }

        private void BudujChodnik(Punkt<double> punktA, Punkt<double> punktB)
        {
            WierzcholekChodnika wierzcholekA;
            if (punktA.X == 0)
                wierzcholekA = new WierzcholekChodnika(new Punkt<double>(punktA.X - 0.5, punktA.Y), TypWierzcholkaPieszych.PunktWejscia);
            else if(punktA.Y == 0)
                wierzcholekA = new WierzcholekChodnika(new Punkt<double>(punktA.X, punktA.Y - 0.5), TypWierzcholkaPieszych.PunktWejscia);
            else
                wierzcholekA = new WierzcholekChodnika(punktA, TypWierzcholkaPieszych.ChodnikPrzestrzeni);

            WierzcholekChodnika wierzcholekB;
            if (punktB.X == rozmiarMapyX -1)
                wierzcholekB = new WierzcholekChodnika(new Punkt<double>(punktB.X + 0.5, punktB.Y), TypWierzcholkaPieszych.PunktWejscia);
            else if (punktB.Y == rozmiarMapyY - 1)
                wierzcholekB = new WierzcholekChodnika(new Punkt<double>(punktB.X, punktB.Y + 0.5), TypWierzcholkaPieszych.PunktWejscia);
            else
                wierzcholekB = new WierzcholekChodnika(punktB, TypWierzcholkaPieszych.ChodnikPrzestrzeni);

            WierzcholkiChodnikow.Add(wierzcholekA);
            WierzcholkiChodnikow.Add(wierzcholekB);
            Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, wierzcholekB));

            ZaznaczNaMapie<TypPrzestrzeni>(Mapa, new Prostokat((Punkt<int>)punktA, (Punkt<int>)punktB), TypPrzestrzeni.Chodnik);
        }

        private List<Prostokat> ZbierzWolneKwadraty()
        {
            List<Prostokat> kwadraty = new List<Prostokat>();

            bool[,] kopiaMapy = new bool[rozmiarMapyX, rozmiarMapyY];
            for (int j = 0; j < rozmiarMapyX; ++j)
                for (int i = 0; i < rozmiarMapyY; ++i)
                    kopiaMapy[j, i] = (Mapa[j, i] == TypPrzestrzeni.Nic ? false : true);

            for (int j = 0; j < rozmiarMapyY; ++j)            
                for (int i = 0; i < rozmiarMapyX; ++i)                
                    if (!kopiaMapy[i, j])                   
                        kwadraty.Add(ZbierzKwadrat(kopiaMapy, new Punkt<int>(i, j)));

            return kwadraty;
        }

        private Prostokat ZbierzKwadrat(bool[,] kopiaMapy, Punkt<int> pozycja)
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


            Prostokat kwadrat = new Prostokat(pozycja, wysokosc - pozycja.Y, szerokosc - pozycja.X);
            ZaznaczNaMapie<bool>(kopiaMapy, kwadrat,true);

            return kwadrat;
        }

        private void ZaznaczNaMapie<T>(T[,] mapa, Prostokat kwadrat, T wartosc)
        {
            for (int j = kwadrat.Pozycja.Y; j < kwadrat.Pozycja.Y + kwadrat.Wysokosc && j < rozmiarMapyY; ++j)
                for (int i = kwadrat.Pozycja.X; i < kwadrat.Pozycja.X + kwadrat.Szerokosc && i < rozmiarMapyX; ++i)
                    mapa[i, j] = wartosc;
        }
    }
}
