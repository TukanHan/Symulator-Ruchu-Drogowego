using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{

    public enum TypPasow {  PrzejsciePieszychPionowe, PrzejsciePieszychPoziome, LiniaPrzerywanaPionowa, LiniaPrzerywanaPozioma,
                            ZakretDolPrawo, ZakretDolLewo, ZakretGoraPrawo, ZakretGoraLewo };

    public class Pasy
    {
        public TypPasow TypPasow { get; }
        public Punkt<double> Pozycja { get; }

        public Pasy(TypPasow typPasow, Punkt<double> pozycja)
        {
            TypPasow = typPasow;
            Pozycja = pozycja;
        }
    }

    public class GeneratorPasow
    {
        public List<Pasy> Pasy { get; } = new List<Pasy>();

        public GeneratorPasow(GeneratorPolaczenSamochodow generatorPolaczen)
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
                        Pasy.Add(new Pasy(TypPasow.PrzejsciePieszychPoziome, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                    else
                        Pasy.Add(new Pasy(TypPasow.PrzejsciePieszychPionowe, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));                       
                }
                else if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Zakret)
                {
                    if(wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWPrawo())
                        Pasy.Add(new Pasy(TypPasow.ZakretDolPrawo, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                    else if(wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWLewo())
                        Pasy.Add(new Pasy(TypPasow.ZakretDolLewo, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                    else if (wierzcholek.CzyJestDrogaWGore() && wierzcholek.CzyJestDrogaWPrawo())
                        Pasy.Add(new Pasy(TypPasow.ZakretGoraPrawo, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                    else if (wierzcholek.CzyJestDrogaWGore() && wierzcholek.CzyJestDrogaWLewo())
                        Pasy.Add(new Pasy(TypPasow.ZakretGoraLewo, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80)));
                }
                else if(wierzcholek.TypWierzcholka == TypWierzcholkaSamochodow.Droga)
                {
                    if (wierzcholek.CzyJestDrogaWDol() && wierzcholek.CzyJestDrogaWGore())
                        Pasy.Add(new Pasy(TypPasow.LiniaPrzerywanaPionowa, new Punkt<double>(wierzcholek.Pozycja.X * 80 + 38, wierzcholek.Pozycja.Y * 80)));
                    else
                        Pasy.Add(new Pasy(TypPasow.LiniaPrzerywanaPozioma, new Punkt<double>(wierzcholek.Pozycja.X * 80, wierzcholek.Pozycja.Y * 80 + 38)));
                }
            }
        }
    }
}
