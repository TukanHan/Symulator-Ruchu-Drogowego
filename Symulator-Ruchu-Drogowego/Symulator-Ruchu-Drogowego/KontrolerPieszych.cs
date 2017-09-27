using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Symulator_Ruchu_Drogowego
{
    public class KontrolerPieszych
    {
        private List<WierzcholekChodnika> wierzcholkiWejscia;
        private List<WierzcholekChodnika> wierzcholkiChodnika;

        private int maksymalnaLiczbaPieszych;
        private DispatcherTimer zdarzenieLosowania = new DispatcherTimer();

        public KontrolerPieszych(List<WierzcholekChodnika> wierzcholkiPieszych,int maksymalnaLiczbaPieszych)
        {
            this.maksymalnaLiczbaPieszych = maksymalnaLiczbaPieszych;
            this.wierzcholkiChodnika = wierzcholkiPieszych;
            this.wierzcholkiWejscia = wierzcholkiPieszych.FindAll(obiekt => obiekt.TypWierzcholka == TypWierzcholkaPieszych.PunktWejscia);
            Pieszy.Piesi = new List<Pieszy>();

            zdarzenieLosowania.Interval = new TimeSpan(0, 0, 0, 1);
            zdarzenieLosowania.Tick += (s, args) => KontrolujLiczbe();
            zdarzenieLosowania.Start();

            for (int i = 0; i < maksymalnaLiczbaPieszych / 2; ++i)
                DodajPostac();
        }

        private void KontrolujLiczbe()
        {
            if(Pieszy.Piesi.Count<= maksymalnaLiczbaPieszych/2)
            {
                DodajPostac();
            }
            else if(Pieszy.Piesi.Count <= maksymalnaLiczbaPieszych)
            {
                int szansza = ((maksymalnaLiczbaPieszych / 2) - (Pieszy.Piesi.Count - (maksymalnaLiczbaPieszych / 2))) * 10;
                if (KontrolerRuchu.GeneratorLosowosci.Next(0, 101) < szansza)
                    DodajPostac();
            }
        }

        private void DodajPostac()
        {
            int wierzcholekWejsciaIndex = KontrolerRuchu.GeneratorLosowosci.Next(0, wierzcholkiWejscia.Count);
            int wierzcholekWyjsciaIndex;
            do
            {
                wierzcholekWyjsciaIndex = KontrolerRuchu.GeneratorLosowosci.Next(0, wierzcholkiWejscia.Count);
            }
            while (wierzcholekWyjsciaIndex == wierzcholekWejsciaIndex);

            WyszukiwanieDrogi wyszukiwanieDrogi = new WyszukiwanieDrogi(wierzcholkiChodnika.ConvertAll( obiekt => (WierzcholekGrafu)obiekt));
            List<WierzcholekGrafu> trasa = wyszukiwanieDrogi.ZwrocScierzke(wierzcholkiWejscia[wierzcholekWejsciaIndex], wierzcholkiWejscia[wierzcholekWyjsciaIndex]);

            Pieszy.Piesi.Add(new Pieszy(wierzcholkiWejscia[wierzcholekWejsciaIndex],trasa));
        }
    }
}