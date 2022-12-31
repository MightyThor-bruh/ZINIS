using System;
using System.Linq;
using System.Text;

namespace Base64_XOR
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //-----------------------task1
            Console.WriteLine("--------------------Task1--------------------------");
            string inputText = "mandag tirsdag onsdag torsdag fredag";
            //string outputTextTobase64;

            Console.WriteLine($"Original text: {inputText}");
            //byte[] asciiInputText = Encoding.ASCII.GetBytes(inputText);
            //outputTextTobase64 = Convert.ToBase64String(asciiInputText);
            string inputTextInBase64 = Base64Encode(inputText);
            Console.WriteLine($"Base64 text: {inputTextInBase64}");
            Console.WriteLine();


            //-----------------------task2
            Console.WriteLine("--------------------Task2-----------------------------");
            double norwegianHartleyEntropy = Math.Log(29, 2);
            Console.WriteLine($"Hartley's entropy of Norwegian: {norwegianHartleyEntropy}");


            string AlphabetRepeat = "";
            double entropyForNorwegian = 0d;

            entropyForNorwegian = ShennonEntropy(inputText, AlphabetRepeat, entropyForNorwegian);
            Console.WriteLine("Shennon's entropy of Norwegian text: " + entropyForNorwegian);

            double redundancy = (1 - entropyForNorwegian / norwegianHartleyEntropy) * 100;
            Console.WriteLine($"Redundancy of Norwegian alphabet: {redundancy}%");
            Console.WriteLine();

            //--

            double norwegianBase64HartleyEntropy = Math.Log(29, 2);
            Console.WriteLine($"Hartley's entropy of Norwegian base64: {norwegianHartleyEntropy}");

            string AlphabetBase64Repeat = "";
            double entropyBase64ForNorwegian = 0d;

            entropyBase64ForNorwegian = ShennonEntropy(inputTextInBase64, AlphabetBase64Repeat, entropyBase64ForNorwegian);
            Console.WriteLine("Shennon's entropy of Norwegian base64 text: " + entropyBase64ForNorwegian);

            double base64Redundancy = (1 - entropyBase64ForNorwegian / norwegianBase64HartleyEntropy) * 100;
            Console.WriteLine($"Redundancy of Norwegian base64 alphabet: {base64Redundancy}%");
            Console.WriteLine();


            //-----------------------task3
            Console.WriteLine("--------------------Task3--------------------------");
            string name = "Elizaveta";
            string encodingName = EncodingToBytes(name);
            string surname = "Shumova";
            string encodingSurname = EncodingToBytes(surname);

            Console.WriteLine($"Input name: {name}");
            Console.WriteLine($"Name in bytes: {encodingName}");
            Console.WriteLine($"Input surname: {surname}");
            Console.WriteLine($"Surname in bytes: {encodingSurname}");
            Console.WriteLine();



            if(encodingSurname.Length > encodingName.Length)
            {
                for(int i = encodingName.Length; i < encodingSurname.Length; i++)
                {
                    encodingName += "0";
                }
            }
            else if(encodingSurname.Length < encodingName.Length)
            {
                for(int i = encodingSurname.Length; i < encodingName.Length; i++)
                {
                    encodingSurname += "0";
                }
            }
            Console.WriteLine($"Updating bytes with nulls(name): {encodingName}");
            Console.WriteLine($"Updating bytes with nulls(surn): {encodingSurname}");
            Console.WriteLine();

            long longname = Convert.ToInt64(encodingName, 2);
            long longsurname = Convert.ToInt64(encodingSurname, 2);
            //Console.WriteLine(longname);
            //Console.WriteLine(longsurname);
            //Console.WriteLine();
            //Console.WriteLine(Convert.ToString(intname, 2));
            //Console.WriteLine(Convert.ToString(intsurname, 2));

            
            //long result = longname ^ longsurname;

            //Console.WriteLine($"Name XOR surname result: {Convert.ToString(result, 2)}");
            
            //long wowresult = longname ^ longsurname ^ longsurname;
            //Console.WriteLine($"Name XOR surname XOR surname result: {Convert.ToString(wowresult, 2)}");

            //-------------------------------------------------------------------------------------------
            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            string nameInBase64 = Base64Encode(name);
            string encodingNameInBase64 = EncodingToBytes(nameInBase64);
            string surnameInBase64 = Base64Encode(surname);
            string encodingSurnameInBase64 = EncodingToBytes(surnameInBase64);

            Console.WriteLine($"Input name in Base64: {nameInBase64}");
            Console.WriteLine($"Base64 name in bytes: {encodingNameInBase64}");
            Console.WriteLine($"Input surname in Base64: {surnameInBase64}");
            Console.WriteLine($"Base64 surname in bytes: {encodingSurnameInBase64}");
            Console.WriteLine();



            if (encodingSurnameInBase64.Length > encodingNameInBase64.Length)
            {
                for (int i = encodingNameInBase64.Length; i < encodingSurnameInBase64.Length; i++)
                {
                    encodingNameInBase64 += "0";
                }
            }
            else if (encodingSurnameInBase64.Length < encodingNameInBase64.Length)
            {
                for (int i = encodingSurnameInBase64.Length; i < encodingNameInBase64.Length; i++)
                {
                    encodingSurnameInBase64 += "0";
                }
            }
            Console.WriteLine($"Updating bytes with nulls(name): {encodingNameInBase64}");
            Console.WriteLine($"Updating bytes with nulls(surn): {encodingSurnameInBase64}");
            Console.WriteLine();

            //ulong longnameInBase64 = Convert.ToUInt64(encodingNameInBase64, 2);
            //ulong longsurnameInBase64 = Convert.ToUInt64(encodingSurnameInBase64, 2);
            //Console.WriteLine(longname);
            //Console.WriteLine(longsurname);
            //Console.WriteLine();
            //Console.WriteLine(Convert.ToString(intname, 2));
            //Console.WriteLine(Convert.ToString(intsurname, 2));

            //ulong resultBase64 = longnameInBase64 ^ longsurnameInBase64;
            //Console.WriteLine($"Name XOR surname result(Base64): {Convert.ToString(resultBase64)}");

            //ulong wowresultBase64 = longnameInBase64 ^ longsurnameInBase64 ^ longsurnameInBase64;
            //Console.WriteLine($"Name XOR surname XOR surname result(Base64): {Convert.ToString(wowresultBase64)}");

            string resultBase64 = XORforLargeSupply(encodingNameInBase64, encodingSurnameInBase64);
            Console.WriteLine($"Name XOR surname result(Base64): {Convert.ToString(resultBase64)}");

            string wowresultBase64 = XORforLargeSupply(XORforLargeSupply(encodingNameInBase64, encodingSurnameInBase64), encodingSurnameInBase64);
            Console.WriteLine($"Name XOR surname XOR surname result(Base64): {Convert.ToString(wowresultBase64)}");
        }
        private static string Base64Encode(string s)
        {
            var bits = string.Empty;
            foreach (var character in s)
            {
                bits += Convert.ToString(character, 2).PadLeft(8, '0');
            }

            string base64 = string.Empty;

            const byte threeOctets = 24;
            var octetsTaken = 0;
            while (octetsTaken < bits.Length)
            {
                var currentOctects = bits.Skip(octetsTaken).Take(threeOctets).ToList();

                const byte sixBits = 6;
                int hextetsTaken = 0;
                while (hextetsTaken < currentOctects.Count())
                {
                    var chunk = currentOctects.Skip(hextetsTaken).Take(sixBits);
                    hextetsTaken += sixBits;

                    var bitString = chunk.Aggregate(string.Empty, (current, currentBit) => current + currentBit);

                    if (bitString.Length < 6)
                    {
                        bitString = bitString.PadRight(6, '0');
                    }
                    var singleInt = Convert.ToInt32(bitString, 2);

                    base64 += Base64Letters[singleInt];
                }

                octetsTaken += threeOctets;
            }

            // Pad with = for however many octects we have left
            for (var i = 0; i < (bits.Length % 3); i++)
            {
                base64 += "=";
            }

            return base64;
        }

        private static readonly char[] Base64Letters = new[]
                                                {
                                              'A'
                                            , 'B'
                                            , 'C'
                                            , 'D'
                                            , 'E'
                                            , 'F'
                                            , 'G'
                                            , 'H'
                                            , 'I'
                                            , 'J'
                                            , 'K'
                                            , 'L'
                                            , 'M'
                                            , 'N'
                                            , 'O'
                                            , 'P'
                                            , 'Q'
                                            , 'R'
                                            , 'S'
                                            , 'T'
                                            , 'U'
                                            , 'V'
                                            , 'W'
                                            , 'X'
                                            , 'Y'
                                            , 'Z'
                                            , 'a'
                                            , 'b'
                                            , 'c'
                                            , 'd'
                                            , 'e'
                                            , 'f'
                                            , 'g'
                                            , 'h'
                                            , 'i'
                                            , 'j'
                                            , 'k'
                                            , 'l'
                                            , 'm'
                                            , 'n'
                                            , 'o'
                                            , 'p'
                                            , 'q'
                                            , 'r'
                                            , 's'
                                            , 't'
                                            , 'u'
                                            , 'v'
                                            , 'w'
                                            , 'x'
                                            , 'y'
                                            , 'z'
                                            , '0'
                                            , '1'
                                            , '2'
                                            , '3'
                                            , '4'
                                            , '5'
                                            , '6'
                                            , '7'
                                            , '8'
                                            , '9'
                                            , '+'
                                            , '/'
                                        };

        public static double ShennonEntropy(string message, string alphabetRepeat, double entropy)
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

                for (int k = 0; k < alphabetRepeat.Length; k++)
                {
                    if (message[i] == alphabetRepeat[k])
                    {
                        theSameLetter = true;
                        break;
                    }
                }

                if (repeatchar)
                {
                    alphabetRepeat = alphabetRepeat + message[i];
                }


                if (!theSameLetter)
                {
                    entropy = entropy + ((-1) * ((double)repeats / (double)message.Length) * Math.Log((double)repeats / (double)message.Length, 2));
                    //Console.WriteLine("Symbol " + "'" + message[i] + "'" + " repeats " + repeats + " time(s) ");
                    //Console.WriteLine("Symbol's " + "'" + message[i] + "'" + " probability is " + (double)repeats / (double)message.Length);
                }
            }
            //Console.WriteLine(englishAlphabetRepeat);
            return entropy;
        }

        public static string EncodingToBytes(string message)
        {
            var enc1251 = Encoding.GetEncoding(1251);

            //if (string.IsNullOrEmpty(message))

            string bin = string.Empty;
            var txt1251 = enc1251.GetBytes(message.ToCharArray());
            foreach (var ch in txt1251)
            {
                //Console.Write($"{ch} ");
                bin += Convert.ToString(ch, 2);
            }
            //Console.WriteLine($"\n{bin}");
            return bin;
        }

        public static string XORforLargeSupply(string name, string surname)
        {
            string result = "";
            for(int i = 0; i < name.Length; i++)
            {
                if(name[i] != surname[i])
                {
                    result += "1";
                }
                else
                {
                    result += "0";
                }
            }
            return result;
        }
    }
}

