using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    class ObiektDrogiSkrzyzowanie : IObiektDrogi
    {
        private object pojazd = null;
        private Punkt<double> pozycja;

        public ObiektDrogiSkrzyzowanie(Punkt<double> pozycja)
        {
            this.pozycja = pozycja;
        }

        public bool CzyMogeWejsc(Samochod samochod)
        {
            return pojazd == null;
        }

        public void Wjedz(Samochod samochod)
        {
            pojazd = samochod;
        }

        public void Wyjedz(Samochod samochod)
        {
            if (samochod == pojazd)
                pojazd = null;
        }

        public Punkt<double> Przesuniecie(Punkt<double> punkt)
        {
            return new Punkt<double>(40,40);
        }

        public Punkt<double> Przesuniecie(Punkt<double> punkt, Punkt<double> kolejny)
        {
            if(kolejny.Y > pozycja.Y) // w dół
            {
                return new Punkt<double>(20, 60);
            }
            else if (kolejny.Y < pozycja.Y) // w górę
            {
                return new Punkt<double>(60, 20);
            }
            else if (kolejny.X > pozycja.X) //w prawo
            {
                return new Punkt<double>(60, 60);
            }
            else  // w lewo
            {
                return new Punkt<double>(20, 20);
            }
        }
    }
}
