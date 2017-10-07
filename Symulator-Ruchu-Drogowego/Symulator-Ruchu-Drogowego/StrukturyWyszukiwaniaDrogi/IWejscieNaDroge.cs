using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public interface IWejscieNaDroge : IWejscieNaElement<Samochod>
    {
        Punkt<double> Przesuniecie(Punkt<double> punkt);
    }
}
