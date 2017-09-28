using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public interface IObiektDrogi
    {
        bool CzyMogeWejsc(Samochod Obiekt);
        void Wjedz(Samochod samochod);
        void Wyjedz(Samochod samochod);
        Punkt<double> Przesuniecie(Punkt<double> punkt);
    }
}
