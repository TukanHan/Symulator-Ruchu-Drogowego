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

        private KontrolerPieszych kontrolerPieszych;
        private KontrolerSamochodow kontolerSamochodow;

        public KontrolerRuchu(List<WierzcholekChodnika> wierzcholkiPieszych, int maxLiczbaPieszych, List<WierzcholekDrogi> wierzcholkiDrogi, int maxLiczbaSamochodow)
        {
            kontrolerPieszych = new KontrolerPieszych(wierzcholkiPieszych, maxLiczbaPieszych);
            kontolerSamochodow = new KontrolerSamochodow(wierzcholkiDrogi,maxLiczbaSamochodow);
        }

        public void Zatrzymaj()
        {
            kontrolerPieszych.Zatrzymaj();
            kontolerSamochodow.Zatrzymaj();
        }
    }
}
