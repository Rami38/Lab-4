using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the training file location and name:");
            string trainingFile = Console.ReadLine();
            string readText = trainingFile.ReadAllText(trainingFile);

            int A = 'A';
            int Z = 'Z';

            Console.WriteLine($"A: {A} and Z:{Z} ");
            Console.WriteLine(Z - A);
            int numberOfShifts = 23;
            //Convert letterToInteger
            int x = 'Z' - 'A';
            //The modulo ensures the shifted value wraps around the alphabet.(we have 26 letters in the alphabet)
            int encrypted = (x + numberOfShifts) % 26;
            Console.WriteLine("Encrypted value: " + encrypted);
            char encryptedChar = (char)(encrypted + 'A');
            Console.WriteLine($"Encrypted char: {encryptedChar}");

        }
    }
}
