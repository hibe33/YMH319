using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RA_LANG
{
    public partial class Form1 : Form
    {
        public static TextBox output_area;
        public static TextBox input_area;

        public Form1()
        {
            InitializeComponent();
            output_area = textBox2;
            input_area = textBox1;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();

            LineControl(textBox1);


        }

        public static void printActiveVariables()
        {
            foreach(Degisken d in degiskenListesi)
            {
                OUTPUT_PANEL.write(d.toString());
            }
        }

        public static ArrayList degiskenListesi = new ArrayList();

        public static void LineControl(TextBox editor)
        {
            for (int lineIndex = 0; lineIndex < editor.Lines.Length; lineIndex++)
            {
                String line = editor.Lines[lineIndex];

                String[] wordList = line.Split(' ');
                try { 
                for (int wordIndex = 0; wordIndex < wordList.Length; wordIndex++)
                {
                    if (wordList[wordIndex].ToString() == SYNTAX.NUMBER)
                    {
                        OUTPUT_PANEL.write("Değişken belirtimi algılandı...");
                        if (wordList[wordIndex + 2] == SYNTAX.ATAMA)
                        {
                            OUTPUT_PANEL.write("Atama operatörü algılandı...");
                            string degiskenAdi = wordList[wordIndex + 1];
                            double degiskenDegeri;
                            if (wordList[wordIndex + 4] == SYNTAX.END_OF_LINE && Double.TryParse(wordList[wordIndex + 3], out degiskenDegeri))
                            {
                                Degisken degisken = new Degisken(degiskenAdi, degiskenDegeri);
                                hafizayaKaydet(degisken);
                            }

                            else if (wordList[wordIndex + 4] == SYNTAX.END_OF_LINE && isOperantion(wordList[wordIndex + 3]))
                            {
                                try
                                {
                                    Degisken degisken = new Degisken(degiskenAdi, dortIslem(wordList[wordIndex + 3], lineIndex));
                                    hafizayaKaydet(degisken);
                                }
                                catch
                                {
                                    OUTPUT_PANEL.write(ERRORS.INVALID_OPERATION + lineIndex);
                                }
                            }
                        }
                    }
                    else if (wordList[wordIndex].ToString() == SYNTAX.YAZDIR)
                    {
                        
                        if (memoryIsContain(wordList[wordIndex + 1].ToString()))
                        {
                            OUTPUT_PANEL.write(getDegiskenFromMemory(wordList[wordIndex + 1]).toString());
                        }
                        else if(isOperantion(wordList[wordIndex + 1]))
                        {
                            try
                            {                                
                                OUTPUT_PANEL.write(dortIslem(wordList[wordIndex + 1].ToString(), lineIndex).ToString());
                            }
                            catch
                            {
                                OUTPUT_PANEL.write(ERRORS.INVALID_OPERATION + lineIndex);
                            }

                        }
                    }
                    else if(wordList[wordList.Length-1].ToString() != ";")
                    {
                        OUTPUT_PANEL.write(ERRORS.END_OF_LINE_EXPECTING + lineIndex);
                    }
                }
                }
                catch
                {
                    StopWithError(ERRORS.INVALID_SYNTAX);
                    break;
                }
            }
        }

        public static void EndOfLineControl()
        {

        }

        public static double hesapla(ArrayList sayilar, ArrayList operatorler, int satir)
        {
            double result;

            for (int i = 0; i < sayilar.Count; i++)
            {
                double _ = 0;
                if (!Double.TryParse(sayilar[i].ToString(), out _))
                {
                    if (memoryIsContain(sayilar[i].ToString()))
                    {
                        foreach (Degisken degisken in degiskenListesi)
                        {
                            if (degisken.getIsim() == sayilar[i].ToString())
                            {
                                sayilar[i] = degisken.getDeger().ToString();
                            }
                        }
                    }

                    else
                    {
                        StopWithError(ERRORS.INVALID_VARIABLE + satir);
                    }
                }
            }

            while (sayilar.Count != 1)
            {
                for (int index = 0; index < operatorler.Count; index++)
                {
                    if (operatorler[index].ToString() == "*" || operatorler[index].ToString() == "/")
                    {
                        sayilar[index] = islemYap(operatorler[index].ToString(), sayilar[index].ToString(), sayilar[index + 1].ToString());
                        sayilar.RemoveRange(index + 1, 1);
                        operatorler.RemoveRange(index, 1);
                    }

                }
                for (int index = 0; index < operatorler.Count; index++)
                {
                    if (operatorler[index].ToString() == "+" || operatorler[index].ToString() == "-")
                    {
                        sayilar[index] = islemYap(operatorler[index].ToString(), sayilar[index].ToString(), sayilar[index + 1].ToString());
                        sayilar.RemoveRange(index + 1, 1);
                        operatorler.RemoveRange(index, 1);
                    }
                }                
            }
            result = Convert.ToDouble(sayilar[0]);
            return result;
        }

        public static double islemYap(string islem, string deger1, string deger2)
        {
            double x = Convert.ToDouble(deger1);
            double y = Convert.ToDouble(deger2);
            if (islem == "+") { return x + y; }
            else if(islem == "-") { return x - y; }
            else if (islem == "*") { return x * y; }
            else if (islem == "/") { return x / y; }
            else { return Double.NaN; }
        }

        public static string arrayToString(ArrayList list)
        {
            string result ="";

            for (int i = 0; i < list.Count; i++)
            {
                if(i == list.Count - 1)
                {
                    result += list[i];
                    return result;
                }
                result += list[i] + ",";
            } 
            return result;
        }
        public static ArrayList stringToArray(string deger)
        {
            ArrayList result = new ArrayList();
            foreach(var x in deger)
            {
                result.Add(x);
            }
            return result;
        }

        public static bool booleIslemYap(string islem, string deger1, string deger2)
        {
            bool x = Convert.ToBoolean(deger1);
            bool y = Convert.ToBoolean(deger2);
            if (islem == "AND") { return x & y; }
            else if (islem == "OR") { return x | y; }
            else { return false; }
        }

        public static bool karsilastir(string islem, string deger1, string deger2)
        {
            double x = Convert.ToDouble(deger1);
            double y = Convert.ToDouble(deger2);
            if (islem == "<") { return x < y; }
            else if (islem == "<=") { return x <= y; }
            else if (islem == "==") { return x == y; }
            else if (islem == ">") { return x > y; }
            else if (islem == ">=") { return x >= y; }
            else { return false; }
        }

        public static double dortIslem(string denklem, int satir)
        {
            string text = denklem;
            ArrayList operatorler = new ArrayList() { '+', '-', '*', '/' };
            ArrayList sayilar = new ArrayList();
            ArrayList islemler = new ArrayList();
            string _tempNumber = "";
            for (int index = 0; index < text.Length; index++)
            {
                if (operatorler.Contains(text[index]))
                {
                    islemler.Add(text[index]);                   
                    sayilar.Add(_tempNumber);
                    _tempNumber = "";
                }
                else
                {                                  
                    _tempNumber += text[index];
                }
            }
            sayilar.Add(_tempNumber);
            return hesapla(sayilar, islemler, satir);
        }

        public static bool memoryIsContain(string degisken_adi)
        {
            foreach(Degisken degisken in degiskenListesi)
            {
                if(degisken.getIsim() == degisken_adi)
                {
                    return true;
                }
            }
            return false;
        }        

        public static void StopWithError(string error)
        {
            degiskenListesi.Clear();
            OUTPUT_PANEL.write(error);
            OUTPUT_PANEL.write("PROGRAM DURDURULDU!");
        }

        public static bool isOperantion(string denklem)
        {
            ArrayList operatorler = new ArrayList() { '+', '-', '*', '/' };
            foreach(char op in operatorler)
            {
                if (denklem.Contains(op))
                {
                    return true;
                }
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printActiveVariables();

        }

        public static bool hafizayaKaydet(Degisken degisken)
        {
            foreach (Degisken d in degiskenListesi)
            {
                if (d.getIsim() == degisken.getIsim())
                {
                    OUTPUT_PANEL.write(degisken.getIsim() + "değişkeni tekrardan atandı....");
                    d.degerAta(degisken.getDeger());
                    return true;
                }
            }
            degiskenListesi.Add(degisken);
            return true;
            
        }

        public static Degisken getDegiskenFromMemory(string name)
        {
            foreach(Degisken d in degiskenListesi)
            {
                if(d.getIsim() == name)
                {
                    return d;
                }
            }
            return null;
        }

    }
}
