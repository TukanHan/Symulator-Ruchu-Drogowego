using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public class PrzejscieDlaPieszych
    {
        enum Obecnosc { Nikt, Samochod, Pieszy };

        private Obecnosc obecnosc = Obecnosc.Nikt;
        private int liczbaSamochodow =0;
        private int liczbaPieszych =0;

        public bool CzyMogeWejsc(Object obiekt)
        {
            if (obiekt is Pieszy)
                return obecnosc != Obecnosc.Samochod;
            else if (obiekt is Samochod)
                return obecnosc != Obecnosc.Pieszy;
            else
                throw new Exception("Nieokreślony obiekt jako argument");
        }

        public void Wjedz(Object obiekt)
        {
            if (obiekt is Samochod)
            {
                obecnosc = Obecnosc.Samochod;
                liczbaSamochodow++;
            }
            else if (obiekt is Pieszy)
            {
                obecnosc = Obecnosc.Pieszy;
                liczbaPieszych++;
            }
            else
                throw new Exception("Nieokreślony obiekt jako argument");
        }

        public void Wyjedz(Object obiekt)
        {
            if (obiekt is Samochod)
            {
                liczbaSamochodow--;
                if (liczbaSamochodow == 0)
                    obecnosc = Obecnosc.Nikt;
            }
            else if (obiekt is Pieszy)
            {
                liczbaPieszych--;
                if (liczbaPieszych == 0)
                    obecnosc = Obecnosc.Nikt;
            }
            else
                throw new Exception("Nieokreślony obiekt jako argument");
        }
    }
}
