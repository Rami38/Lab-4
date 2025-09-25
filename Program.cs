using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab_4
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                {
                    Console.WriteLine("Usage: Lab_3.exe <training_file> <scrambled_file> <output_file>");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    return;
                }

                string trainingFile = args[0];
                string scrambledFile = args[1];
                string outputFile = args[2];

                CaesarCipher processor = new CaesarCipher();
                processor.ProcessFiles(trainingFile, scrambledFile, outputFile);

                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }
    }

    public class CaesarCipher
    {
        public void ProcessFiles(string trainingFile, string scrambledFile, string outputFile)
        {
            Console.WriteLine($"Reading input file \"{trainingFile}\".");
            string trainingText = File.ReadAllText(trainingFile);
            Console.WriteLine($"Length of input file is {trainingText.Split('\n').Length} lines, and {trainingText.Length} characters.");

            char mostFrequentTraining = GetMostFrequentLetter(trainingText);
            Console.WriteLine($"The most occurring character is {mostFrequentTraining}, occurring {trainingText.Count(c => char.ToLower(c) == char.ToLower(mostFrequentTraining))} times.");

            Console.WriteLine($"Reading the encrypted file \"{scrambledFile}\".");
            string scrambledText = File.ReadAllText(scrambledFile);
            char mostFrequentScrambled = GetMostFrequentLetter(scrambledText);
            int scrambledCount = scrambledText.Count(c => char.ToLower(c) == char.ToLower(mostFrequentScrambled));
            Console.WriteLine($"The most occurring character is {mostFrequentScrambled}, occurring {scrambledCount} times.");

            int shift = (mostFrequentScrambled - mostFrequentTraining + 26) % 26;
            Console.WriteLine($"A shift factor of {shift} has been determined.");

            string decryptedText = DecryptText(scrambledText, shift);

            Console.WriteLine($"Writing output file now to \"{outputFile}\".");
            File.WriteAllText(outputFile, decryptedText);

            Console.WriteLine("Display the file? (y/n)");
            string response = Console.ReadLine();
            if (response?.ToLower() == "y")
            {
                Console.WriteLine("Decrypted content:");
                Console.WriteLine(decryptedText);
            }
        }

        private char GetMostFrequentLetter(string text)
        {
            var frequency = new Dictionary<char, int>();

            foreach (char c in text.ToLower())
            {
                if (char.IsLetter(c))
                {
                    if (frequency.ContainsKey(c))
                        frequency[c]++;
                    else
                        frequency[c] = 1;
                }
            }

            return frequency.OrderByDescending(kv => kv.Value).First().Key;
        }

        private string DecryptText(string text, int shift)
        {
            char[] result = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    char shifted = (char)(((c - baseChar - shift + 26) % 26) + baseChar);
                    result[i] = shifted;
                }
                else
                {
                    result[i] = c;
                }
            }

            return new string(result);
        }
    }
}