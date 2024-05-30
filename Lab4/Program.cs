using System;
using System.IO;

namespace FileCounterApp
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Помилка: шлях до каталогу не вказано.");
                return -1;
            }
            else if (args.Length == 1 && args[0].Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                ShowHelp();
                return 0;
            }

            try
            {
                string directoryPath = args[0];
                string filePattern = "*.*";
                string fileAttributes = "";
                int fileCount = 0;

                if (args.Length > 1 && args[1].StartsWith("-"))
                {
                    fileAttributes = args[1].ToUpper();
                    if (args.Length > 2)
                    {
                        filePattern = args[2];
                    }
                }
                else if (args.Length > 1)
                {
                    filePattern = args[1];
                }

                string[] files = Directory.GetFiles(directoryPath, filePattern);

                foreach (string file in files)
                {
                    if (IsMatchAttributes(file, fileAttributes))
                    {
                        fileCount++;
                    }
                }

                Console.WriteLine($"Кількість файлів у каталозі '{directoryPath}': {fileCount}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Сталася помилка: {ex.Message}");
                return -2;
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Синтаксис використання утиліти:");
            Console.WriteLine("FileCounterApp <шлях до каталогу> [-атрибути] [шаблон файлів]");
            Console.WriteLine("Де:");
            Console.WriteLine("  <шлях до каталогу> - обов'язковий параметр, шлях до каталогу для підрахунку файлів.");
            Console.WriteLine("  -атрибути - необов'язковий параметр, що визначає атрибути файлів (H - приховані, A - архівні, R - тільки для читання).");
            Console.WriteLine("  [шаблон файлів] - необов'язковий параметр, шаблон файлів (наприклад, *.exe).");
        }

        static bool IsMatchAttributes(string filePath, string attributes)
        {
            FileAttributes fileAttr = File.GetAttributes(filePath);

            if (attributes.Contains('H') && (fileAttr & FileAttributes.Hidden) != FileAttributes.Hidden)
                return false;
            if (attributes.Contains('A') && (fileAttr & FileAttributes.Archive) != FileAttributes.Archive)
                return false;
            if (attributes.Contains('R') && (fileAttr & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                return false;

            return true;
        }
    }
}
