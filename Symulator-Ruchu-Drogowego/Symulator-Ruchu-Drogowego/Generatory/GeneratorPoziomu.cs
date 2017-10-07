using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Symulator_Ruchu_Drogowego
{
    public class GeneratorPoziomu
    {
        public static Random GeneratorLosowosci { get; } = new Random();

        public List<WierzcholekDrogi> WierzcholkiDrog { get { return generatorPolaczen.WierzcholkiDrog; } }
        public List<WierzcholekChodnika> WierzcholkiChodnikow { get { return generatorPolaczenPieszych.WierzcholkiChodnikow; } }

        private GeneratorPolaczenSamochodow generatorPolaczen;
        private GeneratorPolaczenPieszych generatorPolaczenPieszych;

        public GeneratorPoziomu(Canvas rodzicObrazkow, int szerokosc, int wysokosc, int liczbaWejsc)
        {
            generatorPolaczen = new GeneratorPolaczenSamochodow(szerokosc, wysokosc, liczbaWejsc);
            GeneratorZnakowPoziomych generatorPasow = new GeneratorZnakowPoziomych(generatorPolaczen);
            GeneratorPrzestrzeni generatorBudynkow = new GeneratorPrzestrzeni(szerokosc, wysokosc, generatorPolaczen);
            generatorPolaczenPieszych = new GeneratorPolaczenPieszych(szerokosc, wysokosc, generatorPolaczen, generatorBudynkow);

            RysujDrogi();
            RysujBudynki(generatorBudynkow);
            RysujMape(generatorBudynkow);
            //RysujKonturChodnika();
            RysujPasy(generatorPasow);
        }

        private void RysujDrogi()
        {
            foreach(WierzcholekDrogi wierzcholek in generatorPolaczen.WierzcholkiDrog)
            {
                if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Pasy || wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Droga)
                {
                    if(wierzcholek.CzyJestDrogaWDol() || wierzcholek.CzyJestDrogaWGore())
                        Tworz4Obrazki(wierzcholek.Pozycja, @"Drogi\DrogaPionowoLewa.png", @"Drogi\DrogaPionowoPrawa.png", @"Drogi\DrogaPionowoLewa.png", @"Drogi\DrogaPionowoPrawa.png");
                    else
                        Tworz4Obrazki(wierzcholek.Pozycja, @"Drogi\DrogaPoziomoLewa.png", @"Drogi\DrogaPoziomoLewa.png", @"Drogi\DrogaPoziomoPrawa.png", @"Drogi\DrogaPoziomoPrawa.png");
                }
                else if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Skrzyzowanie)
                {
                    string obrazek1, obrazek2, obrazek3, obrazek4;

                    if (wierzcholek.CzyJestDrogaWGore())
                    {
                        obrazek2 = wierzcholek.CzyJestDrogaWPrawo() ? @"Skrzyzowania\SkrzyzowaniePrawyGorny.png" : @"Drogi\DrogaPionowoPrawa.png";
                        obrazek1 = wierzcholek.CzyJestDrogaWLewo() ? @"Skrzyzowania\SkrzyzowanieLewyGorny.png" : @"Drogi\DrogaPionowoLewa.png";
                    }
                    else                    
                        obrazek1 = obrazek2 = @"Drogi\DrogaPoziomoLewa.png";

                    if (wierzcholek.CzyJestDrogaWDol())
                    {
                        obrazek4 = wierzcholek.CzyJestDrogaWPrawo() ? @"Skrzyzowania\SkrzyzowaniePrawyDolny.png" : @"Drogi\DrogaPionowoPrawa.png";
                        obrazek3 = wierzcholek.CzyJestDrogaWLewo() ? @"Skrzyzowania\SkrzyzowanieLewyDolny.png" : @"Drogi\DrogaPionowoLewa.png";
                    }
                    else
                        obrazek4 = obrazek3 = @"Drogi\DrogaPoziomoPrawa.png";

                    Tworz4Obrazki(wierzcholek.Pozycja, obrazek1, obrazek2, obrazek3, obrazek4);
                }
                else if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Zakret)
                {
                    if(wierzcholek.CzyJestDrogaWGore() && wierzcholek.CzyJestDrogaWPrawo())
                        Tworz4Obrazki(wierzcholek.Pozycja, @"Drogi\DrogaPionowoLewa.png", @"Zakret\ZakretWewnetrznyGoraPrawo.png", @"Zakret\ZakretZewnetrznyDolLewo.png", @"Drogi\DrogaPoziomoPrawa.png");
                    else if (wierzcholek.CzyJestDrogaWGore() && wierzcholek.CzyJestDrogaWLewo())
                        Tworz4Obrazki(wierzcholek.Pozycja, @"Zakret\ZakretWewnetrznyGoraLewo.png", @"Drogi\DrogaPionowoPrawa.png", @"Drogi\DrogaPoziomoPrawa.png", @"Zakret\ZakretZewnetrznyDolPrawo.png");
                    else if (wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWLewo())
                        Tworz4Obrazki(wierzcholek.Pozycja, @"Drogi\DrogaPoziomoLewa.png", @"Zakret\ZakretZewnetrznyGoraPrawo.png", @"Zakret\ZakretWewnetrznyDolLewo.png", @"Drogi\DrogaPionowoPrawa.png");
                    else if (wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWPrawo())
                        Tworz4Obrazki(wierzcholek.Pozycja, @"Zakret\ZakretZewnetrznyGoraLewo.png", @"Drogi\DrogaPoziomoLewa.png", @"Drogi\DrogaPionowoLewa.png", @"Zakret\ZakretWewnetrznyDolPrawo.png");
                }              
            }
        }

        private void RysujBudynki(GeneratorPrzestrzeni generatorBudynkow)
        {
            foreach (Prostokat budynek in generatorBudynkow.Budynki)
            {
                string folder = $"Budynek{GeneratorLosowosci.Next(1, 6)}";

                for (int i = 0; i < budynek.Szerokosc; ++i)
                {
                    for (int j = 0; j < budynek.Wysokosc; ++j)
                    {
                        Image obrazek = null;

                        if (i == 0 && j == 0) obrazek = TworzObrazek($@"Budynki\{folder}\LewaGora.png");
                        else if (i == 0 && j + 1 == budynek.Wysokosc) obrazek = TworzObrazek($@"Budynki\{folder}\LewyDol.png");
                        else if (i == 0) obrazek = TworzObrazek($@"Budynki\{folder}\LewySrodek.png");

                        else if (i + 1 == budynek.Szerokosc && j == 0) obrazek = TworzObrazek($@"Budynki\{folder}\PrawaGora.png");
                        else if (i + 1 == budynek.Szerokosc && j + 1 == budynek.Wysokosc) obrazek = TworzObrazek($@"Budynki\{folder}\PrawyDol.png");
                        else if (i + 1 == budynek.Szerokosc) obrazek = TworzObrazek($@"Budynki\{folder}\PrawySrodek.png");

                        else if (j == 0) obrazek = TworzObrazek($@"Budynki\{folder}\SrodkowaGora.png");
                        else if (j + 1 == budynek.Wysokosc) obrazek = TworzObrazek($@"Budynki\{folder}\SrodkowyDol.png");
                        else obrazek = TworzObrazek($@"Budynki\{folder}\SrodkowySrodek.png");

                        UstawPozycjeObiektu(obrazek, new Punkt<double>((budynek.Pozycja.X + i) * 40, (budynek.Pozycja.Y + j) * 40));
                    }
                }
            }
        }

        private void RysujMape(GeneratorPrzestrzeni generatorBudynkow)
        {
            for(int i=0; i< generatorBudynkow.Mapa.GetLength(0); ++i)
            {
                for (int j = 0; j < generatorBudynkow.Mapa.GetLength(1); ++j)
                {
                    if (generatorBudynkow.Mapa[i, j] == TypPrzestrzeni.Chodnik || generatorBudynkow.Mapa[i, j] == TypPrzestrzeni.Droga)
                        RysujChodnik(new Punkt<double>(i, j));
                    else if (generatorBudynkow.Mapa[i, j] == TypPrzestrzeni.Nic)
                        RysujOzdobe(new Punkt<double>(i, j));
                }
            }
        }

        private void RysujOzdobe(Punkt<double> punkt)
        {
            int rand = GeneratorLosowosci.Next(0, 14);
            if (rand == 0)
                TworzIUstawObrazekOzdoby(@"Ozdoby/Lampa.png", punkt);
            else if (rand == 1)
                TworzIUstawObrazekOzdoby(@"Ozdoby/Lawka.png", punkt);
            else if (rand == 2)
                TworzIUstawObrazekOzdoby(@"Ozdoby/Poczta.png", punkt);
            else if (rand == 3)
                TworzIUstawObrazekOzdoby(@"Ozdoby/Smietnik.png", punkt);
            else if (rand == 4)
                TworzIUstawObrazekOzdoby(@"Ozdoby/Hydrant.png", punkt);
            else if (rand >= 5 && rand <= 7)
                TworzIUstawObrazekOzdoby(@"Ozdoby/Drzewo1.png", punkt);
            else if (rand >= 8 && rand <= 10)
                TworzIUstawObrazekOzdoby(@"Ozdoby/Drzewo2.png", punkt);
            else if (rand >= 11 && rand <= 13)
                TworzIUstawObrazekOzdoby(@"Ozdoby/Drzewo3.png", punkt);
        }

        private void RysujChodnik(Punkt<double> punkt)
        {
            Image obrazek = TworzObrazek(@"Inne/Chodnik.png", 0);
            UstawPozycjeObiektu(obrazek, new Punkt<double>(punkt.X * 40, punkt.Y * 40));
        }

        private void RysujKonturChodnika()
        {
            foreach (KrawedzGrafu krawedz in generatorPolaczenPieszych.Chodniki)
            {
                System.Windows.Shapes.Polyline yellowPolyline = new System.Windows.Shapes.Polyline()
                {
                    Stroke = System.Windows.Media.Brushes.Black,
                    StrokeThickness = 4
                };

                int r = GeneratorLosowosci.Next(0, 10);
                if (r == 0) yellowPolyline.Stroke = System.Windows.Media.Brushes.Black;
                if (r == 1) yellowPolyline.Stroke = System.Windows.Media.Brushes.Aqua;
                if (r == 2) yellowPolyline.Stroke = System.Windows.Media.Brushes.Red;
                if (r == 3) yellowPolyline.Stroke = System.Windows.Media.Brushes.Green;
                if (r == 4) yellowPolyline.Stroke = System.Windows.Media.Brushes.Honeydew;
                if (r == 5) yellowPolyline.Stroke = System.Windows.Media.Brushes.Indigo;
                if (r == 6) yellowPolyline.Stroke = System.Windows.Media.Brushes.LightSkyBlue;
                if (r == 7) yellowPolyline.Stroke = System.Windows.Media.Brushes.Khaki;
                if (r == 8) yellowPolyline.Stroke = System.Windows.Media.Brushes.PaleVioletRed;

                System.Windows.Media.PointCollection polygonPoints = new System.Windows.Media.PointCollection();
                polygonPoints.Add(new System.Windows.Point(krawedz.WierzcholekA.Pozycja.X * 40 + 20, krawedz.WierzcholekA.Pozycja.Y * 40 + 20));
                polygonPoints.Add(new System.Windows.Point(krawedz.WierzcholekB.Pozycja.X * 40 + 20, krawedz.WierzcholekB.Pozycja.Y * 40 + 20));
                yellowPolyline.Points = polygonPoints;

                Canvas.SetZIndex(yellowPolyline, 4);
                Symulacja.Warstwa.Children.Add(yellowPolyline);
            }

            foreach(WierzcholekChodnika wierzcholek in generatorPolaczenPieszych.WierzcholkiChodnikow)
            {
                TextBlock yellowPolyline = new TextBlock()
                {
                    FontSize = 8,
                    Text = wierzcholek.Krawedzie.Count.ToString()
                };
                Canvas.SetLeft(yellowPolyline, wierzcholek.Pozycja.X * 40 + 20);
                Canvas.SetTop(yellowPolyline, wierzcholek.Pozycja.Y * 40 +20);

                Canvas.SetZIndex(yellowPolyline, 5);
                Symulacja.Warstwa.Children.Add(yellowPolyline);
            }
        }

        private void RysujPasy(GeneratorZnakowPoziomych generatorPasow)
        {
            foreach(ZnakPoziomy pasy in generatorPasow.ZnakiPoziome)
            {
                Image obrazek = null;
                if (pasy.TypPasow == TypPasow.PrzejsciePieszychPionowe)
                    obrazek = TworzObrazekPasow(@"Pasy\PasyPionowo.png", 80, 80);
                else if(pasy.TypPasow == TypPasow.PrzejsciePieszychPoziome)
                    obrazek = TworzObrazekPasow(@"Pasy\PasyPoziomo.png", 80, 80);
                else if(pasy.TypPasow == TypPasow.LiniaPrzerywanaPionowa)
                    obrazek = TworzObrazekPasow(@"Pasy\LiniaPrzerywanaPionowo.png", 4, 80);
                else if (pasy.TypPasow == TypPasow.LiniaPrzerywanaPozioma)
                    obrazek = TworzObrazekPasow(@"Pasy\LiniaPrzerywanaPoziomo.png", 80, 4);
                else if(pasy.TypPasow == TypPasow.ZakretDolPrawo)
                    obrazek = TworzObrazekPasow(@"Pasy\PasyZakretDolPrawo.png", 80, 80);
                else if (pasy.TypPasow == TypPasow.ZakretDolLewo)
                    obrazek = TworzObrazekPasow(@"Pasy\PasyZakretDolLewo.png", 80, 80);
                else if (pasy.TypPasow == TypPasow.ZakretGoraPrawo)
                    obrazek = TworzObrazekPasow(@"Pasy\PasyZakretGoraPrawo.png", 80, 80);
                else if (pasy.TypPasow == TypPasow.ZakretGoraLewo)
                    obrazek = TworzObrazekPasow(@"Pasy\PasyZakretGoraLewo.png", 80, 80);

                UstawPozycjeObiektu(obrazek, pasy.Pozycja);
            }
        }
        
        private void Tworz4Obrazki(Punkt<double> punkt, string lokalizacjaPlku1, string lokalizacjaPlku2, string lokalizacjaPlku3, string lokalizacjaPlku4)
        {
            Image obrazek;

            obrazek = TworzObrazek(lokalizacjaPlku1);
            UstawPozycjeObiektu(obrazek, new Punkt<double>(punkt.X * 2 * obrazek.Height, punkt.Y * 2 * obrazek.Width));

            obrazek = TworzObrazek(lokalizacjaPlku2);
            UstawPozycjeObiektu(obrazek, new Punkt<double>(punkt.X * 2 * obrazek.Height + obrazek.Height, punkt.Y * 2 * obrazek.Width));

            obrazek = TworzObrazek(lokalizacjaPlku3);
            UstawPozycjeObiektu(obrazek, new Punkt<double>(punkt.X * 2 * obrazek.Height, punkt.Y * 2 * obrazek.Width + obrazek.Width));

            obrazek = TworzObrazek(lokalizacjaPlku4);
            UstawPozycjeObiektu(obrazek, new Punkt<double>(punkt.X * 2 * obrazek.Height + obrazek.Height, punkt.Y * 2 * obrazek.Width + obrazek.Width));
        }

        private void UstawPozycjeObiektu(Image obrazek, Punkt<double> punkt)
        {
            Canvas.SetLeft(obrazek, punkt.X);
            Canvas.SetTop(obrazek, punkt.Y);
        }

        private Image TworzObrazek(string lokalizacjaPliku, int zIndex = 1)
        {
            Image obrazek = new Image()
            {
                Height = 40,
                Width = 40,
                Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/{lokalizacjaPliku}", UriKind.Absolute)),
            };

            Canvas.SetZIndex(obrazek, zIndex);
            Symulacja.Warstwa.Children.Add(obrazek);

            return obrazek;
        }

        private Image TworzObrazekPasow(string lokalizacjaPliku, int szerokosc, int wysokosc)
        {
            Image obrazek = TworzObrazek(lokalizacjaPliku, 2);
            obrazek.Width = szerokosc;
            obrazek.Height = wysokosc;

            return obrazek;
        }

        private void TworzIUstawObrazekOzdoby(string lokalizacjaPliku, Punkt<double> pozycja)
        {
            Image obrazek = TworzObrazek(lokalizacjaPliku, 2);
            obrazek.Height = obrazek.Source.Height / 32 * 30;
            obrazek.Width = obrazek.Source.Width / 32 * 30;

            UstawPozycjeObiektu(obrazek, new Punkt<double>(pozycja.X * 40 + ( 40 - obrazek.Width)/2, pozycja.Y * 40 + (35 - obrazek.Height)));
        }
    }
}