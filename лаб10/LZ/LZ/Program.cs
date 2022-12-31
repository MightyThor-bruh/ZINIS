using System;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LZ
{
    class Program
    {
        static void Main(string[] args)
        {
            var originalMessage =
                "213380732";
            var N = 9; // Мощность алфавита
            var n1 = 5;
            var n2 = 5;
            var ratio = 0;

            Console.WriteLine($"Исходная строка:\n{originalMessage}");
            {
                Console.Write($"\nВведите n1 (по-умолчанию {n1}): ");
                if (int.TryParse(Console.ReadLine(), out var newValue))
                    n1 = newValue;
                Console.Write($"Введите n2 (по-умолчанию {n2}): ");
                if (int.TryParse(Console.ReadLine(), out newValue))
                    n2 = newValue;
            }
            originalMessage = string.Join("", Enumerable.Repeat(' ', n1)) + originalMessage;

            var triadLength = (int)(Math.Ceiling(Math.Log(n1, N)) + Math.Ceiling(Math.Log(n2, N)) + 1);
            Console.WriteLine($"\nДлина триады: {triadLength}");

            // Compression
            Stopwatch ticksEncode = new Stopwatch();
            ticksEncode.Start();
            var compressedMessage = new StringBuilder();
            {
                var currentSymbol = n1;
                while (currentSymbol < originalMessage.Length)
                {
                    var sequenceLength = 0;
                    var offsetSequence = n1;
                    int offsetSequenceLast;
                    do
                    {
                        offsetSequenceLast = n1 - offsetSequence;
                        sequenceLength++;
                        var secondBufferLength = currentSymbol + n2 < originalMessage.Length
                            ? n2 - 1
                            : originalMessage.Length - currentSymbol;
                        offsetSequence = originalMessage.Substring(currentSymbol - n1, n1 + secondBufferLength)
                            .IndexOf(originalMessage.Substring(currentSymbol, sequenceLength),
                                StringComparison.Ordinal);
                        if (offsetSequence >= n1)
                            offsetSequence = -1;
                    } while (offsetSequence >= 0);

                    sequenceLength--;
                    currentSymbol += sequenceLength;
                    compressedMessage.Append(string.Format(
                        $"{(char)(97 + offsetSequenceLast)}{(char)(97 + sequenceLength)}{originalMessage.Substring(currentSymbol, 1)} "));
                    currentSymbol += 1;
                }

                Console.WriteLine($"\nПолученная строка:\n{compressedMessage}");
            }
            ticksEncode.Stop();
            // Decompression
            Stopwatch ticksDecode = new Stopwatch();
            ticksDecode.Start();
            {
                var currentSymbol = 0;
                var message = compressedMessage.ToString();
                var decompressedString = new StringBuilder();
                while (compressedMessage.Length > currentSymbol)
                {
                    var triade = message.Substring(currentSymbol, triadLength + 1);
                    var p = triade[0] - 97;
                    var q = triade[1] - 97;
                    if (p == 0)
                        decompressedString.Append(triade[2]);
                    else
                    {
                        while (q - p >= 0)
                        {
                            decompressedString.Append(decompressedString.ToString()[(decompressedString.Length - p)..]);
                            q -= p;
                        }

                        decompressedString.Append(decompressedString.ToString()
                            .Substring(decompressedString.Length - p, q));
                        decompressedString.Append(triade[2]);
                    }

                    currentSymbol += triadLength + 1;
                }
                ticksDecode.Stop();
                Console.WriteLine($"\n Распакованная строка:\n{decompressedString}");
                ratio = decompressedString.Length / compressedMessage.Length;
                Console.WriteLine($"\n Эффективность:\n{ratio}");
                Console.WriteLine("Скорость кодирования: " + ticksEncode.Elapsed);
                Console.WriteLine("Скорость декодирования: " + ticksDecode.Elapsed + "\n");
            }
        }
    }
}
