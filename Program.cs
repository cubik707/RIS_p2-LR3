using System;

namespace RIS_p2_LR3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var ui = new UserInterface();
                ui.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}
