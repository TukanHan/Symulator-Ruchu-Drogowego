using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    class WejscieNaChodnik : IWejscieNaElement<Pieszy>
    {
        public bool CzyMogeWejsc(Pieszy pieszy)
        {
            return true;
        }

        public void Wejdz(Pieszy pieszy){}

        public void Wyjdz(Pieszy pieszy){}
    }
}
