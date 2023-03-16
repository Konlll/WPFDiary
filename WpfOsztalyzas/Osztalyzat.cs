using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfOsztalyzas
{
    public class Osztalyzat
    {
        String nev;
        String datum;
        String tantargy;
        int jegy;

        public Osztalyzat(string nev, string datum, string tantargy, int jegy)
        {
            this.nev = nev;
            this.datum = datum;
            this.tantargy = tantargy;
            this.jegy = jegy;
        }

        public string Nev
        {
            get
            {
                return nev;
            }
            set
            {
                this.nev = value;
            }
        }
        public string Datum { get => datum;  }
        public string Tantargy { get => tantargy; }
        public int Jegy { get => jegy; }
        public string CsaladiNev { get => nev.Split(' ')[0]; }

        public string ForditottNev()
        {
            return $"{this.nev.Split(' ')[1]} {this.nev.Split(' ')[0]}";
        }
    }
        //todo Bővítse az osztályt! Készítsen CsaladiNev néven property-t, ami a névből a családi nevet adja vissza. Feltételezve, hogy a névnek csak az első tagja az. - KÉSZ

        //todo Készítsen metódust ForditottNev néven, ami a két tagból álló nevek esetén megfordítja a névtagokat. Pld. Kiss Ádám => Ádám Kiss - KÉSZ
}
