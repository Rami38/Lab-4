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
                    Console.WriteLine("UEnter the good number of parameters");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    return;
                }

                string trainingFile = args[0];
                string scrambledFile = args[1];
                string outputFile = args[2];

                
                CaesarCypherProcessor processor = new CaesarCypherProcessor();
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

    public class CaesarCypherProcessor
    {
        public void ProcessFiles(string trainingFile, string scrambledFile, string outputFile)
        {
            
            Console.WriteLine($"Reading input file \"{trainingFile}\".");
            string trainingText = ReadFile(trainingFile);
            Console.WriteLine($"Length of input file is {CountLines(trainingText)} lines, and {trainingText.Length} characters.");

            var trainingFrequencies = AnalyzeCharacterFrequency(trainingText);
            char mostFrequentTrainingChar = trainingFrequencies.First().Key;
            int trainingCharCount = trainingFrequencies.First().Value;

            
            var topTwoTraining = trainingFrequencies.Take(2).ToList();
            Console.WriteLine($"The two most occurring characters are {topTwoTraining[0].Key} and {topTwoTraining[1].Key}, " +
                              $"occurring {topTwoTraining[0].Value} times and {topTwoTraining[1].Value} times.");

            
            Console.WriteLine($"Reading the encrypted file \"{scrambledFile}\".");
            string scrambledText = ReadFile(scrambledFile);

            var scrambledFrequencies = AnalyzeCharacterFrequency(scrambledText);
            char mostFrequentScrambledChar = scrambledFrequencies.First().Key;
            int scrambledCharCount = scrambledFrequencies.First().Value;

            Console.WriteLine($"The most occurring character is {mostFrequentScrambledChar}, occurring {scrambledCharCount} times.");

            
            int shiftFactor = CalculateShiftFactor(mostFrequentTrainingChar, mostFrequentScrambledChar);
            Console.WriteLine($"A shift factor of {shiftFactor} has been determined.");

            
            string decryptedText = DecryptText(scrambledText, shiftFactor);

            
            Console.WriteLine($"Writing output file now to \"{outputFile}\".");
            WriteFile(outputFile, decryptedText);

            
            Console.Write("Display the file? (y/n): ");
            string response = Console.ReadLine()?.ToLower();

            if (response == "y" || response == "yes")
            {
                Console.WriteLine("\nDecrypted Text:");
                Console.WriteLine("================");
                Console.WriteLine(decryptedText);
                Console.WriteLine("================");
            }
        }

        private string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            return File.ReadAllText(filePath);
        }

        private void WriteFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        private int CountLines(string text)
        {
            return text.Split('\n').Length;
        }

        private Dictionary<char, int> AnalyzeCharacterFrequency(string text)
        {
            Dictionary<char, int> frequency = new Dictionary<char, int>();

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    char lowerChar = char.ToLower(c);
                    if (frequency.ContainsKey(lowerChar))
                    {
                        frequency[lowerChar]++;
                    }
                    else
                    {
                        frequency[lowerChar] = 1;
                    }
                }
            }

            
            return frequency.OrderByDescending(pair => pair.Value)
                           .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private int CalculateShiftFactor(char trainingChar, char scrambledChar)
        {
            
            trainingChar = char.ToLower(trainingChar);
            scrambledChar = char.ToLower(scrambledChar);

            
            int shift = (scrambledChar - trainingChar) % 26;

            
            if (shift < 0)
            {
                shift += 26;
            }

            return shift;
        }

        private string DecryptText(string encryptedText, int shiftFactor)
        {
            char[] decryptedChars = new char[encryptedText.Length];

            for (int i = 0; i < encryptedText.Length; i++)
            {
                char c = encryptedText[i];

                if (char.IsLetter(c))
                {
                    char baseChar = char.IsUpper(c) ? 'A' : 'a';
                    
                    char decryptedChar = (char)(((c - baseChar - shiftFactor + 26) % 26) + baseChar);
                    decryptedChars[i] = decryptedChar;
                }
                else
                {
                    decryptedChars[i] = c;
                }
            }

            return new string(decryptedChars);
        }
    }
}