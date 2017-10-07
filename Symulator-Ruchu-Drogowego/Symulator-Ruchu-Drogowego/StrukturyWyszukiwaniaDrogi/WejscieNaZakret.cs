using System;

namespace Symulator_Ruchu_Drogowego
{
    class WejscieNaZakret : IWejscieNaDroge
    {
        private object prawyPojazd = null;
        private object lewyPojazd = null;
        private WierzcholekGrafu ja;

        public WejscieNaZakret(WierzcholekGrafu ja)
        {
            this.ja = ja;
        }

        public bool CzyMogeWejsc(Samochod samochod)
        {
            if (ja.CzyJestDrogaWDol() && (ja.CzyJestDrogaWPrawo() || ja.CzyJestDrogaWLewo()))
            {
                if (samochod.ObecnaPozycja.Pozycja.Y > ja.Pozycja.Y)
                    return prawyPojazd == null;
                else
                    return lewyPojazd == null;
            }
            else if (ja.CzyJestDrogaWGore() && (ja.CzyJestDrogaWLewo() || ja.CzyJestDrogaWPrawo()))
            {
                if (samochod.ObecnaPozycja.Pozycja.Y < ja.Pozycja.Y)
                    return lewyPojazd == null;
                else
                    return prawyPojazd == null;
            }

            throw new Exception("Niepoprawny obiekt na wejściu");
        }

        public void Wejdz(Samochod samochod)
        {
            if(ja.CzyJestDrogaWDol() && (ja.CzyJestDrogaWPrawo() || ja.CzyJestDrogaWLewo()))
            {
                if (samochod.ObecnaPozycja.Pozycja.Y > ja.Pozycja.Y)
                    prawyPojazd = samochod;
                else
                    lewyPojazd = samochod;
            }
            else if (ja.CzyJestDrogaWGore() && (ja.CzyJestDrogaWLewo() || ja.CzyJestDrogaWPrawo()))
            {
                if (samochod.ObecnaPozycja.Pozycja.Y < ja.Pozycja.Y)
                    lewyPojazd = samochod;
                else
                    prawyPojazd = samochod;
            }
            else
                throw new Exception("Niepoprawny obiekt na wejściu");
        }

        public void Wyjdz(Samochod samochod) 
        {
            if (samochod == lewyPojazd)
                lewyPojazd = null;
            else if (samochod == prawyPojazd)
                prawyPojazd = null;
            else
                throw new Exception("Niepoprawny obiekt na wejściu");
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