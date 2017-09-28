using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public enum TypWierzcholkaSamochodow { PunktWejscia, Skrzyzowanie, Zakret, Pasy, Droga };

    public class WierzcholekDrogi : WierzcholekGrafu
    {
        public TypWierzcholkaSamochodow TypWierzcholka { get; set; }
        public IObiektDrogi ObiektDrogi { get; set; }

        public WierzcholekDrogi(Punkt<double> pozycja, TypWierzcholkaSamochodow typWierzcholka) : base(pozycja)
        {
            TypWierzcholka = typWierzcholka;
        }

        public void UstawObiektDrogi()
        {
            if (TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia)
                ObiektDrogi = new ObiektDrogiPunktWejscia(Pozycja);
            else if(TypWierzcholka == TypWierzcholkaSamochodow.Pasy)
                ObiektDrogi = new ObiektDrogiPasy(Pozycja);
            else if (TypWierzcholka == TypWierzcholkaSamochodow.Droga)
                ObiektDrogi = new ObiektDrogiDroga(Pozycja);
            else if (TypWierzcholka == TypWierzcholkaSamochodow.Skrzyzowanie)
                ObiektDrogi = new ObiektDrogiSkrzyzowanie(Pozycja);
            else if (TypWierzcholka == TypWierzcholkaSamochodow.Zakret)
                ObiektDrogi = new ObiektDrogiZakret(this);
        }
    }
}
