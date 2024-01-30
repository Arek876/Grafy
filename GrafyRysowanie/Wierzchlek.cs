using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrafyRysowanie
{
    internal class Wierzchlek
    {
        public Point Polozenie {  get; }
        public Int32 Id {  get; }

        private List<Wierzchlek> nastpniki = new List<Wierzchlek>();
        public List<Wierzchlek> Nastpniki 
        { 
            get
            {
                return nastpniki;
            }
        }

        private static int newId = 0;
        private static int NewId
        {
            get { return ++newId; }
        }

        public Wierzchlek(Point Polozenie)
        { 
            this.Polozenie = Polozenie;
            this.Id = NewId;
        }

        public static void ResetGraf()
        {
            newId = 0;
        }
        internal int Odleglosc(Point p)
        {
            return (int)Math.Sqrt(Math.Pow(Polozenie.X-p.X, 2) + Math.Pow(Polozenie.Y-p.Y, 2));
        }
    }
}
