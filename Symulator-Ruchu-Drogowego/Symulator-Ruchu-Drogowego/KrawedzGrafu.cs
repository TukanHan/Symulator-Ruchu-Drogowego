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
            //Wierzchołek punktem bliższy 0,0 jest mniejszy
            WierzcholekA = WierzcholekGrafu.ZwrocMniejszyWierzcholek(wierzcholekA, wierzcholekB);
            WierzcholekB = WierzcholekA == wierzcholekA ? wierzcholekB : wierzcholekA;
        }

        public double DlugoscKrawedzi()
        {
            return Math.Abs(WierzcholekA.Pozycja.X - WierzcholekB.Pozycja.X) + Math.Abs(WierzcholekA.Pozycja.Y - WierzcholekB.Pozycja.Y);
        }

        public WierzcholekGrafu ZwrocPrzeciwnyWierzcholek(WierzcholekGrafu wierzcholek)
        {
            if (WierzcholekA == wierzcholek)
                return WierzcholekB;
            if (WierzcholekB == wierzcholek)
                return WierzcholekA;

            throw new Exception("Brak połączonego wierzchołka");
        }

        public Relacja ZwrocRelacje()
        {
            if (WierzcholekA.Pozycja.X == WierzcholekB.Pozycja.X)
                return Relacja.Pionowe;
            if (WierzcholekA.Pozycja.Y == WierzcholekB.Pozycja.Y)
                return Relacja.Poziome;

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
