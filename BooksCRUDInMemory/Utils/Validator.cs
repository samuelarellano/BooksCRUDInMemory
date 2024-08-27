using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksCRUDInMemory.Utils
{
    public static class ValidateInput
    {
        private static IConsoleWrapper _consoleWrapper;

        public static void SetConsoleWrapper(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }

        public static string RequestInput(string message)
        {
            string input;
            do
            {
                _consoleWrapper.WriteLine(message);
                input = _consoleWrapper.ReadLine() ?? string.Empty; // Garantiza que input no sea null
                if (string.IsNullOrWhiteSpace(input))
                {
                    _consoleWrapper.WriteLine("No haz introducido la información requerida.");
                }
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }
    }
}
