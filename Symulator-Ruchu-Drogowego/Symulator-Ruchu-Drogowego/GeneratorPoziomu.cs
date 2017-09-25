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
        private Canvas rodzicObrazkow;
        private Random generatorLosowosci = new Random();
        private GeneratorPolaczenSamochodow generatorPolaczen;
        private GeneratorPrzestrzeni generatorBudynkow;
        private GeneratorPolaczenPieszych generatorPolaczenPieszych;

        public GeneratorPoziomu(Canvas rodzicObrazkow, int szerokosc, int wysokosc, int liczbaWejsc)
        {
            this.rodzicObrazkow = rodzicObrazkow;

            generatorPolaczen = new GeneratorPolaczenSamochodow(szerokosc, wysokosc, liczbaWejsc);
            generatorBudynkow = new GeneratorPrzestrzeni(szerokosc, wysokosc, generatorPolaczen);
            generatorPolaczenPieszych = new GeneratorPolaczenPieszych(szerokosc, wysokosc, generatorPolaczen, generatorBudynkow);

            RysujDrogi();
            RysujBudynki();
            RysujChodniki();
            RysujPasy();
        }

        private void RysujDrogi()
        {
            foreach(WierzcholekSamochodow wierzcholek in generatorPolaczen.WierzcholkiDrog)
            {
                if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Pasy)
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

            foreach (KrawedzGrafu droga in generatorPolaczen.Drogi)
            {
                Punkt<double> punkt = Punkt<double>.ZwrocPozycjeMniejszego(droga.WierzcholekA.Pozycja, droga.WierzcholekB.Pozycja);

                for (int i = 1; i < droga.DlugoscKrawedzi(); ++i)
                {
                    if (droga.ZwrocRelacje() == Relacja.Pionowe)
                        Tworz4Obrazki(new Punkt<double>(punkt.X, punkt.Y + i), @"Drogi\DrogaPionowoLewa.png", @"Drogi\DrogaPionowoPrawa.png", @"Drogi\DrogaPionowoLewa.png", @"Drogi\DrogaPionowoPrawa.png");
                    else
                        Tworz4Obrazki(new Punkt<double>(punkt.X + i, punkt.Y), @"Drogi\DrogaPoziomoLewa.png", @"Drogi\DrogaPoziomoLewa.png", @"Drogi\DrogaPoziomoPrawa.png", @"Drogi\DrogaPoziomoPrawa.png");
                }
            }


        }

        private void RysujBudynki()
        {
            foreach (Kwadrat budynek in generatorBudynkow.Budynki)
            {
                string folder = $"Budynek{generatorLosowosci.Next(1, 6)}";

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

        private void RysujChodniki()
        {
            foreach (KrawedzGrafu chodnik in generatorPolaczenPieszych.Chodniki)
            {
                Punkt<double> punkt = chodnik.WierzcholekA.Pozycja;

                for (int i = 0; i <= chodnik.DlugoscKrawedzi(); ++i)
                {
                    Image obrazek = TworzObrazek(@"Inne/Chodnik.png", 0);

                    if (chodnik.ZwrocRelacje() == Relacja.Pionowe)
                        UstawPozycjeObiektu(obrazek, new Punkt<double>(punkt.X * 40, (punkt.Y + i) * 40));
                    else
                        UstawPozycjeObiektu(obrazek, new Punkt<double>((punkt.X + i) * 40, punkt.Y * 40));
                }
            }

            foreach (KrawedzGrafu krawedz in generatorPolaczenPieszych.Chodniki)
            {
                System.Windows.Shapes.Polyline yellowPolyline = new System.Windows.Shapes.Polyline()
                {
                    Stroke = System.Windows.Media.Brushes.Black,
                    StrokeThickness = 4
                };

                int r = generatorLosowosci.Next(0, 10);
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
                rodzicObrazkow.Children.Add(yellowPolyline);
            }
        }

        private void RysujPasy()
        {
            foreach(WierzcholekSamochodow wierzcholek in generatorPolaczen.WierzcholkiDrog)
            {
                if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Pasy)
                {
                    if(wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWGore())
                    {
                        Image obrazek = new Image()
                        {
                            Height = 40,
                            Width = 80,
                            Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/Pasy/PasyPoziomo.png", UriKind.Absolute)),
                        };
                        Canvas.SetZIndex(obrazek, 4);

                        rodzicObrazkow.Children.Add(obrazek);

                        UstawPozycjeObiektu(obrazek, new Punkt<double>(wierzcholek.Pozycja.X*40*2, wierzcholek.Pozycja.Y * 40*2 + 20));
                    }
                    else
                    {
                        Image obrazek = new Image()
                        {
                            Height = 80,
                            Width = 40,
                            Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/Pasy/PasyPionowo.png", UriKind.Absolute)),
                        };
                        Canvas.SetZIndex(obrazek, 4);

                        rodzicObrazkow.Children.Add(obrazek);

                        UstawPozycjeObiektu(obrazek, new Punkt<double>(wierzcholek.Pozycja.X * 40 * 2 + 20, wierzcholek.Pozycja.Y * 40 * 2));
                    }
                }
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

        private Image TworzObrazek(string lokalizacjaPlku, int zIndex = 1)
        {
            Image obrazek = new Image()
            {
                Height = 40,
                Width = 40,
                Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/{lokalizacjaPlku}", UriKind.Absolute)),
            };

            Canvas.SetZIndex(obrazek, zIndex);
            rodzicObrazkow.Children.Add(obrazek);

            return obrazek;
        }
    }
}