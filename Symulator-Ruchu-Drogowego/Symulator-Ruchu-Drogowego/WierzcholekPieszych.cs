using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public enum TypWierzcholkaPieszych { PunktWejscia, ChodnikDrogi, ChodnikPrzestrzeni, Pasy };

    public class WierzcholekPieszych : WierzcholekGrafu
    {
        public TypWierzcholkaPieszych TypWierzcholka { get; set; }

        public WierzcholekPieszych(Punkt<double> pozycja, TypWierzcholkaPieszych typWierzcholka) : base(pozycja)
        {
            TypWierzcholka = typWierzcholka;
        }    
    }
}
