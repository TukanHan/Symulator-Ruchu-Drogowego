using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public enum Relacja { Poziome, Pionowe, Brak }

    public struct Punkt<T> where T : struct
    {
        public T X { get; set; }
        public T Y { get; set; }

        public Punkt(T x, T y)
        {
            X = x;
            Y = y;
        }

        public static T Odleglosc(Punkt<T> punktA, Punkt<T> punktB)
        {
            return Math.Abs((dynamic)punktA.X - punktB.X) + Math.Abs((dynamic)punktA.Y - punktB.Y);
        }

        public static Relacja ZwrocRelacje(Punkt<T> punktA, Punkt<T> punktB)
        {
            if (punktA.X.Equals(punktB.X))
                return Relacja.Pionowe;
            if (punktA.Y.Equals(punktB.Y))
                return Relacja.Poziome;

            return Relacja.Brak;
        }

        public static Punkt<T> ZwrocPozycjeMniejszego(Punkt<T> punktA, Punkt<T> punktB)
        {
            Relacja relacja = ZwrocRelacje(punktA, punktB);
            if (relacja == Relacja.Poziome)
                return (dynamic)punktA.X < punktB.X ? punktA : punktB;
            else if(relacja == Relacja.Pionowe)
                return (dynamic)punktA.Y < punktB.Y ? punktA : punktB;

            throw new Exception("Brak relacji");
        }

        public static Punkt<T> ZwrocPozycjeWiekszego(Punkt<T> punktA, Punkt<T> punktB)
        {
            Relacja relacja = ZwrocRelacje(punktA, punktB);
            if (relacja == Relacja.Poziome)
                return (dynamic)punktA.X > punktB.X ? punktA : punktB;
            else if (relacja == Relacja.Pionowe)
                return (dynamic)punktA.Y > punktB.Y ? punktA : punktB;

            throw new Exception("Brak relacji");
        }

        public static  explicit operator Punkt<int>(Punkt<T> punktWejsciowy)
        {
            return new Punkt<int>((int)((dynamic)punktWejsciowy.X), (int)((dynamic)punktWejsciowy.Y));
        }

        public static explicit operator Punkt<double>(Punkt<T> punktWejsciowy)
        {
            return new Punkt<double>((double)((dynamic)punktWejsciowy.X), (double)((dynamic)punktWejsciowy.Y));
        }
    }
}
