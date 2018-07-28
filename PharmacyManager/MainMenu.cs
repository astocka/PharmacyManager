using System;

namespace PharmacyManager
{
    public class MainMenu
    {
        public static void Menu()
        {
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            ConsoleEx.WriteLine(":: P H A R M A C Y   M A N A G E R ::".PadLeft(70), ConsoleColor.Gray);
            ConsoleEx.WriteLine("".PadLeft(115, '-'), ConsoleColor.DarkYellow);
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            Console.WriteLine("   :: MAIN MENU ::");
            Console.WriteLine();
            Console.WriteLine(" >> AddMedicine    [am]");
            Console.WriteLine(" >> EditMedicine   [em]");
            Console.WriteLine(" >> UpdateAmount   [ua]");
            Console.WriteLine(" >> DeleteMedicine [dm]");
            Console.WriteLine(" >> ShowMedicines  [sm]");
            Console.WriteLine(" >> SellMedicine   [sell]");
            Console.WriteLine(" >> Help           [h]");
            Console.WriteLine(" >> Exit           [e]");
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
        }

        public static void MenuS()
        {
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            ConsoleEx.WriteLine(":: P H A R M A C Y   M A N A G E R ::".PadLeft(70), ConsoleColor.Gray);
            ConsoleEx.WriteLine("".PadLeft(115, '-'), ConsoleColor.DarkYellow);
        }
    }
}
