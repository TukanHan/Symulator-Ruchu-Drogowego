using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Symulator_Ruchu_Drogowego
{
    class KontrolerSamochodow
    {
        private List<WierzcholekDrogi> wierzcholkiWejscia;
        private List<WierzcholekDrogi> wierzcholkiDrogi;

        private int maxLiczbaSamochodow;
        private DispatcherTimer zdarzenieLosowania = new DispatcherTimer();
        private DispatcherTimer zdarzeniePoruszania = new DispatcherTimer();

        public KontrolerSamochodow(List<WierzcholekDrogi> wierzcholkiDrogi, int maxLiczbaSamochodow)
        {
            this.maxLiczbaSamochodow = maxLiczbaSamochodow;
            this.wierzcholkiDrogi = wierzcholkiDrogi;
            this.wierzcholkiWejscia = wierzcholkiDrogi.FindAll(obiekt => obiekt.TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia);
            Samochod.Samochody = new List<Samochod>();

            zdarzenieLosowania.Interval = new TimeSpan(0, 0, 0, 1);
            zdarzenieLosowania.Tick += (s, args) => KontrolujLiczbe();
            zdarzenieLosowania.Start();

            zdarzeniePoruszania.Interval = new TimeSpan(0, 0, 0, 0, 17);
            zdarzeniePoruszania.Tick += (s, args) => PoruszajSamochodami();
            zdarzeniePoruszania.Start();

            for (int i = 0; i < maxLiczbaSamochodow / 2; ++i)
                DodajSamochod();
        }

        public void Zatrzymaj()
        {
            zdarzenieLosowania.Stop();
            zdarzeniePoruszania.Stop();
        }

        private void PoruszajSamochodami()
        {
            for (int i = Samochod.Samochody.Count - 1; i >= 0; --i)
            {
                Samochod.Samochody[i].PoruszanieSamochodem();
            }
        }

        private void KontrolujLiczbe()
        {
            if (Samochod.Samochody.Count <= maxLiczbaSamochodow / 2)
            {
                DodajSamochod();
            }
            else if (Samochod.Samochody.Count <= maxLiczbaSamochodow)
            {
                int szansza = ((maxLiczbaSamochodow / 2) - (Samochod.Samochody.Count - (maxLiczbaSamochodow / 2))) * 10;
                if (KontrolerRuchu.GeneratorLosowosci.Next(0, 101) < szansza)
                    DodajSamochod();
            }
        }

        private void DodajSamochod()
        {
            int wierzcholekWejsciaIndex = KontrolerRuchu.GeneratorLosowosci.Next(0, wierzcholkiWejscia.Count);
            int wierzcholekWyjsciaIndex;
            do
            {
                wierzcholekWyjsciaIndex = KontrolerRuchu.GeneratorLosowosci.Next(0, wierzcholkiWejscia.Count);
            }
            while (wierzcholekWyjsciaIndex == wierzcholekWejsciaIndex);

            WyszukiwanieDrogi wyszukiwanieDrogi = new WyszukiwanieDrogi(wierzcholkiDrogi.ConvertAll(obiekt => (WierzcholekGrafu)obiekt));
            List<WierzcholekGrafu> trasa = wyszukiwanieDrogi.ZwrocScierzke(wierzcholkiWejscia[wierzcholekWejsciaIndex], wierzcholkiWejscia[wierzcholekWyjsciaIndex]);

            Samochod.Samochody.Add(new Samochod(wierzcholkiWejscia[wierzcholekWejsciaIndex], trasa));
        }
    }
}
