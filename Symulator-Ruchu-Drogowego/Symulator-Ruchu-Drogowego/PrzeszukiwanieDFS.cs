using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    public class PrzeszukiwanieDFS
    {
        private List<WierzcholekGrafu> infrastruktura;

        public PrzeszukiwanieDFS(List<WierzcholekGrafu> infrastruktura)
        {
            this.infrastruktura = infrastruktura;
        }

        public bool CzyGrafSpojny()
        {
            Dictionary<WierzcholekGrafu, bool> listaOdwiedzen = new Dictionary<WierzcholekGrafu, bool>();
            for (int i = 0; i < infrastruktura.Count; ++i)
                listaOdwiedzen.Add(infrastruktura[i], false);

            PrzeszukajGraf(listaOdwiedzen);

            foreach (var odwiedzony in listaOdwiedzen)
                if (!odwiedzony.Value)
                    return false;

            return true;
        }

        private void PrzeszukajGraf(Dictionary<WierzcholekGrafu, bool> listaOdwiedzen)
        {
            Stack<WierzcholekGrafu> stos = new Stack<WierzcholekGrafu>();
            stos.Push(infrastruktura[0]);
            while (stos.Count > 0)
            {
                WierzcholekGrafu wierzcholek = stos.Pop();

                if (!listaOdwiedzen[wierzcholek])
                {
                    listaOdwiedzen[wierzcholek] = true;

                    foreach (KrawedzGrafu poloczenie in wierzcholek.Krawedzie)
                        stos.Push(poloczenie.ZwrocPrzeciwnyWierzcholek(wierzcholek));
                }
            }
        }
    }
}
