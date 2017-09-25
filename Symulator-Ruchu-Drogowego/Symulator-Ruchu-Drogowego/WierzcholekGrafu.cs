using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public class WierzcholekGrafu
    {
        public Punkt<double> Pozycja { get; }
        public List<KrawedzGrafu> Krawedzie { get; protected set; } = new List<KrawedzGrafu>();

        public WierzcholekGrafu(Punkt<double> pozycja)
        {
            Pozycja = pozycja;
        }

        public static WierzcholekGrafu ZwrocMniejszyWierzcholek(WierzcholekGrafu wierzcholekA, WierzcholekGrafu wierzcholekB)
        {
            if (Punkt<double>.ZwrocPozycjeMniejszego(wierzcholekA.Pozycja, wierzcholekB.Pozycja).Equals(wierzcholekA.Pozycja))
                return wierzcholekA;
            else
                return wierzcholekB;
        }

        public bool CzyJestDrogaWGore()
        {
            foreach (KrawedzGrafu krawedz in Krawedzie)
            {
                if (krawedz.ZwrocPrzeciwnyWierzcholek(this).Pozycja.Y < Pozycja.Y)
                    return true;
            }
            return false;
        }

        public bool CzyJestDrogaWDol()
        {
            foreach (KrawedzGrafu krawedz in Krawedzie)
            {
                if (krawedz.ZwrocPrzeciwnyWierzcholek(this).Pozycja.Y > Pozycja.Y)
                    return true;
            }
            return false;
        }

        public bool CzyJestDrogaWLewo()
        {
            foreach (KrawedzGrafu krawedz in Krawedzie)
            {
                if (krawedz.ZwrocPrzeciwnyWierzcholek(this).Pozycja.X < Pozycja.X)
                    return true;
            }
            return false;
        }

        public bool CzyJestDrogaWPrawo()
        {
            foreach (KrawedzGrafu krawedz in Krawedzie)
            {
                if (krawedz.ZwrocPrzeciwnyWierzcholek(this).Pozycja.X > Pozycja.X)
                    return true;
            }
            return false;
        }

        public KrawedzGrafu ZwrocKrawedzGorna()
        {
            foreach (KrawedzGrafu krawedz in Krawedzie)
            {
                if (krawedz.ZwrocPrzeciwnyWierzcholek(this).Pozycja.Y < Pozycja.Y)
                    return krawedz;
            }
            return null;
        }

        public KrawedzGrafu ZwrocKrawedzDolna()
        {
            foreach (KrawedzGrafu krawedz in Krawedzie)
            {
                if (krawedz.ZwrocPrzeciwnyWierzcholek(this).Pozycja.Y > Pozycja.Y)
                    return krawedz;
            }
            return null;
        }

        public KrawedzGrafu ZwrocKrawedzPrawa()
        {
            foreach (KrawedzGrafu krawedz in Krawedzie)
            {
                if (krawedz.ZwrocPrzeciwnyWierzcholek(this).Pozycja.X > Pozycja.X)
                    return krawedz;
            }
            return null;
        }

        public KrawedzGrafu ZwrocKrawedzLewa()
        {
            foreach (KrawedzGrafu krawedz in Krawedzie)
            {
                if (krawedz.ZwrocPrzeciwnyWierzcholek(this).Pozycja.X < Pozycja.X)
                    return krawedz;
            }
            return null;
        }
    }
}
