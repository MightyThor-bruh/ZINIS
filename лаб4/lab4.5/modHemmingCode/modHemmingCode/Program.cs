using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace modHemmingCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.Write("Введите информационное сообщение, длина которого больше 2 символов: ");
            string message = Console.ReadLine();
            while (message.Length < 2)
            {
                Console.Write("Некорректный ввод. Введите любое сообщение с 2-мя и более символами: ");
                message = Console.ReadLine();
            }

            string encodingMessage = EncodingToBytes(message);
            Console.WriteLine($"Введенное сообщение в двоичном виде: {encodingMessage}");

            int k = encodingMessage.Length;
            int r = HemingLenght(k);
            int n = k + r;

            int[] mas = new int[encodingMessage.Length + r];
            int[,] checkMatrix = new int[n, r];

            mas = StrInMas(encodingMessage, k);
            Console.Write("\nВходная строка = ");
            OutMass(mas, k);

            checkMatrix = CheckMatrix(k);
            Console.WriteLine("Проверочная матрица");
            OutMass(checkMatrix, k);

            //------------------------------------------------------------------------
            int nNew = n + 1;
            int rNew = r + 1;
            int[] newMas = new int[k + rNew];
            for(int i = 0; i < newMas.Length - 1; i++)
            {
                newMas[i] = mas[i];
            }
            newMas[newMas.Length - 1] = 0;
            Console.WriteLine();
            Console.WriteLine("Расширенная входная строка:");
            NewOutMass(newMas, k, rNew);
            int[,] newCheckMatrix = new int[nNew, rNew];

            #region Creation of newCheckMatrix
            for (int i = 0; i < r; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    newCheckMatrix[j, i] = checkMatrix[j, i];
                }
            }

            for(int i = 0; i < rNew; i++)
            {
                newCheckMatrix[nNew - 1, i] = 0;
            }

            for(int i = 0; i < k; i++)
            {
                newCheckMatrix[i, rNew - 1] = 1;
            }
            for (int i = k; i < nNew; i++)
            {
                newCheckMatrix[i, rNew - 1] = 0;
            }
            newCheckMatrix[nNew - 1, rNew - 1] = 1;
            #endregion

            #region newCheckMatrix output
            Console.WriteLine("\nРасширенная проверочная матрица:");
            for (int i = 0; i < rNew; i++)
            {
                for (int j = 0; j < nNew; j++)
                {
                    if (j == k)
                    {
                        Console.Write("|");
                    }
                    Console.Write(newCheckMatrix[j, i]);
                }
                Console.WriteLine();
            }
            #endregion
            //------------------------------------------------------------------------

            SindromForNewCheckMatrix(newCheckMatrix, newMas, k, rNew);
            Console.WriteLine("\nПолная строка(Xk|Xr)");
            NewOutMass(newMas, k, rNew);

            string choice;
            string temp = "012";
            do
            {
                Console.Write("Выберите количество ошибок от 0 до 2: ");
                choice = Console.ReadLine();
            } while (!temp.Contains(choice));

            int numberOfErrors = Convert.ToInt32(choice);

            switch (numberOfErrors)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        int errorPosition = GetRandom(0, k);
                        Console.WriteLine("Позиция ошибки: " + errorPosition);
                        if (newMas[errorPosition] == 1) newMas[errorPosition] = 0;
                        else newMas[errorPosition] = 1;
                        Console.WriteLine();
                        break;
                    }
                case 2:
                    {
                        int errorPosition1 = GetRandom(0, k);
                        int errorPosition2;
                        do
                        {
                            errorPosition2 = GetRandom(0, k);
                        } while (errorPosition1 == errorPosition2);
                        Console.WriteLine("Позиция первой ошибки: " + errorPosition1 + ", позиция второй: " + errorPosition2);
                        if (newMas[errorPosition1] == 1) newMas[errorPosition1] = 0;
                        else newMas[errorPosition1] = 1;
                        if (newMas[errorPosition2] == 1) newMas[errorPosition2] = 0;
                        else newMas[errorPosition2] = 1;
                        Console.WriteLine();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Из-за некорректного ввода, количество ошибок взято за число 0");
                        Console.WriteLine();
                        break;
                    }
            }


            Console.WriteLine("Строка с ошибкой/ами(Yk|Yr)");
            NewOutMass(newMas, k, rNew);

            newMas = NewSearchError(newMas, newCheckMatrix, k, rNew);
            Console.WriteLine("Строка без ошибки/ок(Xk|Xr)");
            NewOutMass(newMas, k, rNew);
        }
        //рандомное число
        public static int GetRandom(int begin, int end)
        {
            Random rnd = new Random();
            int value = rnd.Next(begin, end);
            return value;
        }

        //Перевод в биты
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

        //Находим количество проверочных символов
        public static int HemingLenght(int k)
        {
            int r = (int)(Math.Log(k, 2) + 1.99f);
            return r;
        }

        //Преобразование строки в массив
        public static int[] StrInMas(string encodingMessage, int k)
        {
            int r = HemingLenght(k);
            int[] mas = new int[encodingMessage.Length + r];

            for (int i = 0; i < encodingMessage.Length; i++)
            {
                if (encodingMessage[i] == 49)
                {
                    mas[i] = 1;
                }
                else
                {
                    mas[i] = 0;
                }
            }
            return mas;
        }

        //Вывод одномерного массива
        public static void OutMass(int[] mas, int k)
        {
            int n = HemingLenght(k) + k;
            for (int i = 0; i < n; i++)
            {
                if (i == k)
                {
                    Console.Write("|");
                }
                Console.Write(mas[i]);
            }
            Console.WriteLine("\n");
        }

        public static void NewOutMass(int[] mas, int k, int r)
        {
            int n = r + k;
            for (int i = 0; i < n; i++)
            {
                if (i == k)
                {
                    Console.Write("|");
                }
                Console.Write(mas[i]);
            }
            Console.WriteLine("\n");
        }

        //Вывод матрицы
        public static void OutMass(int[,] mas, int k)
        {
            int r = HemingLenght(k);
            int n = k + r;

            for (int i = 0; i < r; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j == k)
                    {
                        Console.Write("|");
                    }
                    Console.Write(mas[j, i]);
                }
                Console.WriteLine();
            }
        }

        //Создание проверочной матрицы
        public static int[,] CheckMatrix(int k)
        {
            int r = HemingLenght(k);
            int n = r + k;
            double rDouble = r - 1;
            int rPow = (int)(Math.Pow(2, rDouble));

            int[,] mas = new int[n, r];
            int[,] combinations = new int[rPow, r];

            for (int i = 0; i < rPow; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    combinations[i, j] = 0;
                }
            }

            for (int segmentLenght = 0; segmentLenght < r - 2; segmentLenght++)
            {
                if (segmentLenght * r > k) break;

                for (int i = 0; i < segmentLenght + 2; i++)
                {
                    combinations[segmentLenght * r, i] = 1;
                }

                for (int segmentPositin = 1; segmentPositin < r; segmentPositin++)
                {
                    for (int i = 0; i < r - 1; i++)
                    {
                        combinations[segmentLenght * r + segmentPositin, i + 1] = combinations[segmentLenght * r + segmentPositin - 1, i];
                    }
                    combinations[segmentLenght * r + segmentPositin, 0] = combinations[segmentLenght * r + segmentPositin - 1, r - 1];
                }

                if (segmentLenght == r - 3)
                {
                    for (int i = 0; i < r; i++)
                    {
                        combinations[rPow - 1, i] = 1;
                    }
                }
            }

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    mas[i, j] = combinations[i, j];
                }
            }

            for (int i = 0; i < r; i++)
            {
                mas[i + k, i] = 1;
            }

            return mas;
        }

        //Поиск синдрома
        public static int[] Sindrom(int[,] CheckaMatrix, int[] mas, int k)
        {
            int r = HemingLenght(k);
            int[] sindrom = new int[r];

            for (int i = 0, l = 0; i < r; i++, l = 0)
            {
                for (int j = 0; j < k; j++)
                {
                    if (CheckaMatrix[j, i] == 1 && mas[j] == 1)
                    {
                        l++;
                    }
                    else
                    {
                        sindrom[i] = 0;
                    }
                }
                if (l % 2 == 1)
                {
                    sindrom[i] = 1;
                }
                else
                {
                    sindrom[i] = 0;
                }
            }

            for (int i = 0; i < r; i++)
            {
                mas[i + k] = sindrom[i];
            }

            return mas;
        }

        public static int[] SindromForNewCheckMatrix(int[,] CheckaMatrix, int[] mas, int k, int r)
        {
            int[] sindrom = new int[r];

            for (int i = 0, l = 0; i < r; i++, l = 0)
            {
                for (int j = 0; j < k; j++)
                {
                    if (CheckaMatrix[j, i] == 1 && mas[j] == 1)
                    {
                        l++;
                    }
                    else
                    {
                        sindrom[i] = 0;
                    }
                }
                if (l % 2 == 1)
                {
                    sindrom[i] = 1;
                }
                else
                {
                    sindrom[i] = 0;
                }
            }

            for (int i = 0; i < r; i++)
            {
                mas[i + k] = sindrom[i];
            }

            return mas;
        }

        //Нахождение ошибок
        public static int[] SearchError(int[] mas, int[,] checkMatrix, int k)
        {
            int r = HemingLenght(k);
            int n = k + r;

            int[] beforeSindrom = new int[r];

            //запоминание проверочных битов
            for (int i = k; i < n; i++)
            {
                beforeSindrom[i - k] = mas[i];
            }

            mas = Sindrom(checkMatrix, mas, k);
            Console.WriteLine("Вывод новых избыточных символов/проверочного кода(Yk|Xr')");
            OutMass(mas, k);

            //сложение синдрома по модулю два
            for (int i = k, j = 0; i < n; i++)
            {
                if (beforeSindrom[i - k].Equals(mas[i]))
                {
                    mas[i] = 0;

                    j++;

                    if (j == r)
                    {
                        for (int l = k; l < n; l++)
                        {
                            mas[l] = beforeSindrom[l - k];
                        }
                        return mas;
                    }
                }
                else
                {
                    mas[i] = 1;
                }
            }

            for (int i = 0; i < n; i++)
            {
                int l = 0;
                for (int j = 0; j < r; j++)
                {
                    if (checkMatrix[i, j].Equals(mas[j + k]))
                        l++;
                }
                if (l == r)
                {
                    mas[i] = (mas[i] + 1) % 2;
                }
            }
            Console.WriteLine("Вывод синдрома(Xk|s)");
            OutMass(mas, k);
            mas = Sindrom(checkMatrix, mas, k);
            return mas;
        }

        public static int[] NewSearchError(int[] mas, int[,] checkMatrix, int k, int r)
        {
            int n = k + r;

            int[] beforeSindrom = new int[r];

            //запоминание проверочных битов
            for (int i = k; i < n; i++)
            {
                beforeSindrom[i - k] = mas[i];
            }

            mas = SindromForNewCheckMatrix(checkMatrix, mas, k, r);
            Console.WriteLine("Вывод новых избыточных символов/проверочного кода(Yk|Xr')");
            NewOutMass(mas, k, r);

            //сложение синдрома по модулю два
            for (int i = k, j = 0; i < n; i++)
            {
                if (beforeSindrom[i - k].Equals(mas[i]))
                {
                    mas[i] = 0;

                    j++;

                    if (j == r)
                    {
                        for (int l = k; l < n; l++)
                        {
                            mas[l] = beforeSindrom[l - k];
                        }
                        return mas;
                    }
                }
                else
                {
                    mas[i] = 1;
                }
            }

            for (int i = 0; i < n; i++)
            {
                int l = 0;
                for (int j = 0; j < r; j++)
                {
                    if (checkMatrix[i, j].Equals(mas[j + k]))
                        l++;
                }
                if (l == r)
                {
                    mas[i] = (mas[i] + 1) % 2;
                }
            }
            Console.WriteLine("Вывод синдрома(Xk|s)");
            NewOutMass(mas, k, r);

            int counter = 0;
            for(int i = k; i < mas.Length; i++)
            {
                if(mas[i] == 1)
                {
                    counter++;
                }
            }
            if(counter % 2 == 0)
            {
                Console.WriteLine("Вес синдрома четный");
            }
            else
            {
                Console.WriteLine("Вес синдрома нечетный");
            }
            //Console.WriteLine("counter = " + counter);

            mas = SindromForNewCheckMatrix(checkMatrix, mas, k, r);
            return mas;
        }
    }
}
//присутствует несоответствие четности и нечетности синдрома с количеством ошибок:(
