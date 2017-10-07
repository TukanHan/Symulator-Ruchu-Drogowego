using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{

    public enum TypPasow {  PrzejsciePieszychPionowe, PrzejsciePieszychPoziome, LiniaPrzerywanaPionowa, LiniaPrzerywanaPozioma,
                            ZakretDolPrawo, ZakretDolLewo, ZakretGoraPrawo, ZakretGoraLewo };

    public class ZnakPoziomy
    {
        public TypPasow TypPasow { get; }
        public Punkt<double> Pozycja { get; }

        public ZnakPoziomy(TypPasow typPasow, Punkt<double> pozycja)
        {
            TypPasow = typPasow;
            Pozycja = pozycja;
        }
    }

    public class GeneratorZnakowPoziomych
    {
        public List<ZnakPoziomy> ZnakiPoziome { get; } = new List<ZnakPoziomy>();

        public GeneratorZnakowPoziomych(GeneratorPolaczenSamochodow generatorPolaczen)
        {
            GenerujPasyZWierzcholkow(generatorPolaczen);
        }

        private void GenerujPasyZWierzcholkow(GeneratorPolaczenSamochodow generatorPolaczen)
        {
            foreach(WierzcholekDrogi wierzcholek in generatorPolaczen.WierzcholkiDrog)
            {
                if (wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Pasy)
                {
                    if (wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWGore())
                        ZnakiPoziome.Add(new ZnakPoziomy(TypPasow.PrzejsciePieszychPoziome, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                    else
                        ZnakiPoziome.Add(new ZnakPoziomy(TypPasow.PrzejsciePieszychPionowe, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));                       
                }
                else if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Zakret)
                {
                    if(wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWPrawo())
                        ZnakiPoziome.Add(new ZnakPoziomy(TypPasow.ZakretDolPrawo, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                    else if(wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWLewo())
                        ZnakiPoziome.Add(new ZnakPoziomy(TypPasow.ZakretDolLewo, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                    else if (wierzcholek.CzyJestDrogaWGore() && wierzcholek.CzyJestDrogaWPrawo())
                        ZnakiPoziome.Add(new ZnakPoziomy(TypPasow.ZakretGoraPrawo, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                    else if (wierzcholek.CzyJestDrogaWGore() && wierzcholek.CzyJestDrogaWLewo())
                        ZnakiPoziome.Add(new ZnakPoziomy(TypPasow.ZakretGoraLewo, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                }
                else if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Droga)
                {
                    if (wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWGore())
                        ZnakiPoziome.Add(new ZnakPoziomy(TypPasow.LiniaPrzerywanaPionowa, new Punkt<double>(wierzcholek.Pozycja.X * 80 + 38, wierzcholek.Pozycja.Y * 80)));
                    else
                        ZnakiPoziome.Add(new ZnakPoziomy(TypPasow.LiniaPrzerywanaPozioma, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80 + 38)));
                }
            }
        }
    }
}
