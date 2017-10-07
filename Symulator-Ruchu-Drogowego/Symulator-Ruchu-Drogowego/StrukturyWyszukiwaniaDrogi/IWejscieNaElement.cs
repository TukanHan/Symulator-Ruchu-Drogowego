using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public interface IWejscieNaElement<T>
    {
        bool CzyMogeWejsc(T obiekt);
        void Wejdz(T samochod);
        void Wyjdz(T samochod);
    }
}