using System;
using System.Collections;
using System.IO;
using System.Text;

namespace HemingCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var messageFile = "message.txt";
            string message;
            using (var reader = new StreamReader(messageFile))
            {
                 message = reader.ReadToEnd();     
            }
            Console.WriteLine("Сообщение из файла: "+message);

            while(message.Length < 2)
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
            Console.Write("\nВходное сообщение = ");
            OutMass(mas, k);

            checkMatrix = CheckMatrix(k);
            Console.WriteLine("Проверочная матрица");
            OutMass(checkMatrix, k);

            Sindrom(checkMatrix, mas, k);
            Console.WriteLine("\nПолное сообщение (Xk|Xr)");
            OutMass(mas, k);

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
                        if (mas[errorPosition] == 1) mas[errorPosition] = 0;
                            else mas[errorPosition] = 1;
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
                        if (mas[errorPosition1] == 1) mas[errorPosition1] = 0;
                            else mas[errorPosition1] = 1;
                        if (mas[errorPosition2] == 1) mas[errorPosition2] = 0;
                            else mas[errorPosition2] = 1;
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


            Console.WriteLine("Сообщение с ошибкой/ами(Yk|Yr)");
            OutMass(mas, k);

            mas = SearchError(mas, checkMatrix, k);
            Console.WriteLine("Сообщение без ошибки/ок(Xk|Xr)");
            OutMass(mas, k);
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

            for(int i=0; i< encodingMessage.Length; i++)
            {
                if(encodingMessage[i] == 49)
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
            for(int i = 0; i < n; i++)
            {
                if(i == k)
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

            for(int i = 0; i < r; i++)
            {
                for(int j =0; j <n;j++)
                {
                    if(j == k)
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

            for(int i=0; i <rPow; i++)
            {
                for(int j = 0; j < r; j++)
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

            for(int i=0, l = 0; i < r; i++, l=0)
            {
                for(int j = 0; j<k; j++)
                {
                    if(CheckaMatrix[j,i] == 1 && mas[j] == 1)
                    {
                        l++;
                    }
                    else
                    {
                        sindrom[i] = 0;
                    }
                }
                if(l % 2 == 1)
                {
                    sindrom[i] = 1;
                }
                else
                {
                    sindrom[i] = 0;
                }
            }

            for(int i = 0; i < r; i++)
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
            for(int i =k; i < n; i++)
            {
                beforeSindrom[i - k] = mas[i];
            }

            mas = Sindrom(checkMatrix, mas, k);
            Console.WriteLine("Вывод новых избыточных символов/проверочного кода(Yk|Xr')");
            OutMass(mas, k);

            //сложение синдрома по модулю два
            for (int i = k, j = 0; i < n; i++)
            {
                if(beforeSindrom[i -k].Equals(mas[i]))
                {
                    mas[i] = 0;

                    j++;
                    
                    if(j == r)
                    {
                        for(int l = k; l < n; l++)
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

            for(int i = 0; i < n; i++)
            {
                int l = 0;
                for(int j = 0; j <r ; j++)
                {
                    if (checkMatrix[i, j].Equals(mas[j + k]))
                        l++;
                }
                if(l == r)
                {
                    mas[i] = (mas[i] + 1) % 2;
                }
            }
            Console.WriteLine("Вывод синдрома(Xk|s)");
            OutMass(mas, k);
            mas = Sindrom(checkMatrix, mas, k);
            return mas;
        }
    }
}
