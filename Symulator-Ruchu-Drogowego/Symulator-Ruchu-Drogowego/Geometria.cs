using System;

namespace Symulator_Ruchu_Drogowego
{
    public class Geometria
    {
        private Punkt<double> obiektA;
        private Punkt<double> obiektB;

        public Geometria(Punkt<double> obiektA, Punkt<double> obiektB)
        {
            this.obiektA = obiektA;
            this.obiektB = obiektB;
        }

        public Punkt<double> ObliczPozycjePomiedzy()
        {
            return new Punkt<double>((obiektA.X + obiektB.X) / 2, (obiektA.Y + obiektB.Y) / 2);
        }

        public double ObliczOdlegloscPomiedzy()
        {
            return Math.Sqrt(Math.Pow(obiektA.X - obiektB.X, 2) + Math.Pow(obiektA.Y - obiektB.Y, 2));
        }

        public double ObliczKatPomiedzy()
        {
            return Math.Atan2(obiektB.Y - obiektA.Y, obiektB.X - obiektA.X) * (180 / Math.PI) + 90;
        }

        public Punkt<double> ObliczWektorPrzesuniecia(double przesuniecie)
        {
            double kawalki = ObliczOdlegloscPomiedzy() / przesuniecie;

            if (kawalki == 0)
                return Punkt<double>.PustyPunkt;

            return new Punkt<double>(-(obiektB.X - obiektA.X) / kawalki, -(obiektB.Y - obiektA.Y) / kawalki);
        }
    }
}
