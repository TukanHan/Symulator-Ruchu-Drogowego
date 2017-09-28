﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Symulator_Ruchu_Drogowego
{
    public partial class MainWindow : Window
    {
        public static Canvas Warstwa { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Warstwa = warstwa;

            GenerujSymulacje(12,8,8);
        }

        private void GenerujSymulacje(int szerokosc, int wysokosc, int liczbaWejsc)
        {
            bool wygenerowanoMape = false;
            while (!wygenerowanoMape)
            {
                //warstwa.Children.Clear();

                try
                {
                    GeneratorPoziomu generatorPoziomu = new GeneratorPoziomu(warstwa, szerokosc, wysokosc, liczbaWejsc);
                    KontrolerRuchu kontorlerRuchu = new KontrolerRuchu(generatorPoziomu.WierzcholkiChodnikow, 30, generatorPoziomu.WierzcholkiDrog, 20);
                    wygenerowanoMape = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        //private void warstwa_KeyUp(object sender, MouseButtonEventArgs e)
        //{
        //    GenerujSymulacje(12,8,8);
        //}
    }
}