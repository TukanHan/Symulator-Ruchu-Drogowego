using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public enum TypWierzcholkaSamochodow { PunktWejscia, Skrzyzowanie, Zakret, Pasy };

    public class WierzcholekSamochodow : WierzcholekGrafu
    {
        public TypWierzcholkaSamochodow TypWierzcholka { get; set; }

        public WierzcholekSamochodow(Punkt<double> pozycja, TypWierzcholkaSamochodow typWierzcholka) : base(pozycja)
        {
            TypWierzcholka = typWierzcholka;
        }
    }
}
