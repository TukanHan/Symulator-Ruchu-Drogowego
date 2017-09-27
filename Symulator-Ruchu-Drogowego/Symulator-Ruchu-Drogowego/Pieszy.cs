﻿using System;
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
    public class Pieszy : IDisposable
    {
        public static List<Pieszy> Piesi { get; set; }

        private Image obrazek;
        //private WierzcholekGrafu obecnyWierzcholek;
        private List<WierzcholekGrafu> trasa;
        private DispatcherTimer zdarzeniePoruszania = new DispatcherTimer();

        public Pieszy(WierzcholekGrafu start, List<WierzcholekGrafu> trasa)
        {
            //this.obecnyWierzcholek = start;
            this.trasa = trasa;

            zdarzeniePoruszania.Interval = new TimeSpan(0, 0, 0, 0, 40);
            zdarzeniePoruszania.Tick += (s, args) => PoruszaniePostaci();
            zdarzeniePoruszania.Start();

            TworzObrazek();
            UstawPozycje(new Punkt<double>(start.Pozycja.X*40 + obrazek.Width / 2, start.Pozycja.Y * 40 + obrazek.Height / 2));
        }

        private void PoruszaniePostaci()
        {
            if (trasa.Count > 0)
            {
                Geometria geometriaPieszyCel = new Geometria(   new Punkt<double>(trasa[0].Pozycja.X*40+20, trasa[0].Pozycja.Y * 40+20),
                                                                new Punkt<double>(ZwrocPozycje().X + obrazek.Width / 2, ZwrocPozycje().Y + obrazek.Height / 2));

                if (geometriaPieszyCel.ObliczOdlegloscPomiedzy() >= 2)
                {
                    UstawPozycje(new Punkt<double>( ZwrocPozycje().X + geometriaPieszyCel.ObliczWektorPrzesuniecia(1.5).X,
                                                    ZwrocPozycje().Y + geometriaPieszyCel.ObliczWektorPrzesuniecia(1.5).Y));

                    obrazek.RenderTransform = new RotateTransform(geometriaPieszyCel.ObliczKatPomiedzy());
                }
                else
                    trasa.RemoveAt(0);                
            }
            else
                Dispose();
        }

        private void TworzObrazek()
        {
            obrazek = new Image()
            {
                Height = 16,
                Width = 16,
                Source = new BitmapImage(new Uri($@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/Obrazki/Osoby/Osoba{KontrolerRuchu.GeneratorLosowosci.Next(1, 10)}.png", UriKind.Absolute)),
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
            zdarzeniePoruszania.Stop();
            MainWindow.Warstwa.Children.Remove(obrazek);
            Piesi.Remove(this);
        }
    }
}
