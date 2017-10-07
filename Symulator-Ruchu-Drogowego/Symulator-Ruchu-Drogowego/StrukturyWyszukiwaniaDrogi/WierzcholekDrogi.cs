namespace Symulator_Ruchu_Drogowego
{
    public enum TypWierzcholkaSamochodow { PunktWejscia, Skrzyzowanie, Zakret, Pasy, Droga };

    public class WierzcholekDrogi : WierzcholekGrafu
    {
        public TypWierzcholkaSamochodow TypWierzcholka { get; set; }
        public IWejscieNaDroge ObiektDrogi { get; set; }

        public WierzcholekDrogi(Punkt<double> pozycja, TypWierzcholkaSamochodow typWierzcholka) : base(pozycja)
        {
            TypWierzcholka = typWierzcholka;
        }

        public void UstawObiektDrogi()
        {
            if (TypWierzcholka == TypWierzcholkaSamochodow.PunktWejscia)
                ObiektDrogi = new WejscieNaPunktWejscia(Pozycja);
            else if(TypWierzcholka == TypWierzcholkaSamochodow.Pasy)
                ObiektDrogi = new WejscieNaPasy(Pozycja);
            else if (TypWierzcholka == TypWierzcholkaSamochodow.Droga)
                ObiektDrogi = new WejscieNaDroge(Pozycja);
            else if (TypWierzcholka == TypWierzcholkaSamochodow.Skrzyzowanie)
                ObiektDrogi = new WejscieNaSkrzyzowanie(Pozycja);
            else if (TypWierzcholka == TypWierzcholkaSamochodow.Zakret)
                ObiektDrogi = new WejscieNaZakret(this);
        }

        public bool CzyMogeWejsc(Samochod samochod)
        {
            return ObiektDrogi.CzyMogeWejsc(samochod);
        }

        public void Wejdz(Samochod samochod)
        {
            ObiektDrogi.Wejdz(samochod);
        }

        public void Wyjdz(Samochod samochod)
        {
            ObiektDrogi.Wyjdz(samochod);
        }
    }
}