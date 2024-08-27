using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksCRUDInMemory.Utils
{
    public static class ValidateInput
    {
        public static string RequestInput(string message)
        {
            string input;
            do
            {
                Console.WriteLine(message);
                input = Console.ReadLine() ?? string.Empty; // Garantiza que input no sea null
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("No haz introducido la información requerida.");
                }
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }
    }
}
