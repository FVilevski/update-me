using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.App
{
    internal static class Extensions
    {
        public static void WriteErrorToConsole(this string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WriteSuccessToConsole(this string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void WriteInfoToConsole(this string message)
        {
            Console.WriteLine(message);
        }

        public static void WriteProgressToConsole(this int progress)
        {
            int currentPosition = Console.CursorLeft;
            Console.Write($" {progress}%");
            Console.SetCursorPosition(currentPosition, Console.CursorTop);
        }
    }
}
