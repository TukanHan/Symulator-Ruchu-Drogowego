using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public class GeneratorPolaczenPieszych
    {
        public readonly int GRANICA_DOLNA;
        public readonly int GRANICA_GORNA = 0;
        public readonly int GRANICA_PRAWA;
        public readonly int GRANICA_LEWA = 0;

        public List<WierzcholekChodnika> WierzcholkiChodnikow { get; private set; }
        public List<KrawedzGrafu> Chodniki { get; private set; }

        public GeneratorPolaczenPieszych(int rozmiarMapyX, int rozmiarMapyY, GeneratorPolaczenSamochodow generatorPolaczen, GeneratorPrzestrzeni generatorBudynkow)
        {            
            GRANICA_DOLNA = rozmiarMapyY * 2 - 1;
            GRANICA_PRAWA = rozmiarMapyX * 2 - 1;

            WierzcholkiChodnikow = generatorBudynkow.WierzcholkiChodnikow;
            Chodniki = generatorBudynkow.Chodniki;

            OdwzorujChodnikiZDrog(generatorPolaczen);
            LaczChodniki();
            RedukujPolaczenia();
            OznaczPunktyWejscia();

            WyszukiwanieDrogi sprawdzeniePoprawnosciPolaczen = new WyszukiwanieDrogi(WierzcholkiChodnikow.ConvertAll(o => (WierzcholekGrafu)o));
            if (!sprawdzeniePoprawnosciPolaczen.CzyGrafSpojny())
                throw new Exception("Graf połączeń pieszych nie spójny");
        }

        /// <summary>
        /// Odwzorowuje chodniki z połączeń drogowych
        /// </summary>
        private void OdwzorujChodnikiZDrog(GeneratorPolaczenSamochodow generatorPolaczen)
        {
            foreach(WierzcholekDrogi wierzcholek in generatorPolaczen.WierzcholkiDrog)
            {
                if(!wierzcholek.CzyJestDrogaWGore() && !(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia
                    && !wierzcholek.CzyJestDrogaWGore()))
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholek.Pozycja.X * 2 -0.5, wierzcholek.Pozycja.Y * 2 - 0.5),
                                                    new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1.5, wierzcholek.Pozycja.Y * 2 -0.5));
                }
                if (!wierzcholek.CzyJestDrogaWDol() && !(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia
                    && !wierzcholek.CzyJestDrogaWDol()))
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholek.Pozycja.X * 2 -0.5, wierzcholek.Pozycja.Y * 2 + 1.5),
                                                    new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1.5, wierzcholek.Pozycja.Y * 2 + 1.5));               
                }
                if (!wierzcholek.CzyJestDrogaWLewo() && !(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia
                    && !wierzcholek.CzyJestDrogaWLewo()))
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholek.Pozycja.X * 2 -0.5, wierzcholek.Pozycja.Y * 2 - 0.5),
                                                    new Punkt<double>(wierzcholek.Pozycja.X * 2 -0.5, wierzcholek.Pozycja.Y * 2 + 1.5));
                }
                if (!wierzcholek.CzyJestDrogaWPrawo() && !(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia
                    && !wierzcholek.CzyJestDrogaWPrawo()))
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1.5, wierzcholek.Pozycja.Y * 2 - 0.5),
                                                    new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1.5, wierzcholek.Pozycja.Y * 2 + 1.5));
                }

                if (wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Pasy)
                {
                    WierzcholekChodnika wierzcholekA = null;
                    WierzcholekChodnika wierzcholekB = null;
                    if (wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWGore())
                    {
                        wierzcholekA = DodajLubZnajdzWierzcholek(new Punkt<double>(wierzcholek.Pozycja.X * 2 -0.5, wierzcholek.Pozycja.Y * 2 + 0.5));
                        wierzcholekB = DodajLubZnajdzWierzcholek(new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1.5, wierzcholek.Pozycja.Y * 2 + 0.5));
                    }
                    else if (wierzcholek.CzyJestDrogaWPrawo() && wierzcholek.CzyJestDrogaWLewo())
                    {
                        wierzcholekA = DodajLubZnajdzWierzcholek(new Punkt<double>(wierzcholek.Pozycja.X * 2 + 0.5, wierzcholek.Pozycja.Y * 2 -0.5));
                        wierzcholekB = DodajLubZnajdzWierzcholek(new Punkt<double>(wierzcholek.Pozycja.X * 2 + 0.5, wierzcholek.Pozycja.Y * 2 + 1.5));
                    }
                    wierzcholekA.TypWierzcholka = TypWierzcholkaPieszych.Pasy;
                    wierzcholekB.TypWierzcholka = TypWierzcholkaPieszych.Pasy;

                    KrawedzGrafu krawedzA = SzukajDrogiPomiedzyPunktem(wierzcholekA.Pozycja);
                    LaczTraseWWierzcholku(krawedzA, wierzcholekA);

                    KrawedzGrafu krawedzB = SzukajDrogiPomiedzyPunktem(wierzcholekB.Pozycja);
                    LaczTraseWWierzcholku(krawedzB, wierzcholekB);

                    Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, wierzcholekB));
                }
            }
        }

        /// <summary>
        /// Łaczy chodniki generowane w przestrzeni między budynkami
        /// z chodnikami generowanymi z połączeń drogowych
        /// </summary>
        private void LaczChodniki()
        {
            List<KrawedzGrafu> chodnikiPrzestrzeni = Chodniki.Where<KrawedzGrafu>(o => (o.WierzcholekA as WierzcholekChodnika)
                .TypWierzcholka == TypWierzcholkaPieszych.ChodnikPrzestrzeni || (o.WierzcholekA as WierzcholekChodnika)
                .TypWierzcholka == TypWierzcholkaPieszych.PunktWejscia).ToList();

            foreach(KrawedzGrafu chodnikPrzestrzeni in chodnikiPrzestrzeni)
            {
                if (chodnikPrzestrzeni.ZwrocRelacje() == Relacja.Pionowe)
                {
                    if (chodnikPrzestrzeni.WierzcholekA.Pozycja.Y > GRANICA_GORNA)
                    {
                        Punkt<double> punkt = new Punkt<double>(chodnikPrzestrzeni.WierzcholekA.Pozycja.X, chodnikPrzestrzeni.WierzcholekA.Pozycja.Y - 0.5);
                        ProbujLaczycChodnik(punkt, (WierzcholekChodnika)chodnikPrzestrzeni.WierzcholekA);    
                    }
                    if(chodnikPrzestrzeni.WierzcholekB.Pozycja.Y < GRANICA_DOLNA)
                    {
                        Punkt<double> punkt = new Punkt<double>(chodnikPrzestrzeni.WierzcholekB.Pozycja.X, chodnikPrzestrzeni.WierzcholekB.Pozycja.Y + 0.5);
                        ProbujLaczycChodnik(punkt, (WierzcholekChodnika)chodnikPrzestrzeni.WierzcholekB);
                    }                    
                }
                else
                {
                    if (chodnikPrzestrzeni.WierzcholekA.Pozycja.X > GRANICA_LEWA)
                    {
                        Punkt<double> punkt = new Punkt<double>(chodnikPrzestrzeni.WierzcholekA.Pozycja.X - 0.5, chodnikPrzestrzeni.WierzcholekA.Pozycja.Y);
                        ProbujLaczycChodnik(punkt, (WierzcholekChodnika)chodnikPrzestrzeni.WierzcholekA);
                    }
                    if (chodnikPrzestrzeni.WierzcholekB.Pozycja.X  < GRANICA_PRAWA)
                    {
                        Punkt<double> punkt = new Punkt<double>(chodnikPrzestrzeni.WierzcholekB.Pozycja.X + 0.5, chodnikPrzestrzeni.WierzcholekB.Pozycja.Y);
                        ProbujLaczycChodnik(punkt, (WierzcholekChodnika)chodnikPrzestrzeni.WierzcholekB);
                    }
                }
            }
        }

        /// <summary>
        /// Usówa nadmiarowe skrzyżowania
        /// </summary>
        private void RedukujPolaczenia()
        {
            foreach(WierzcholekChodnika wierzcholek in WierzcholkiChodnikow)
            {
                if(wierzcholek.CzyJestDrogaWLewo() && wierzcholek.CzyJestDrogaWPrawo())
                {
                    if(!(wierzcholek.CzyJestDrogaWDol() || wierzcholek.CzyJestDrogaWGore()))
                    {
                        WierzcholekChodnika wierzcholekA = (WierzcholekChodnika)wierzcholek.ZwrocKrawedzLewa().ZwrocPrzeciwnyWierzcholek(wierzcholek);
                        WierzcholekChodnika wierzcholekB = (WierzcholekChodnika)wierzcholek.ZwrocKrawedzPrawa().ZwrocPrzeciwnyWierzcholek(wierzcholek);

                        Chodniki.Remove(wierzcholek.ZwrocKrawedzLewa().UsunKrawedz());
                        Chodniki.Remove(wierzcholek.ZwrocKrawedzPrawa().UsunKrawedz());
                        Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, wierzcholekB));
                    }
                }
                if (wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWGore())
                {
                    if (!(wierzcholek.CzyJestDrogaWLewo() || wierzcholek.CzyJestDrogaWPrawo()))
                    {
                        WierzcholekChodnika wierzcholekA = (WierzcholekChodnika)wierzcholek.ZwrocKrawedzGorna().ZwrocPrzeciwnyWierzcholek(wierzcholek);
                        WierzcholekChodnika wierzcholekB = (WierzcholekChodnika)wierzcholek.ZwrocKrawedzDolna().ZwrocPrzeciwnyWierzcholek(wierzcholek);

                        Chodniki.Remove(wierzcholek.ZwrocKrawedzGorna().UsunKrawedz());
                        Chodniki.Remove(wierzcholek.ZwrocKrawedzDolna().UsunKrawedz());
                        Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, wierzcholekB));
                    }
                }
            }

            for(int i= WierzcholkiChodnikow.Count-1; i>=0; --i)
            {
                if (WierzcholkiChodnikow[i].Krawedzie.Count == 0)
                    WierzcholkiChodnikow.Remove(WierzcholkiChodnikow[i]);
            }
        }

        /// <summary>
        /// Punkty poza widoczną przestrzenią będą punktami wejścia
        /// </summary>
        private void OznaczPunktyWejscia()
        {
            foreach(WierzcholekChodnika wierzcholek in WierzcholkiChodnikow)
            {
                if (wierzcholek.Pozycja.X < GRANICA_LEWA || wierzcholek.Pozycja.X > GRANICA_PRAWA ||
                    wierzcholek.Pozycja.Y > GRANICA_DOLNA || wierzcholek.Pozycja.Y < GRANICA_GORNA)
                    wierzcholek.TypWierzcholka = TypWierzcholkaPieszych.PunktWejscia;
            }
        }

        private void TworzPolaczeniaKopiowanychTras(Punkt<double> punktA, Punkt<double> punktB)
        {
            WierzcholekChodnika wierzcholekA = DodajLubZnajdzWierzcholek(punktA);
            WierzcholekChodnika wierzcholekB = DodajLubZnajdzWierzcholek(punktB);
            Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, wierzcholekB));
        }

        private void ProbujLaczycChodnik(Punkt<double> punkt, WierzcholekChodnika wierzcholek)
        {
            WierzcholekChodnika szukanyWierzcholek = WierzcholkiChodnikow.Find(o => o.Pozycja.Equals(punkt));
            if (szukanyWierzcholek != null)
                Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholek, szukanyWierzcholek));
            else
            {
                KrawedzGrafu szukanaKrawedz = SzukajDrogiPomiedzyPunktem(punkt);
                if (szukanaKrawedz != null)
                {
                    szukanyWierzcholek = DzielTraseWPunkcie(szukanaKrawedz, punkt);
                    Chodniki.Add(KrawedzGrafu.StworzDroge(szukanyWierzcholek, wierzcholek));
                }
            }
        }

        private WierzcholekChodnika DzielTraseWPunkcie(KrawedzGrafu trasa, Punkt<double> punkt)
        {
            WierzcholekChodnika wierzcholekA = (WierzcholekChodnika)trasa.WierzcholekA;
            WierzcholekChodnika wierzcholekB = (WierzcholekChodnika)trasa.WierzcholekB;
            WierzcholekChodnika dzielacyWierzcholek = new WierzcholekChodnika(punkt, TypWierzcholkaPieszych.ChodnikDrogi);
            WierzcholkiChodnikow.Add(dzielacyWierzcholek);

            Chodniki.Remove(trasa.UsunKrawedz());
            Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, dzielacyWierzcholek));
            Chodniki.Add(KrawedzGrafu.StworzDroge(dzielacyWierzcholek, wierzcholekB));

            return dzielacyWierzcholek;
        }

        private void LaczTraseWWierzcholku(KrawedzGrafu trasa, WierzcholekChodnika laczacyWierzcholek)
        {
            WierzcholekChodnika wierzcholekA = (WierzcholekChodnika)trasa.WierzcholekA;
            WierzcholekChodnika wierzcholekB = (WierzcholekChodnika)trasa.WierzcholekB;

            Chodniki.Remove(trasa.UsunKrawedz());
            Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, laczacyWierzcholek));
            Chodniki.Add(KrawedzGrafu.StworzDroge(laczacyWierzcholek, wierzcholekB));
        }

        private KrawedzGrafu SzukajDrogiPomiedzyPunktem(Punkt<double> pozycja)
        {
            foreach(KrawedzGrafu krawedz in Chodniki)
            {
                    if (krawedz.ZwrocRelacje() == Relacja.Pionowe && pozycja.X == krawedz.WierzcholekA.Pozycja.X && 
                        krawedz.WierzcholekA.Pozycja.Y < pozycja.Y && krawedz.WierzcholekB.Pozycja.Y > pozycja.Y)
                        return krawedz;
                    else if (pozycja.Y == krawedz.WierzcholekA.Pozycja.Y && krawedz.WierzcholekA.Pozycja.X < pozycja.X &&
                        krawedz.WierzcholekB.Pozycja.X > pozycja.X)
                        return krawedz;
            }
            return null;
        }

        private WierzcholekChodnika DodajLubZnajdzWierzcholek(Punkt<double> pozycja)
        {
            WierzcholekChodnika wierzcholek = WierzcholkiChodnikow.Find(obiekt => obiekt.Pozycja.Equals(pozycja));
            if (wierzcholek == null)
                WierzcholkiChodnikow.Add(wierzcholek = new WierzcholekChodnika(pozycja, TypWierzcholkaPieszych.ChodnikDrogi));

            return wierzcholek;
        }
    }
}