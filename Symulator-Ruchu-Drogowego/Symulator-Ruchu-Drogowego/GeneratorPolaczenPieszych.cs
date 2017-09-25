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

        public List<WierzcholekPieszych> WierzcholkiChodnikow { get; private set; }
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

            WyszukiwanieDrogi sprawdzeniePoprawnosciPolaczen = new WyszukiwanieDrogi(WierzcholkiChodnikow.ConvertAll(o => (WierzcholekGrafu)o));
            if (!sprawdzeniePoprawnosciPolaczen.CzyGrafSpojny())
                throw new Exception("Graf połączeń pieszych nie spójny");
        }

        /// <summary>
        /// Odwzorowuje chodniki z połączeń drogowych
        /// </summary>
        /// <param name="generatorPolaczen">Źródło połączeń drogowych</param>
        private void OdwzorujChodnikiZDrog(GeneratorPolaczenSamochodow generatorPolaczen)
        {
            foreach(KrawedzGrafu droga in generatorPolaczen.Drogi)
            {
                Punkt<double> wierzcholekApoz = droga.WierzcholekA.Pozycja;
                Punkt<double> wierzcholekBpoz = droga.WierzcholekB.Pozycja;

                if (droga.ZwrocRelacje() == Relacja.Pionowe)
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholekApoz.X * 2, wierzcholekApoz.Y * 2 + 1),
                                                    new Punkt<double>(wierzcholekBpoz.X * 2, wierzcholekBpoz.Y * 2));
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholekApoz.X * 2 + 1, wierzcholekApoz.Y * 2 + 1),
                                                    new Punkt<double>(wierzcholekBpoz.X * 2 + 1, wierzcholekBpoz.Y * 2));
                }
                else
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholekApoz.X * 2 + 1, wierzcholekApoz.Y * 2),
                                                    new Punkt<double>(wierzcholekBpoz.X * 2, wierzcholekBpoz.Y * 2));
                    TworzPolaczeniaKopiowanychTras(new Punkt<double>(wierzcholekApoz.X * 2 + 1, wierzcholekApoz.Y * 2 + 1),
                                                    new Punkt<double>(wierzcholekBpoz.X * 2, wierzcholekBpoz.Y * 2 + 1));
                }     
            }
            foreach(WierzcholekSamochodow wierzcholek in generatorPolaczen.WierzcholkiDrog)
            {
                if(!wierzcholek.CzyJestDrogaWGore() && !(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia
                    && !wierzcholek.CzyJestDrogaWGore()))
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholek.Pozycja.X * 2, wierzcholek.Pozycja.Y * 2),
                                                    new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1, wierzcholek.Pozycja.Y * 2));
                }
                if (!wierzcholek.CzyJestDrogaWDol() && !(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia
                    && !wierzcholek.CzyJestDrogaWDol()))
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholek.Pozycja.X * 2, wierzcholek.Pozycja.Y * 2 + 1),
                                                    new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1, wierzcholek.Pozycja.Y * 2 + 1));               
                }
                if (!wierzcholek.CzyJestDrogaWLewo() && !(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia
                    && !wierzcholek.CzyJestDrogaWLewo()))
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholek.Pozycja.X * 2, wierzcholek.Pozycja.Y * 2),
                                                    new Punkt<double>(wierzcholek.Pozycja.X * 2, wierzcholek.Pozycja.Y * 2 + 1));
                }
                if (!wierzcholek.CzyJestDrogaWPrawo() && !(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia
                    && !wierzcholek.CzyJestDrogaWPrawo()))
                {
                    TworzPolaczeniaKopiowanychTras( new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1, wierzcholek.Pozycja.Y * 2),
                                                    new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1, wierzcholek.Pozycja.Y * 2 + 1));
                }

                if (wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Pasy)
                {
                    WierzcholekPieszych wierzcholekA = null;
                    WierzcholekPieszych wierzcholekB = null;
                    if (wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWGore())
                    {
                        wierzcholekA = DodajLubZnajdzWierzcholek(new Punkt<double>(wierzcholek.Pozycja.X * 2, wierzcholek.Pozycja.Y * 2 + 0.5));
                        wierzcholekB = DodajLubZnajdzWierzcholek(new Punkt<double>(wierzcholek.Pozycja.X * 2 + 1, wierzcholek.Pozycja.Y * 2 + 0.5));
                    }
                    else if (wierzcholek.CzyJestDrogaWPrawo() && wierzcholek.CzyJestDrogaWLewo())
                    {
                        wierzcholekA = DodajLubZnajdzWierzcholek(new Punkt<double>(wierzcholek.Pozycja.X * 2 + 0.5, wierzcholek.Pozycja.Y * 2));
                        wierzcholekB = DodajLubZnajdzWierzcholek(new Punkt<double>(wierzcholek.Pozycja.X * 2 + 0.5, wierzcholek.Pozycja.Y * 2 + 1));
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
            List<KrawedzGrafu> chodnikiPrzestrzeni = Chodniki.Where<KrawedzGrafu>(o => (o.WierzcholekA as WierzcholekPieszych)
                .TypWierzcholka == TypWierzcholkaPieszych.ChodnikPrzestrzeni || (o.WierzcholekA as WierzcholekPieszych)
                .TypWierzcholka == TypWierzcholkaPieszych.PunktWejscia).ToList();

            foreach(KrawedzGrafu chodnikPrzestrzeni in chodnikiPrzestrzeni)
            {
                if (chodnikPrzestrzeni.ZwrocRelacje() == Relacja.Pionowe)
                {
                    if (chodnikPrzestrzeni.WierzcholekA.Pozycja.Y > GRANICA_GORNA)
                    {
                        Punkt<double> punkt = new Punkt<double>(chodnikPrzestrzeni.WierzcholekA.Pozycja.X, chodnikPrzestrzeni.WierzcholekA.Pozycja.Y - 1);
                        ProbujLaczycChodnik(punkt, (WierzcholekPieszych)chodnikPrzestrzeni.WierzcholekA);    
                    }
                    if(chodnikPrzestrzeni.WierzcholekB.Pozycja.Y < GRANICA_DOLNA)
                    {
                        Punkt<double> punkt = new Punkt<double>(chodnikPrzestrzeni.WierzcholekB.Pozycja.X, chodnikPrzestrzeni.WierzcholekB.Pozycja.Y + 1);
                        ProbujLaczycChodnik(punkt, (WierzcholekPieszych)chodnikPrzestrzeni.WierzcholekB);
                    }                    
                }
                else
                {
                    if (chodnikPrzestrzeni.WierzcholekA.Pozycja.X > GRANICA_LEWA)
                    {
                        Punkt<double> punkt = new Punkt<double>(chodnikPrzestrzeni.WierzcholekA.Pozycja.X - 1, chodnikPrzestrzeni.WierzcholekA.Pozycja.Y);
                        ProbujLaczycChodnik(punkt, (WierzcholekPieszych)chodnikPrzestrzeni.WierzcholekA);
                    }
                    if (chodnikPrzestrzeni.WierzcholekB.Pozycja.X  < GRANICA_PRAWA)
                    {
                        Punkt<double> punkt = new Punkt<double>(chodnikPrzestrzeni.WierzcholekB.Pozycja.X + 1, chodnikPrzestrzeni.WierzcholekB.Pozycja.Y);
                        ProbujLaczycChodnik(punkt, (WierzcholekPieszych)chodnikPrzestrzeni.WierzcholekB);
                    }
                }
            }
        }

        /// <summary>
        /// Usówa nadmiarowe skrzyżowania
        /// </summary>
        private void RedukujPolaczenia()
        {
            foreach(WierzcholekPieszych wierzcholek in WierzcholkiChodnikow)
            {
                if(wierzcholek.CzyJestDrogaWLewo() && wierzcholek.CzyJestDrogaWPrawo())
                {
                    if(!(wierzcholek.CzyJestDrogaWDol() || wierzcholek.CzyJestDrogaWGore()))
                    {
                        WierzcholekPieszych wierzcholekA = (WierzcholekPieszych)wierzcholek.ZwrocKrawedzLewa().ZwrocPrzeciwnyWierzcholek(wierzcholek);
                        WierzcholekPieszych wierzcholekB = (WierzcholekPieszych)wierzcholek.ZwrocKrawedzPrawa().ZwrocPrzeciwnyWierzcholek(wierzcholek);

                        Chodniki.Remove(wierzcholek.ZwrocKrawedzLewa().UsunKrawedz());
                        Chodniki.Remove(wierzcholek.ZwrocKrawedzPrawa().UsunKrawedz());
                        Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, wierzcholekB));
                    }
                }
                if (wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWGore())
                {
                    if (!(wierzcholek.CzyJestDrogaWLewo() || wierzcholek.CzyJestDrogaWPrawo()))
                    {
                        WierzcholekPieszych wierzcholekA = (WierzcholekPieszych)wierzcholek.ZwrocKrawedzGorna().ZwrocPrzeciwnyWierzcholek(wierzcholek);
                        WierzcholekPieszych wierzcholekB = (WierzcholekPieszych)wierzcholek.ZwrocKrawedzDolna().ZwrocPrzeciwnyWierzcholek(wierzcholek);

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

        private void TworzPolaczeniaKopiowanychTras(Punkt<double> punktA, Punkt<double> punktB)
        {
            WierzcholekPieszych wierzcholekA = DodajLubZnajdzWierzcholek(punktA);
            WierzcholekPieszych wierzcholekB = DodajLubZnajdzWierzcholek(punktB);
            Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, wierzcholekB));
        }

        private void ProbujLaczycChodnik(Punkt<double> punkt, WierzcholekPieszych wierzcholek)
        {
            WierzcholekPieszych szukanyWierzcholek = WierzcholkiChodnikow.Find(o => o.Pozycja.Equals(punkt));
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

        private WierzcholekPieszych DzielTraseWPunkcie(KrawedzGrafu trasa, Punkt<double> punkt)
        {
            WierzcholekPieszych wierzcholekA = (WierzcholekPieszych)trasa.WierzcholekA;
            WierzcholekPieszych wierzcholekB = (WierzcholekPieszych)trasa.WierzcholekB;
            WierzcholekPieszych dzielacyWierzcholek = new WierzcholekPieszych(punkt, TypWierzcholkaPieszych.ChodnikDrogi);
            WierzcholkiChodnikow.Add(dzielacyWierzcholek);

            Chodniki.Remove(trasa.UsunKrawedz());
            Chodniki.Add(KrawedzGrafu.StworzDroge(wierzcholekA, dzielacyWierzcholek));
            Chodniki.Add(KrawedzGrafu.StworzDroge(dzielacyWierzcholek, wierzcholekB));

            return dzielacyWierzcholek;
        }

        private void LaczTraseWWierzcholku(KrawedzGrafu trasa, WierzcholekPieszych laczacyWierzcholek)
        {
            WierzcholekPieszych wierzcholekA = (WierzcholekPieszych)trasa.WierzcholekA;
            WierzcholekPieszych wierzcholekB = (WierzcholekPieszych)trasa.WierzcholekB;

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

        private WierzcholekPieszych DodajLubZnajdzWierzcholek(Punkt<double> pozycja)
        {
            WierzcholekPieszych wierzcholek = WierzcholkiChodnikow.Find(obiekt => obiekt.Pozycja.Equals(pozycja));
            if (wierzcholek == null)
                WierzcholkiChodnikow.Add(wierzcholek = new WierzcholekPieszych(pozycja, TypWierzcholkaPieszych.ChodnikDrogi));

            return wierzcholek;
        }
    }
}