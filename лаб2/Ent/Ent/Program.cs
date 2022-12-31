using System;
using System.Text;

namespace Ent
{
    class Program
    {
        //Math.Log(14,2); -- 2 is base, 14 is number
        //byte[] hello = { 1, 0, 0, 1, 1, 1, 1, 0, 0 };
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //------------the first task
            string message = "mandag tirsdag onsdag torsdag fredag ​​lørdag søndag abcdefghijklmnopqrstuvwxyzæøå";
            //string norwegianAlphabetLower = "abcdefghijklmnopqrstuvwxyzæøå";
            //string norwegianAlphabetUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ";
            string norwegianAlphabetRepeat = "";

            double entropyForNorwegian = 0d;


            entropyForNorwegian = NorwegianEntropy(message, norwegianAlphabetRepeat, entropyForNorwegian);
            Console.WriteLine();
            Console.WriteLine("Энтропия для норвежского текста: " + entropyForNorwegian);
            Console.WriteLine();


            string SlavicMessage = "понедѣльникъ вторникъ среда четвергъ пятница суббота ​воскресеньѣ​ абвгдежsзиiклмнопрстуфхцчшщъыьѣюѥѧѫѩѭѯѱѳѵ";
            //string slavicAlphabetLower = "абвгдежsзиiклмнопрстуфхцчшщъыьѣюѥѧѫѩѭѯѱѳѵ";
            //string slavicAlphabetUpper = "АБВГДЕЖЅЗИІКЛМНОПРСТУФХѠЦЧШЩЪЫЬѢЮѤѦѪѨѬѮѰѲѴ";
            string slavicAlphabetRepeat = "";

            double entropyForSlavic = 0d;


            entropyForSlavic = SlavicEntropy(SlavicMessage, slavicAlphabetRepeat, entropyForSlavic);
            Console.WriteLine();
            Console.WriteLine("Энтропия для славянского текста: " + entropyForSlavic);
            Console.WriteLine();


            //------------the second task

            #region Encoding
                var enc1251 = Encoding.GetEncoding(1251);
            
                //if (string.IsNullOrEmpty(message))
                
                string bin = string.Empty;
                var txt1251 = enc1251.GetBytes(message.ToCharArray());
                foreach (var ch in txt1251)
                {
                    //Console.Write($"{ch} ");
                    bin += Convert.ToString(ch, 2);
                }
                Console.WriteLine($"\n{bin}");
            
            #endregion

            //byte[] byteText = { 1, 0 };
            string byteTextRepeat = "";

            double entropyForByteText = 0d;

            //string ConvertByteText = System.Text.Encoding.Default.GetString(byteText);

            entropyForByteText = ByteTextEntropy(bin, byteTextRepeat, entropyForByteText);
            Console.WriteLine();
            Console.WriteLine("Энтропия бинарного алфавита: " + entropyForByteText);
            Console.WriteLine();


            //------------the third task
            string fullNameInNorwegian = "Shumova Yelizaveta Igorevna";
            string fullNameInSlavic = "Шоумова Ѥлисавета Игоревна";
            string fullNameInBinary = "0101001101101000011101010110110101101111011101100110000101011001011001010110110001101001011110100110000101110110011001010111010001100001010010010110011101101110111001001100101011101100110111001100001";

            Console.WriteLine("Количество информации в сообщении на основе норвежского алфавита: " + fullNameInNorwegian.Length * entropyForNorwegian + " бит");
            Console.WriteLine("Количество информации в сообщении на основе славянского алфавита: " + fullNameInSlavic.Length * entropyForSlavic + " бит");
            Console.WriteLine("Количество информации в сообщении на основе бинарного алфавита: " + fullNameInBinary.Length * entropyForByteText + " бит");
            Console.WriteLine();
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для бинарного алфавита: " + fullNameInBinary.Length * entropyForByteText + " бит");


            //------------the fourth task
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("С вероятностью ошибочной передачи 0.1: ");
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для норвежского алфавита: " + fullNameInNorwegian.Length * (1 - (-0.1 * Math.Log(0.1, 2) - 0.9999 * Math.Log(0.9999, 2)))  + " бит");
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для славянского алфавита: " + fullNameInSlavic.Length * (1 - (-0.1 * Math.Log(0.1, 2) - 0.9999 * Math.Log(0.9999, 2)))  + " бит");
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для бинарного алфавита: " + fullNameInBinary.Length * (1 - (-0.00001 * Math.Log(0.00001, 2) - 0.9999 * Math.Log(0.9999, 2)))  + " бит");
            //Console.WriteLine(27*(1-(-0.0001 * Math.Log(0.0001, 2) - 0.9999 * Math.Log(0.9999, 2))));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("С вероятностью ошибочной передачи 0.5: ");
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для норвежского алфавита: " + fullNameInNorwegian.Length * (1 - (-0.5 * Math.Log(0.5, 2) - 0.5 * Math.Log(0.5, 2)))  + " бит");
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для славянского алфавита: " + fullNameInSlavic.Length * (1 - (-0.5 * Math.Log(0.5, 2) - 0.5 * Math.Log(0.5, 2)))  + " бит");
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для бинарного алфавита: " + fullNameInBinary.Length * (1 - (-0.5 * Math.Log(0.5, 2) - 0.5 * Math.Log(0.5, 2)))  + " бит");
            //Console.WriteLine(-0.5 * Math.Log(0.5, 2) - 0.5 * Math.Log(0.5, 2));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("С вероятностью ошибочной передачи 1: ");
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для норвежского алфавита: " + fullNameInNorwegian.Length * (1 - (-0.9999 * Math.Log(0.9999, 2) - 0.000001 * Math.Log(0.000001, 2)))  + " бит");
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для славянского алфавита: " + fullNameInSlavic.Length * (1 - (-0.9999 * Math.Log(0.9999, 2) - 0.000001 * Math.Log(0.000001, 2)))  + " бит");
            Console.WriteLine("Количество информации в сообщении в кодах ASCII для бинарного алфавита: " + fullNameInBinary.Length * (1 - (-0.9999 * Math.Log(0.9999, 2) - 0.000001 * Math.Log(0.000001, 2)))  + " бит");
            //Console.WriteLine(-1 * Math.Log(1, 2) - 0 * Math.Log(0, 2));

        }
        public static double NorwegianEntropy(string message, string norwegianAlphabetRepeat, double entropyForNorwegian)
        {
            for (int i = 0; i < message.Length; i++)
            {
                int repeats = 0;
                bool repeatchar = false;
                bool theSameLetter = false;

                if (!theSameLetter)
                {

                    for (int j = 0; j < message.Length; j++)
                    {
                        if (message[i] == message[j])
                        {
                            repeatchar = true;
                            repeats++;
                        }
                    }
                }
                else
                {
                    continue;
                }

                for (int k = 0; k < norwegianAlphabetRepeat.Length; k++)
                {
                    if (message[i] == norwegianAlphabetRepeat[k])
                    {
                        theSameLetter = true;
                        break;
                    }
                }

                if (repeatchar)
                {
                    norwegianAlphabetRepeat = norwegianAlphabetRepeat + message[i];
                }


                if (!theSameLetter)
                {
                    entropyForNorwegian = entropyForNorwegian + ((-1) * ((double)repeats / (double)message.Length) * Math.Log((double)repeats / (double)message.Length, 2));
                    Console.WriteLine("Symbol " + "'" + message[i] + "'" + " repeats " + repeats + " time(s) ");
                    Console.WriteLine("Symbol's " + "'" + message[i] + "'" + " probability is " + (double)repeats / (double)message.Length);
                }
            }
            return entropyForNorwegian;
        }
        public static double SlavicEntropy(string SlavicMessage, string slavicAlphabetRepeat, double entropyForSlavic)
        {
            for (int i = 0; i < SlavicMessage.Length; i++)
            {
                int repeats = 0;
                bool repeatchar = false;
                bool theSameLetter = false;

                if (!theSameLetter)
                {

                    for (int j = 0; j < SlavicMessage.Length; j++)
                    {
                        if (SlavicMessage[i] == SlavicMessage[j])
                        {
                            repeatchar = true;
                            repeats++;
                        }
                    }
                }
                else
                {
                    continue;
                }

                for (int k = 0; k < slavicAlphabetRepeat.Length; k++)
                {
                    if (SlavicMessage[i] == slavicAlphabetRepeat[k])
                    {
                        theSameLetter = true;
                        break;
                    }
                }

                if (repeatchar)
                {
                    slavicAlphabetRepeat = slavicAlphabetRepeat + SlavicMessage[i];
                }


                if (!theSameLetter)
                {
                    entropyForSlavic = entropyForSlavic + ((-1) * ((double)repeats / (double)SlavicMessage.Length) * Math.Log((double)repeats / (double)SlavicMessage.Length, 2));
                    Console.WriteLine("Символ " + "'" + SlavicMessage[i] + "'" + " повторяется " + repeats + " раз/раза");
                    Console.WriteLine("Вероятность " + "'" + SlavicMessage[i] + "'" + " символа - " + (double)repeats / (double)SlavicMessage.Length);
                }
            }
            return entropyForSlavic;
        }
        public static double ByteTextEntropy(string byteText, string byteTextRepeat, double entropyForByteText)
        {
            for (int i = 0; i < byteText.Length; i++)
            {
                int repeats = 0;
                bool repeatchar = false;
                bool theSameLetter = false;

                if (!theSameLetter)
                {

                    for (int j = 0; j < byteText.Length; j++)
                    {
                        if (byteText[i] == byteText[j])
                        {
                            repeatchar = true;
                            repeats++;
                        }
                    }
                }
                else
                {
                    continue;
                }

                for (int k = 0; k < byteTextRepeat.Length; k++)
                {
                    if (byteText[i] == byteTextRepeat[k])
                    {
                        theSameLetter = true;
                        break;
                    }
                }

                if (repeatchar)
                {
                    byteTextRepeat = byteTextRepeat + byteText[i];
                }


                if (!theSameLetter)
                {
                    entropyForByteText = entropyForByteText + ((-1) * ((double)repeats / (double)byteText.Length) * Math.Log((double)repeats / (double)byteText.Length, 2));
                }
            }
            return entropyForByteText;
        }
    }
}
