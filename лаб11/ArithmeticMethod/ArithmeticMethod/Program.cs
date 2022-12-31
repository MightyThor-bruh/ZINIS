using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArithmeticMethod
{
    class Program
    {
        private struct Section
        {
            public double Left;
            public double Right;
        }
        static void Main(string[] args)
        {
            var originMessage1 = "гидроаэроионизация";
            var originMessage2 = "гидроаэроионизацияэкстраординарный";

            DoArithmeticCodeAndDecode(originMessage1);
            DoArithmeticCodeAndDecode(originMessage2);
        }
        private static void DoArithmeticCodeAndDecode(string message)
        {
            Console.WriteLine($"Исходное сообщение: {message}");

            var probabilitySections = GetProbabilitySections(message);
            Console.WriteLine("Полученные вероятности:");
            foreach (var (letter, probability) in probabilitySections)
                Console.Write($"{letter}-{probability.Right - probability.Left} ");
            Console.WriteLine();

            var code = ArithmeticCode(message, probabilitySections);
            Console.WriteLine($"Полученное число: {code}");

            var decodedMessage = ArithmeticDecode(probabilitySections, code, message.Length);
            Console.WriteLine($"Расшифрованное сообщение: {decodedMessage}\n");
        }

        private static Dictionary<char, Section> GetProbabilitySections(string text)
        {
            Dictionary<char, double> probabilities = new();
            foreach (var letter in text)
                if (probabilities.ContainsKey(letter))
                    probabilities[letter]++;
                else
                    probabilities.Add(letter, 1);

            Dictionary<char, Section> probabilitySections = new();
            double currentProbability = 0;
            foreach (var keyPair in probabilities)
            {
                var probability = keyPair.Value / text.Length;
                probabilitySections.Add(keyPair.Key,
                    new Section { Left = currentProbability, Right = currentProbability + probability });
                currentProbability += probability;
            }

            return probabilitySections;
        }

        private static double ArithmeticCode(string originMessage, Dictionary<char, Section> probabilitySetions)
        {
            var current = new Section { Left = 0, Right = 1 };
            foreach (var letter in originMessage)
            {
                var newRight = current.Left + (current.Right - current.Left) * probabilitySetions[letter].Right;
                current.Left = current.Left + (current.Right - current.Left) * probabilitySetions[letter].Left;
                current.Right = newRight;
            }

            return (current.Left + current.Right) / 2;
        }

        private static string ArithmeticDecode(Dictionary<char, Section> probabilitySections, double code,
            int messageLength)
        {
            StringBuilder decodedMessage = new();
            var arrayLetters = probabilitySections.Keys.ToList();
            for (var i = 0; i < messageLength; i++)
                for (var j = 0; j < probabilitySections.Count; j++)
                    if (code > probabilitySections[arrayLetters[j]].Left &&
                        code < probabilitySections[arrayLetters[j]].Right)
                    {
                        decodedMessage.Append(arrayLetters[j]);
                        code = (code - probabilitySections[arrayLetters[j]].Left) /
                               (probabilitySections[arrayLetters[j]].Right - probabilitySections[arrayLetters[j]].Left);
                        break;
                    }

            return decodedMessage.ToString();
        }
    }
}
