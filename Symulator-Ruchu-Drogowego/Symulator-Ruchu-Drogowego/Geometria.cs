using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (przesuniecie == 0)
                return new Punkt<double>(0, 0);

            double kawalki = ObliczOdlegloscPomiedzy() / przesuniecie;

            if (kawalki == 0)
                return new Punkt<double>(0, 0);

            return new Punkt<double>(-(obiektB.X - obiektA.X) / kawalki, -(obiektB.Y - obiektA.Y) / kawalki);
        }
    }
}
