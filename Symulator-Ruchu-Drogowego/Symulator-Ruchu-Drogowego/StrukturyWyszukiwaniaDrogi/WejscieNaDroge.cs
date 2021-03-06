﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public class WejscieNaDroge : IWejscieNaDroge
    {
        private object prawyPojazd = null;
        private object lewyPojazd = null;
        private Punkt<double> pozycja;

        public WejscieNaDroge(Punkt<double> pozycja)
        {
            this.pozycja = pozycja;
        }

        public bool CzyMogeWejsc(Samochod samochod)
        {
            if (Punkt<double>.ZwrocRelacje(samochod.ObecnaPozycja.Pozycja, pozycja) == Relacja.Pionowe)
            {
                if (samochod.ObecnaPozycja.Pozycja.Y > pozycja.Y)
                    return lewyPojazd == null;
                else
                    return prawyPojazd == null;
            }
            else if (Punkt<double>.ZwrocRelacje(samochod.ObecnaPozycja.Pozycja, pozycja) == Relacja.Poziome)
            {
                if (samochod.ObecnaPozycja.Pozycja.X > pozycja.X)
                    return prawyPojazd == null;
                else
                    return lewyPojazd == null;
            }
            else
                throw new Exception("Nieprawidłowy obiekt na wejściu");
        }

        public void Wejdz(Samochod samochod)
        {
            if (Punkt<double>.ZwrocRelacje(samochod.ObecnaPozycja.Pozycja, pozycja) == Relacja.Pionowe)
            {
                if (samochod.ObecnaPozycja.Pozycja.Y > pozycja.Y)
                    lewyPojazd = samochod;
                else
                    prawyPojazd = samochod;
            }
            else if (Punkt<double>.ZwrocRelacje(samochod.ObecnaPozycja.Pozycja, pozycja) == Relacja.Poziome)
            {
                if (samochod.ObecnaPozycja.Pozycja.X > pozycja.X)
                    prawyPojazd = samochod;
                else
                    lewyPojazd = samochod;
            }
            else
                throw new Exception("Nieprawidłowy obiekt na wejściu");
        }

        public void Wyjdz(Samochod samochod)
        {
            if (samochod == lewyPojazd)
                lewyPojazd = null;
            else if (samochod == prawyPojazd)
                prawyPojazd = null;
            else
                throw new Exception("Nieprawidłowy obiekt na wejściu");
        }

        public Punkt<double> Przesuniecie(Punkt<double> punkt)
        {
            if(Punkt<double>.ZwrocRelacje(punkt,pozycja) == Relacja.Pionowe)
            {
                if (punkt.Y > pozycja.Y)
                    return new Punkt<double>(60, 40);
                else
                    return new Punkt<double>(20, 40);
            }
            else
            {
                if (punkt.X > pozycja.X)
                    return new Punkt<double>(40, 20);
                else
                    return new Punkt<double>(40, 60);
            }
        }
    }
}
