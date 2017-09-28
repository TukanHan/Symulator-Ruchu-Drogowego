using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    class ObiektDrogiZakret : IObiektDrogi
    {
        private object prawyPojazd = null;
        private object lewyPojazd = null;
        private WierzcholekGrafu ja;

        public ObiektDrogiZakret(WierzcholekGrafu ja)
        {
            this.ja = ja;
        }

        public bool CzyMogeWejsc(Samochod Obiekt)
        {
            return true;
        }

        public void Wjedz(Samochod Obiekt)
        {

        }

        public void Wyjedz(Samochod Obiekt)
        {

        }

        public Punkt<double> Przesuniecie(Punkt<double> punkt)
        {
            if (ja.CzyJestDrogaWGore() && ja.CzyJestDrogaWPrawo())
            {
                if (punkt.Y < ja.Pozycja.Y)
                    return new Punkt<double>(20, 60);
                else
                    return new Punkt<double>(60, 20);
            }          
            else if (ja.CzyJestDrogaWGore() && ja.CzyJestDrogaWLewo())
            {
                if (punkt.Y < ja.Pozycja.Y)
                    return new Punkt<double>(20, 20);
                else
                    return new Punkt<double>(60, 60);
            }
            else if (ja.CzyJestDrogaWDol() && ja.CzyJestDrogaWLewo())
            {
                if (punkt.Y > ja.Pozycja.Y)
                    return new Punkt<double>(60, 20);
                else
                    return new Punkt<double>(20, 60);
            }
            else
            {
                if (punkt.Y > ja.Pozycja.Y)
                    return new Punkt<double>(60, 60);
                else
                    return new Punkt<double>(20, 20);
            }
        }
    }
}
