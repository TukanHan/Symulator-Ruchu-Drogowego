using System.Windows;
using System.Windows.Controls;

namespace Symulator_Ruchu_Drogowego
{
    /// <summary>
    /// Interaction logic for Symulacja.xaml
    /// </summary>
    public partial class Symulacja : Window
    {
        public static Canvas Warstwa { get; private set; }

        private KontrolerRuchu kontorlerRuchu;

        public Symulacja(int szerokosc, int wysokosc, int liczbaWejsc, int liczbaPieszych, int liczbaSamochodow)
        {
            InitializeComponent();
            Warstwa = warstwa;

            GenerujSymulacje(szerokosc, wysokosc, liczbaWejsc, liczbaPieszych, liczbaSamochodow);
        }

        public void Zatrzymaj()
        {
            kontorlerRuchu.Zatrzymaj();
        }

        private void GenerujSymulacje(int szerokosc, int wysokosc, int liczbaWejsc, int liczbaPieszych, int liczbaSamochodow)
        {
            bool wygenerowanoMape = false;
            int liczbaPowtorzen = 0;
            while (!wygenerowanoMape)
            {
                warstwa.Children.Clear();
                try
                {
                    GeneratorPoziomu generatorPoziomu = new GeneratorPoziomu(warstwa, szerokosc, wysokosc, liczbaWejsc);
                    kontorlerRuchu = new KontrolerRuchu(generatorPoziomu.WierzcholkiChodnikow, liczbaPieszych, generatorPoziomu.WierzcholkiDrog, liczbaSamochodow);

                    wygenerowanoMape = true;
                    Height = 80 * wysokosc + 35;
                    Width = 80 * szerokosc + 5;
                }
                catch
                {                  
                    ++liczbaPowtorzen;
                    if(liczbaPowtorzen % 3 == 0)
                        liczbaWejsc--;  
                }
            }
        }
    }
}
