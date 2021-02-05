using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA_LANG
{
    public class Degisken
    {
        private string isim;
        private double deger;

        public Degisken(string isim, double deger)
        {
            OUTPUT_PANEL.write(isim + " değişkeni oluşturuldu...");
            this.isim = isim;
            this.deger = deger;
        }

        public void degerAta(double yeniDeger)
        {
            this.deger = yeniDeger;
        }
        public double getDeger()
        {
            return this.deger;
        }
        public string toString()
        {
            return "Değişken Adı: " + isim + " Değişkenin Değeri: " + deger;
        }

        public string getIsim()
        {
            return this.isim;
        }

    }
}
