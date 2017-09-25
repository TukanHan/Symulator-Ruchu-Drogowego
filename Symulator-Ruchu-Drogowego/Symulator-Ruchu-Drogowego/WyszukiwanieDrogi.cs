using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symulator_Ruchu_Drogowego
{
    class WyszukiwanieDrogi
    {
        private class Wezel
        {
            public WierzcholekGrafu Klucz { get; }
            public Wezel Poprzednik { get; set; }
            public Double Dystans { get; set; } = Double.MaxValue;
            public bool CzyDodawany { get; set; } = false;

            public Wezel(WierzcholekGrafu klucz)
            {
                Klucz = klucz;
            }
        }

        private List<WierzcholekGrafu> infrastruktura;

        public WyszukiwanieDrogi(List<WierzcholekGrafu> infrastruktura)
        {
            this.infrastruktura = infrastruktura;
        }

        public bool CzyGrafSpojny()
        {
            Dictionary<WierzcholekGrafu, bool>  listaOdwiedzen = new Dictionary<WierzcholekGrafu, bool>();
            for (int i = 0; i < infrastruktura.Count; ++i)
                listaOdwiedzen.Add(infrastruktura[i], false);

            PrzeszukiwanieDFS(listaOdwiedzen);

            foreach (var odwiedzony in listaOdwiedzen)
                if (!odwiedzony.Value)
                    return false;

            return true;
        }

        private void PrzeszukiwanieDFS(Dictionary<WierzcholekGrafu, bool> listaOdwiedzen)
        {
            Stack<WierzcholekGrafu> stos = new Stack<WierzcholekGrafu>();
            stos.Push(infrastruktura[0]);
            while (stos.Count > 0)
            {
                WierzcholekGrafu wierzcholek = stos.Pop();

                if (!listaOdwiedzen[wierzcholek])
                {
                    listaOdwiedzen[wierzcholek] = true;

                    foreach(KrawedzGrafu poloczenie in wierzcholek.Krawedzie)                   
                        stos.Push(poloczenie.ZwrocPrzeciwnyWierzcholek(wierzcholek));                    
                }
            }
        }

        public List<WierzcholekGrafu> ZwrocScierzke(WierzcholekGrafu start, WierzcholekGrafu koniec)
        {
            List<Wezel> wezly = new List<Wezel>();
            for (int i = 0; i < infrastruktura.Count; ++i)
            {
                wezly.Add(new Wezel(infrastruktura[i]));
            }

            wezly.Find(obiekt => obiekt.Klucz == start).Dystans = 0;

            List<Wezel> kolejka = new List<Wezel>();
            kolejka.Add(wezly.Find(obiekt => obiekt.Klucz == start));
            wezly.Find(obiekt => obiekt.Klucz == start).CzyDodawany = true;
            while (kolejka.Count > 0)
            {
                Wezel aktualnyWezel = kolejka.OrderBy(obiekt => obiekt.Dystans).First();
                if (aktualnyWezel.Klucz == koniec)
                    break;

                foreach (KrawedzGrafu polaczenie in aktualnyWezel.Klucz.Krawedzie)
                {
                    Wezel sasiedniWezel = wezly.Find(obiekt => obiekt.Klucz == polaczenie.ZwrocPrzeciwnyWierzcholek(aktualnyWezel.Klucz));
                    if (sasiedniWezel.Dystans > aktualnyWezel.Dystans + polaczenie.DlugoscKrawedzi())
                    {
                        sasiedniWezel.Dystans = aktualnyWezel.Dystans + polaczenie.DlugoscKrawedzi();
                        sasiedniWezel.Poprzednik = aktualnyWezel;
                    }
                    if (!sasiedniWezel.CzyDodawany)
                    {
                        kolejka.Add(sasiedniWezel);
                        sasiedniWezel.CzyDodawany = true;
                    }
                }

                kolejka.Remove(aktualnyWezel);
            }
            return ZnajdzTraseZPoprzednikow(wezly, start, koniec);
        }

        //metoda która z poprzedników węzła potrafi znaleźć trasę
        private List<WierzcholekGrafu> ZnajdzTraseZPoprzednikow(List<Wezel> poprzednicy, WierzcholekGrafu start, WierzcholekGrafu koniec)
        {
            List<WierzcholekGrafu> trasa = new List<WierzcholekGrafu>();

            Wezel nastepny = poprzednicy.Find(obiekt => obiekt.Klucz == koniec);
            while (nastepny.Poprzednik != null)
            {
                trasa.Add(nastepny.Klucz);
                nastepny = nastepny.Poprzednik;
            }

            return trasa.Reverse<WierzcholekGrafu>().ToList();
        }     
    }
}
