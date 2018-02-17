using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BinarneDatoteke
{
    class Program
    {
        static byte str2bin(string a) { //rocno, od desne proti levi, pazi da ni string vecji ali manjsi od 8
            int mask = 1;
            //      012345678901234567
            // a = "101010100101001010"
            //
            //      012
            // a = "101"
            //        |
            int start = a.Length - 1;
            int count = 0;
            int newByte = 0;

            while (count < 8 && start > 0) {
                if (a[start] == '1')
                    newByte += mask;

                mask *= 2;
                start--;
                count++;
            }

            return (byte)newByte;
        }

        static Dictionary<string, int> slovar = new Dictionary<string, int>();

        static void Encoder (string data) {
            int len = 1; //z kolkimi biti bomo pisali
            int numCodes = 1 << len; //koliko razlicnih simbolov s tolkimi biti

            for (int i = 0; i < data.Length; i++)
            {
                if(!slovar.ContainsKey(data[i].ToString())) {
                    slovar.Add(data[i].ToString(), slovar.Count);

                    if(slovar.Count > numCodes) { //velikost slovara preseze stevilo kod
                        len++; //povecamo koko bitov potrebujemo da zapisemo datoteko
                        numCodes = 1 << len;
                    }
                    //write to file len;
                }

            }
        }

        /*
         * Encoder: 0, 1, 10, 11, 100
         * 1, 0
         * 2, 1
         * 3, 2
         * 4, 3
         * 5, 4 spet povecamo stevilo bitov
         * 
         * 
         * Branje:
         * v vsakem vstopu v  dictionary, ko porabimo vse bite jih vedno povecamo za 1 npr pri 256 ves da gres iz 8 bitov na 9 bitov
         */

        static void Main(string[] args)
        {
            //pisanje tabele frekvenc
            byte[] dat = File.ReadAllBytes("ansi.txt");
            int[] freq = new int[256];

            foreach (byte bb in dat)
            {
                freq[bb]++;
            }

            List<byte> _b = new List<byte>();

            for(int i = 0; i < 256; i++) { //shranimo vse 4 bajte od inta v list in zapisemo v datoteko
                int t = freq[i];
                _b.Add((byte)(t & 255));
                t = t >> 8;
                _b.Add((byte)(t & 255));
                t = t >> 8;
                _b.Add((byte)(t & 255));
                t = t >> 8;
                _b.Add((byte)(t & 255));
                t = t >> 8;
            }

            File.WriteAllBytes("freq.bin", _b.ToArray());

            //za pisaje bit po bit samo posiljas bitev v funckijo in ko jih mas v 1 byto 8-em jih kar napises
            Encoder("1234567890");    

            byte[] ansi = File.ReadAllBytes("ansi.txt");
            byte[] utf = File.ReadAllBytes("utf8.txt");

            //LZW slovar, testirali bodo samo za vhodno ansi datoteko, ansi je default encoding
            Dictionary<string, int> s = new Dictionary<string, int>();

            for(int i = 0; i < 256; i++) { //ustvarimo LZW slovar
                s.Add(((char)i).ToString(), i);
            }
           
            foreach(byte n in ansi) {
                Console.Write(n + ",");
            }

            Console.WriteLine();
            Console.WriteLine();

            foreach (byte n in utf)
            {
                Console.Write(n + ",");
            }

            Console.WriteLine();
            Console.WriteLine();

            File.WriteAllBytes("newf.txt", ansi);
            File.WriteAllBytes("newf2.txt", utf); //ta je vecja, utf 8 ima 2 bajta za sumnike, ne dobimo isto besedilo
            File.WriteAllText("newf2.txt", "ĐŠČĆŽ", Encoding.Default); //dobimo ven isto besedilo, PAZI DA DAS ENCODING DEFAULT KO PISES DA OSTANE V ANSI

            //CE DELAS S STRINGI VEDNO UPORABI ANSI ENCODING!
            byte e = str2bin("101010100101001010");
            //File.WriteAllText("f1.bin", "11111111"); //8 bajtov
            byte a = Convert.ToByte("11111111", 2); //pretvori string v 1 byte

            string aa = "10101010101010110100100101010111010000101010101010";

            while(aa.Length > 8) { //jemati moramo po 8 znakov, po teh 8 znakov spremenimo v 1 bajt
                string bin = aa.Substring(0, 8);
                byte b = Convert.ToByte(bin, 2);
                Console.WriteLine(bin + " => " + b);
                aa = aa.Substring(8, aa.Length - 8);
            }

            Console.ReadLine();
            //File.WriteAllBytes("f2.bin", new byte[1] {255}); //1 bajt




        }
    }
}
