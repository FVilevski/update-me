using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.App
{
    internal static class Extensions
    {
        public const string RELEASE_INFO_FILENAME = "RELEASES";
        public const string SETUP_EXE_FILENAME = "Setup.exe";
        public const string SETUP_MSI_FILENAME = "Setup.msi";

        public const string DISTRIBUTION_TYPE_LOCAL = "local";
        public const string DISTRIBUTION_TYPE_AWS = "aws";

        public static string[] FILES_EXCLUDE_LIST = { "*.pdb", "*.vshost*" };

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

        public static int Value(this ResultCodeEnum code)
        {
            return (int)code;
        }

        public static IEnumerable<string> GetDirectoryFiles(this string directoryPath)
        {
            List<string> excludeFiles = new List<string>();
            foreach (string filter in FILES_EXCLUDE_LIST)
            {
                excludeFiles.AddRange(System.IO.Directory.GetFiles(directoryPath, filter));
            }

            return System.IO.Directory.GetFiles(directoryPath).Except(excludeFiles);
        }
    }
}
