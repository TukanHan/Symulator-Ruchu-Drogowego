using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public class KrawedzGrafu
    {
        public WierzcholekGrafu WierzcholekA { get; }
        public WierzcholekGrafu WierzcholekB { get; }

        public KrawedzGrafu(WierzcholekGrafu wierzcholekA, WierzcholekGrafu wierzcholekB)
        {
            WierzcholekA = WierzcholekGrafu.ZwrocMniejszyWierzcholek(wierzcholekA, wierzcholekB);
            WierzcholekB = WierzcholekGrafu.ZwrocWiekszyWierzcholek(wierzcholekA, wierzcholekB);
        }

        public double DlugoscKrawedzi()
        {
            return Math.Abs(WierzcholekA.Pozycja.X - WierzcholekB.Pozycja.X) + Math.Abs(WierzcholekA.Pozycja.Y - WierzcholekB.Pozycja.Y);
        }

        public WierzcholekGrafu ZwrocPrzeciwnyWierzcholek(WierzcholekGrafu wierzcholek)
        {
            if (WierzcholekA == wierzcholek)
                return WierzcholekB;
            else if (WierzcholekB == wierzcholek)
                return WierzcholekA;
            else
                throw new Exception("Brak połączonego wierzchołka");
        }

        public Relacja ZwrocRelacje()
        {
            if (WierzcholekA.Pozycja.X == WierzcholekB.Pozycja.X)
                return Relacja.Pionowe;
            else if (WierzcholekA.Pozycja.Y == WierzcholekB.Pozycja.Y)
                return Relacja.Poziome;
            else
                return Relacja.Brak;
        }

        public KrawedzGrafu UsunKrawedz()
        {
            WierzcholekA.Krawedzie.Remove(this);
            WierzcholekB.Krawedzie.Remove(this);

            return this;
        }

        public static KrawedzGrafu StworzDroge(WierzcholekGrafu wierzcholekA, WierzcholekGrafu wierzcholekB)
        {
            KrawedzGrafu nowaDroga = new KrawedzGrafu(wierzcholekA, wierzcholekB);
            wierzcholekA.Krawedzie.Add(nowaDroga);
            wierzcholekB.Krawedzie.Add(nowaDroga);

            return nowaDroga;
        }
    }
}
