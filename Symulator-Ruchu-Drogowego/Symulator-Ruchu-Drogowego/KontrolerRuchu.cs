using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public class KontrolerRuchu
    {
        public static Random GeneratorLosowosci { get; } = new Random();

        public KontrolerRuchu(List<WierzcholekChodnika> wierzcholkiPieszych, int maxLiczbaPieszych, List<WierzcholekDrogi> wierzcholkiDrogi, int maxLiczbaSamochodow)
        {
            KontrolerPieszych kontrolerPieszych = new KontrolerPieszych(wierzcholkiPieszych, maxLiczbaPieszych);
            //KontrolerSamochodow kontolerSamochodow = new KontrolerSamochodow(wierzcholkiDrogi,maxLiczbaSamochodow);
        }
    }
}
