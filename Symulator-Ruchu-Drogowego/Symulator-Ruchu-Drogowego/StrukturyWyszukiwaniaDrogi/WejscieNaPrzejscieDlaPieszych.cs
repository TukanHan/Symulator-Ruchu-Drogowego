using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    class WejscieNaPrzejscieDlaPieszych : IWejscieNaElement<Pieszy>
    {
        private PrzejscieDlaPieszych przejscieDlaPieszych;
        private WierzcholekGrafu przeciwnyWierzcholek;

        private List<Pieszy> piesiPrzezPasy = new List<Pieszy>();

        public WejscieNaPrzejscieDlaPieszych(PrzejscieDlaPieszych przejscieDlaPieszych, WierzcholekGrafu przeciwnyWierzcholek)
        {
            this.przejscieDlaPieszych = przejscieDlaPieszych;
            this.przeciwnyWierzcholek = przeciwnyWierzcholek;
        }

        public bool CzyMogeWejsc(Pieszy pieszy)
        {
            if (pieszy.ObecnaPozycja == przeciwnyWierzcholek)
            {
                if (przejscieDlaPieszych.CzyMogeWejsc(pieszy))
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        public void Wejdz(Pieszy pieszy)
        {
            if(pieszy.ObecnaPozycja == przeciwnyWierzcholek)
            {
                przejscieDlaPieszych.Wjedz(pieszy);
                piesiPrzezPasy.Add(pieszy);
            }
        }

        public void Wyjdz(Pieszy pieszy)
        {
            if(piesiPrzezPasy.Find(obiekt => obiekt == pieszy) != null)
            {
                przejscieDlaPieszych.Wyjedz(pieszy);
                piesiPrzezPasy.Remove(pieszy);
            }
        }
    }
}