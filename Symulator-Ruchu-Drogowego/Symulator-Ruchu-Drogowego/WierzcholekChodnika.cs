using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public enum TypWierzcholkaPieszych { PunktWejscia, ChodnikDrogi, ChodnikPrzestrzeni, Pasy };

    public class WierzcholekChodnika : WierzcholekGrafu
    {
        public TypWierzcholkaPieszych TypWierzcholka { get; set; }

        public WierzcholekChodnika(Punkt<double> pozycja, TypWierzcholkaPieszych typWierzcholka) : base(pozycja)
        {
            TypWierzcholka = typWierzcholka;
        }    
    }
}
