using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    class WejscieNaSkrzyzowanie : IWejscieNaDroge
    {
        private Samochod[] pojazd = new Samochod[2];
        private bool[][] przecinaneKwadraty = new bool[2][];
        private Punkt<double> pozycja;

        public WejscieNaSkrzyzowanie(Punkt<double> pozycja)
        {
            this.pozycja = pozycja;
        }

        public bool CzyMogeWejsc(Samochod samochod)
        {
            if (pojazd[0] == null && pojazd[1] == null)
                return true;
            else if (pojazd[0] == null || pojazd[1] == null)
            {
                int index = pojazd[0] == null ? 1 : 0;

                bool[] tablicaPrzecinan = WyznaczPrzecinanieDrogi(samochod);

                bool czyPrzecina = false;
                for (int i = 0; i < 4 && !czyPrzecina; ++i)
                {
                    if (przecinaneKwadraty[index][i] && tablicaPrzecinan[i])
                        czyPrzecina = true;
                }

                return !czyPrzecina;
            }
            else
                return false;
        }

        public void Wejdz(Samochod samochod)
        {
            if(pojazd[0] == null)
            {
                pojazd[0] = samochod;
                przecinaneKwadraty[0] = WyznaczPrzecinanieDrogi(samochod);
            }            
            else if (pojazd[1] == null)
            {
                pojazd[1] = samochod;
                przecinaneKwadraty[1] = WyznaczPrzecinanieDrogi(samochod);
            }
            else
                throw new Exception("Nieprawidłowy obiekt na wejściu");
        }

        public void Wyjdz(Samochod samochod)
        {
            if (samochod == pojazd[0])
            {
                pojazd[0] = null;
                przecinaneKwadraty[0] = null;
            }              
            else if (samochod == pojazd[1])
            {
                pojazd[1] = null;
                przecinaneKwadraty[1] = null;
            }
            else
                throw new Exception("Nieprawidłowy obiekt na wejściu");
        }

        public Punkt<double> Przesuniecie(Punkt<double> punkt)
        {
            throw new NotImplementedException();
        }

        public Punkt<double> Przesuniecie(Punkt<double> punkt, Punkt<double> kolejny)
        {
            if (kolejny.Y > pozycja.Y) // w dół
                return new Punkt<double>(20, 60);
            else if (kolejny.Y < pozycja.Y) // w górę
                return new Punkt<double>(60, 20);
            else if (kolejny.X > pozycja.X) //w prawo
                return new Punkt<double>(60, 60);
            else if (kolejny.X < pozycja.X)  // w lewo
                return new Punkt<double>(20, 20);
            else
                return Punkt<double>.PustyPunkt;
        }

        private bool[] WyznaczPrzecinanieDrogi(Samochod samochod)
        {
            bool[] tab = new bool[4];

            int wejscie = -1;
            if (samochod.ObecnaPozycja.Pozycja.X < pozycja.X)//lewo
                wejscie = 2;
            else if (samochod.ObecnaPozycja.Pozycja.X > pozycja.X)//prawo
                wejscie = 1;
            else if (samochod.ObecnaPozycja.Pozycja.Y < pozycja.Y)//prawo
                wejscie = 0;
            else if (samochod.ObecnaPozycja.Pozycja.Y > pozycja.Y)//prawo
                wejscie = 3;
            else
                return tab;

            tab[wejscie] = true;

            int wyjscie = -1;
            if (samochod.Cel.Pozycja.X < pozycja.X)//lewo
                wyjscie = 0;
            else if (samochod.Cel.Pozycja.X > pozycja.X)//prawo
                wyjscie = 3;
            else if (samochod.Cel.Pozycja.Y < pozycja.Y)//prawo
                wyjscie = 1;
            else if (samochod.Cel.Pozycja.Y > pozycja.Y)//prawo
                wyjscie = 2;

            tab[wyjscie] = true;

            if (wejscie == 3 && wyjscie == 0)
                tab[1] = true;
            else if (wejscie == 2 && wyjscie == 1)
                tab[3] = true;
            else if (wejscie == 0 && wyjscie == 3)
                tab[2] = true;
            else if (wejscie == 1 && wyjscie == 2)
                tab[0] = true;

            return tab;
        }

        public void ZmienPrzecinanieDrogi(Samochod samochod)
        {
            int index = -1;

            if (pojazd[0] == samochod)
                index = 0;
            else if (pojazd[1] == samochod)
                index = 1;

            przecinaneKwadraty[index] = new bool[4];

            if (samochod.Cel.Pozycja.X < pozycja.X)//lewo
                przecinaneKwadraty[index][0] = true;
            else if (samochod.Cel.Pozycja.X > pozycja.X)//prawo
                przecinaneKwadraty[index][3] = true;
            else if (samochod.Cel.Pozycja.Y < pozycja.Y)//prawo
                przecinaneKwadraty[index][1] = true;
            else if (samochod.Cel.Pozycja.Y > pozycja.Y)//prawo
                przecinaneKwadraty[index][2] = true;
        }
    }
}
