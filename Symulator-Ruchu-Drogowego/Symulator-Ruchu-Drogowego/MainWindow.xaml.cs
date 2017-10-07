using System.Windows;

namespace Symulator_Ruchu_Drogowego
{
    public partial class MainWindow : Window
    {    
        private Symulacja symulacja = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void przyciskGeneruj_Click(object sender, RoutedEventArgs e)
        {
            if (symulacja != null)
            {
                symulacja.Close();
                symulacja.Zatrzymaj();
            }
                

            symulacja = new Symulacja(  (int)szerokosc.Value, (int)wysokosc.Value, (int)punktyWejscia.Value,
                                        (int)liczbaPieszych.Value, (int)liczbaSamochodow.Value);
            symulacja.Show();
        }
    }
}