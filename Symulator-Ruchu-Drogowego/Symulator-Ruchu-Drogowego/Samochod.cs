using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Symulator_Ruchu_Drogowego
{
    public class Samochod : IDisposable
    {
        public static List<Samochod> Samochody { get; set; }
        public WierzcholekGrafu ObecnaPozycja { get; private set; }

        private Image obrazek;
        private List<WierzcholekGrafu> trasa;
        private bool czyCzekamNaPozwolenie = false;

        public Samochod(WierzcholekGrafu start, List<WierzcholekGrafu> trasa)
        {
            this.trasa = trasa;
            this.ObecnaPozycja = start;

            TworzObrazek();

            Punkt<double> przesuniecie = ((ObiektDrogiPunktWejscia)((WierzcholekDrogi)start).ObiektDrogi).PunktWejsciowy();
            UstawPozycje(new Punkt<double>(start.Pozycja.X * 80 + przesuniecie.X, start.Pozycja.Y * 80 + przesuniecie.Y));
        }

        public void PoruszanieSamochodem()
        {
            if (trasa.Count > 0)
            {
                if(!czyCzekamNaPozwolenie)
                {
                    Punkt<double> przesuniecie;

                    if(((WierzcholekDrogi)trasa[0]).TypWierzcholka == TypWierzcholkaSamochodow.Skrzyzowanie)
                        przesuniecie = ((ObiektDrogiSkrzyzowanie)((WierzcholekDrogi)trasa[0]).ObiektDrogi).Przesuniecie(ObecnaPozycja.Pozycja,trasa[1].Pozycja);
                    else
                        przesuniecie = ((WierzcholekDrogi)trasa[0]).ObiektDrogi.Przesuniecie(ObecnaPozycja.Pozycja);

                    Geometria geometriaPieszyCel = new Geometria(new Punkt<double>(trasa[0].Pozycja.X * 80 + przesuniecie.X, trasa[0].Pozycja.Y * 80 + przesuniecie.Y),
                                                                new Punkt<double>(ZwrocPozycje().X + obrazek.Width / 2, ZwrocPozycje().Y + obrazek.Height / 2));

                    if (geometriaPieszyCel.ObliczOdlegloscPomiedzy() >= 2)
                    {
                        UstawPozycje(new Punkt<double>(ZwrocPozycje().X + geometriaPieszyCel.ObliczWektorPrzesuniecia(2).X,
                                                        ZwrocPozycje().Y + geometriaPieszyCel.ObliczWektorPrzesuniecia(2).Y));

                        obrazek.RenderTransform = new RotateTransform(geometriaPieszyCel.ObliczKatPomiedzy());
                    }
                    else
                    {
                        ObecnaPozycja = trasa[0];


                        //W razie chęci poprawy - usunąć
                        ((WierzcholekDrogi)ObecnaPozycja).ObiektDrogi.Wyjedz(this);


                        trasa.RemoveAt(0);
                        czyCzekamNaPozwolenie = true;
                    }
                }
                
                if(czyCzekamNaPozwolenie && trasa.Count > 0)
                {
                    if(((WierzcholekDrogi)trasa[0]).ObiektDrogi.CzyMogeWejsc(this))
                    {
                        ((WierzcholekDrogi)ObecnaPozycja).ObiektDrogi.Wyjedz(this);
                        czyCzekamNaPozwolenie = false;
                        ((WierzcholekDrogi)trasa[0]).ObiektDrogi.Wjedz(this);
                    }
                }    
            }
            else
            {
                ((WierzcholekDrogi)ObecnaPozycja).ObiektDrogi.Wyjedz(this);
                Dispose();
            }             
        }

        private void TworzObrazek()
        {
            obrazek = new Image()
            {
                Height = 40,
                Width = 40,
                Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/Samochody/samochod{KontrolerRuchu.GeneratorLosowosci.Next(1, 4)}.png", UriKind.Absolute)),
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            Canvas.SetZIndex(obrazek, 3);
            MainWindow.Warstwa.Children.Add(obrazek);
        }

        private void UstawPozycje(Punkt<double> punkt)
        {
            Canvas.SetLeft(obrazek, punkt.X);
            Canvas.SetTop(obrazek, punkt.Y);
        }

        private Punkt<double> ZwrocPozycje()
        {
            return new Punkt<double>(Canvas.GetLeft(obrazek), Canvas.GetTop(obrazek));
        }

        public void Dispose()
        {          
            MainWindow.Warstwa.Children.Remove(obrazek);
            Samochody.Remove(this);
        }
    }
}
