namespace Symulator_Ruchu_Drogowego
{
    public enum TypWierzcholkaPieszych { PunktWejscia, ChodnikDrogi, ChodnikPrzestrzeni, Pasy };

    public class WierzcholekChodnika : WierzcholekGrafu
    {
        public TypWierzcholkaPieszych TypWierzcholka { get; set; }

        private IWejscieNaElement<Pieszy> obiektChodnika { get; set; }

        public WierzcholekChodnika(Punkt<double> pozycja, TypWierzcholkaPieszych typWierzcholka) : base(pozycja)
        {
            TypWierzcholka = typWierzcholka;
            obiektChodnika = new WejscieNaChodnik();
        }

        public void UstawObiektWejscia(PrzejscieDlaPieszych przejscieDlaPieszych, WierzcholekChodnika przeciwnyWierzcholek)
        {
            obiektChodnika = new WejscieNaPrzejscieDlaPieszych(przejscieDlaPieszych, przeciwnyWierzcholek);
        }

        public bool CzyMogeWejsc(Pieszy pieszy)
        {
            return obiektChodnika.CzyMogeWejsc(pieszy);
        }

        public void Wejdz(Pieszy pieszy)
        {
            obiektChodnika.Wejdz(pieszy);
        }

        public void Wyjdz(Pieszy pieszy)
        {
            obiektChodnika.Wyjdz(pieszy);
        }
    }
}