﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public class GeneratorPolaczenSamochodow
    {
        public readonly int GRANICA_DOLNA;
        public readonly int GRANICA_GORNA = -1;
        public readonly int GRANICA_PRAWA;
        public readonly int GRANICA_LEWA = -1;

        public List<WierzcholekSamochodow> WierzcholkiDrog { get; private set; } = new List<WierzcholekSamochodow>();
        public List<KrawedzGrafu> Drogi { get; private set; } = new List<KrawedzGrafu>();

        private Random generatorLosowosci = new Random();
        private int rozmiarMapyX;
        private int rozmiarMapyY;
        private int liczbaWejsc;

        public GeneratorPolaczenSamochodow(int rozmiarMapyX, int rozmiarMapyY, int liczbaWejsc)
        {
            GRANICA_PRAWA = this.rozmiarMapyX = rozmiarMapyX;
            GRANICA_DOLNA = this.rozmiarMapyY = rozmiarMapyY;
            this.liczbaWejsc = liczbaWejsc;

            WylosujPunktyWejscia();
            GenerujPoloczeniaKonieczne();
            LaczSkrzyzowania();
            RedukujPolaczenia();
            GenerujPrzejsciaPieszych();

            WyszukiwanieDrogi sprawdzeniePoprawnosciPolaczen = new WyszukiwanieDrogi(WierzcholkiDrog.ConvertAll(o => (WierzcholekGrafu)o));
            if (!sprawdzeniePoprawnosciPolaczen.CzyGrafSpojny())
                throw new Exception("Graf połączeń drogowych nie spójny");
        }

        /// <summary>
        /// Losuje wierzchołki z których będą pojawiać się nowe samochody, punkty wejścia na mapę
        /// </summary>
        private void WylosujPunktyWejscia()
        {
            bool[][] tablicaOznaczenWejsc = new bool[4][]
            {
                new bool[rozmiarMapyX - 4],
                new bool[rozmiarMapyY - 4],
                new bool[rozmiarMapyX - 4],
                new bool[rozmiarMapyY - 4]
            };
            int liczbaWejscDoLosowania = liczbaWejsc;

            while (liczbaWejscDoLosowania > 0)
            {
                int wylosowanyBok = generatorLosowosci.Next(0, 4);
                int rozmiarMapy = wylosowanyBok == 0 || wylosowanyBok == 2 ? rozmiarMapyX : rozmiarMapyY;
                int wylosowanaKomorka = generatorLosowosci.Next(0, rozmiarMapy - 4);               

                if (tablicaOznaczenWejsc[wylosowanyBok][wylosowanaKomorka] == false)
                {
                    tablicaOznaczenWejsc[wylosowanyBok][wylosowanaKomorka] = true;
                    if(wylosowanaKomorka>0)
                        tablicaOznaczenWejsc[wylosowanyBok][wylosowanaKomorka - 1] = true;
                    if (wylosowanaKomorka < rozmiarMapy - 5)
                        tablicaOznaczenWejsc[wylosowanyBok][wylosowanaKomorka + 1] = true;

                    if (wylosowanyBok == 0) // Góra
                        WierzcholkiDrog.Add(new WierzcholekSamochodow(new Punkt<double>(wylosowanaKomorka + 2, GRANICA_GORNA),TypWierzcholkaSamochodow.PunktWejscia));
                    else if(wylosowanyBok == 1) // Prawa
                        WierzcholkiDrog.Add(new WierzcholekSamochodow(new Punkt<double>(GRANICA_PRAWA, wylosowanaKomorka + 2), TypWierzcholkaSamochodow.PunktWejscia));
                    else if (wylosowanyBok == 2) // Dół
                        WierzcholkiDrog.Add(new WierzcholekSamochodow(new Punkt<double>(wylosowanaKomorka + 2, GRANICA_DOLNA), TypWierzcholkaSamochodow.PunktWejscia));
                    else if (wylosowanyBok == 3) // Lewa
                        WierzcholkiDrog.Add(new WierzcholekSamochodow(new Punkt<double>(GRANICA_LEWA, wylosowanaKomorka + 2), TypWierzcholkaSamochodow.PunktWejscia));

                    liczbaWejscDoLosowania--;
                }
            }
        }

        /// <summary>
        /// Wyznacza połączenia które są pewnikiem i są podstawą między innymi połączeniami
        /// </summary>
        private void GenerujPoloczeniaKonieczne()
        {
            Punkt<double> punktGorny =  new Punkt<double>(0, rozmiarMapyY / 2);
            Punkt<double> punktDolny = new Punkt<double>(0, rozmiarMapyY / 2);
            Punkt<double> punktLewy = new Punkt<double>(rozmiarMapyX / 2, 0);
            Punkt<double> punktPrawy = new Punkt<double>(rozmiarMapyX / 2, 0);

            foreach (WierzcholekSamochodow wierzcholek in WierzcholkiDrog)
            {
                if (wierzcholek.Pozycja.Y < punktGorny.Y && wierzcholek.Pozycja.Y != GRANICA_GORNA)
                    punktGorny = wierzcholek.Pozycja;
                if (wierzcholek.Pozycja.Y > punktDolny.Y && wierzcholek.Pozycja.Y != GRANICA_DOLNA)
                    punktDolny = wierzcholek.Pozycja;
                if (wierzcholek.Pozycja.X > punktPrawy.X && wierzcholek.Pozycja.X != GRANICA_PRAWA)
                    punktPrawy = wierzcholek.Pozycja;
                if (wierzcholek.Pozycja.X < punktLewy.X && wierzcholek.Pozycja.X != GRANICA_LEWA)
                    punktLewy = wierzcholek.Pozycja;
            }

            List<WierzcholekSamochodow> punktyWejscia = WierzcholkiDrog.ToList(); 
            foreach (WierzcholekSamochodow wierzcholek in punktyWejscia)
            {
                Punkt<double> punktPrzeciecia = new Punkt<double>(0,0);

                if (wierzcholek.Pozycja.Y == GRANICA_GORNA) // Góra
                    punktPrzeciecia = new Punkt<double>(wierzcholek.Pozycja.X, punktGorny.Y);                
                else if (wierzcholek.Pozycja.X == GRANICA_PRAWA) // Prawa
                    punktPrzeciecia = new Punkt<double>(punktPrawy.X, wierzcholek.Pozycja.Y);
                else if (wierzcholek.Pozycja.Y == GRANICA_DOLNA) // Dół
                    punktPrzeciecia = new Punkt<double>(wierzcholek.Pozycja.X, punktDolny.Y);
                else if (wierzcholek.Pozycja.X == GRANICA_LEWA) // Lewa
                    punktPrzeciecia = new Punkt<double>(punktLewy.X, wierzcholek.Pozycja.Y);

                WierzcholekSamochodow nowyWierzcholek = DodajLubZnajdzWierzcholek(punktPrzeciecia);
                Drogi.Add(KrawedzGrafu.StworzDroge(nowyWierzcholek, wierzcholek));
            }
        }

        /// <summary>
        /// łączy skrzyżowania pewne
        /// </summary>
        private void LaczSkrzyzowania()
        {
            List<WierzcholekSamochodow> skrzyzowania = WierzcholkiDrog.FindAll(o => o.TypWierzcholka == TypWierzcholkaSamochodow.Skrzyzowanie);
            foreach (WierzcholekSamochodow wierzcholek in skrzyzowania)
            {
                WierzcholekSamochodow wlasciwy = null;         
                foreach (WierzcholekSamochodow potencjalny in skrzyzowania)
                {
                    if(wierzcholek != potencjalny && wierzcholek.Krawedzie.Find(o => o.ZwrocPrzeciwnyWierzcholek(wierzcholek) == potencjalny) == null)
                    {
                        if ((wlasciwy == null || Punkt<double>.Odleglosc(wlasciwy.Pozycja, wierzcholek.Pozycja) > Punkt<double>.Odleglosc(potencjalny.Pozycja, wierzcholek.Pozycja)) &&
                            Punkt<double>.ZwrocRelacje(wierzcholek.Pozycja,potencjalny.Pozycja) != Relacja.Brak && 
                            !CzyIstniejeWierzcholekPomiedzy(wierzcholek, potencjalny))
                            wlasciwy = potencjalny;                     
                    }
                }
                if(wlasciwy != null)             
                    Drogi.Add(KrawedzGrafu.StworzDroge(wlasciwy, wierzcholek));                        
            }
        }

        /// <summary>
        /// Wyliczanie przejść dla pieszych
        /// </summary>
        private void GenerujPrzejsciaPieszych()
        {
            List<KrawedzGrafu> drogiTymczasowe = Drogi.ToList();
            foreach(KrawedzGrafu droga in drogiTymczasowe)
            {
                if(droga.DlugoscKrawedzi()>1)
                {
                    Punkt<double> punkt;
                    if(droga.ZwrocRelacje() == Relacja.Pionowe)
                        punkt = new Punkt<double>(droga.WierzcholekA.Pozycja.X, (int)(droga.DlugoscKrawedzi() / 2) + droga.WierzcholekA.Pozycja.Y);
                    else
                        punkt = new Punkt<double>((int)(droga.DlugoscKrawedzi() / 2) + droga.WierzcholekA.Pozycja.X,  droga.WierzcholekA.Pozycja.Y);

                    WierzcholekSamochodow wierzcholekA = (WierzcholekSamochodow)droga.WierzcholekA;
                    WierzcholekSamochodow wierzcholekB = (WierzcholekSamochodow)droga.WierzcholekB;
                    WierzcholekSamochodow dzielacyWierzcholek = new WierzcholekSamochodow(punkt, TypWierzcholkaSamochodow.Pasy);
                    WierzcholkiDrog.Add(dzielacyWierzcholek);

                    Drogi.Remove(droga.UsunKrawedz());
                    Drogi.Add(KrawedzGrafu.StworzDroge(wierzcholekA, dzielacyWierzcholek));
                    Drogi.Add(KrawedzGrafu.StworzDroge(dzielacyWierzcholek, wierzcholekB));
                }
            }
        }

        /// <summary>
        /// Usówa ślepe połączenia, usówa nadmiarowe skrzyżowania, tworzy zakręty
        /// </summary>
        private void RedukujPolaczenia()
        {
            for(int i=0; i< WierzcholkiDrog.Count; ++i)
            {
                WierzcholekSamochodow wierzcholek = WierzcholkiDrog[i];
                if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Skrzyzowanie)
                {
                    if (wierzcholek.Krawedzie.Count == 1)
                    {
                        for(int j=wierzcholek.Krawedzie.Count-1; j>=0; --j)
                            Drogi.Remove(wierzcholek.Krawedzie[j].UsunKrawedz());
                        WierzcholkiDrog.Remove(wierzcholek);
                    }                      
                    else if(wierzcholek.Krawedzie.Count == 2)
                    {
                        if (Punkt<double>.ZwrocRelacje(wierzcholek.Krawedzie[0].ZwrocPrzeciwnyWierzcholek(wierzcholek).Pozycja, wierzcholek.Krawedzie[1].ZwrocPrzeciwnyWierzcholek(wierzcholek).Pozycja) == Relacja.Brak)
                            wierzcholek.TypWierzcholka = TypWierzcholkaSamochodow.Zakret;
                        else
                        {
                            Drogi.Add(KrawedzGrafu.StworzDroge(wierzcholek.Krawedzie[1].ZwrocPrzeciwnyWierzcholek(wierzcholek), wierzcholek.Krawedzie[0].ZwrocPrzeciwnyWierzcholek(wierzcholek)));
                            for (int j = wierzcholek.Krawedzie.Count - 1; j >= 0; --j)
                                Drogi.Remove(wierzcholek.Krawedzie[j].UsunKrawedz() as KrawedzGrafu);
                            WierzcholkiDrog.Remove(wierzcholek);
                        }
                    }
                }
                else if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia)
                {
                    if (wierzcholek.Krawedzie.Count == 0)
                        WierzcholkiDrog.Remove(wierzcholek);
                }
            }
        }

        private bool CzyIstniejeWierzcholekPomiedzy(WierzcholekSamochodow wierzcholekA, WierzcholekSamochodow wierzcholekB)
        {
            Relacja relacja = Punkt<double>.ZwrocRelacje(wierzcholekA.Pozycja, wierzcholekB.Pozycja);

            Punkt<double> punktMniejszy = Punkt<double>.ZwrocPozycjeMniejszego(wierzcholekA.Pozycja, wierzcholekB.Pozycja);
            Punkt<double> punktWiekszy = Punkt<double>.ZwrocPozycjeWiekszego(wierzcholekA.Pozycja, wierzcholekB.Pozycja);

            foreach (WierzcholekSamochodow wierzcholek in WierzcholkiDrog)
            {
                if(relacja == Relacja.Pionowe && wierzcholekA.Pozycja.X == wierzcholek.Pozycja.X)
                {
                    if (wierzcholek.Pozycja.Y > punktMniejszy.Y && wierzcholek.Pozycja.Y < punktWiekszy.Y)
                        return true;                     
                }
                else if(relacja == Relacja.Poziome && wierzcholekA.Pozycja.Y == wierzcholek.Pozycja.Y)
                {
                    if (wierzcholek.Pozycja.X > punktMniejszy.X && wierzcholek.Pozycja.X < punktWiekszy.X)
                        return true;                                          
                }
            }
            return false;
        }

        private WierzcholekSamochodow DodajLubZnajdzWierzcholek(Punkt<double> pozycja)
        {
            WierzcholekSamochodow wierzcholek = WierzcholkiDrog.Find(obiekt => obiekt.Pozycja.Equals(pozycja));
            if (wierzcholek == null)
                WierzcholkiDrog.Add(wierzcholek = new WierzcholekSamochodow(pozycja, TypWierzcholkaSamochodow.Skrzyzowanie));

            return wierzcholek;
        }
    }
}