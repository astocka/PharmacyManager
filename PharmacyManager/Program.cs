using System;
using System.Collections.Generic;

namespace PharmacyManager
{
    class Program
    {
        static void Main(string[] args)
        {
            string command = "";
            bool exitApp = false;

            while (!exitApp)
            {
                Console.Clear();
                MainMenu.Menu();
                Console.Write("Enter the command: ");
                command = Console.ReadLine().Trim().ToLower();
                Console.Clear();

                switch (command)
                {
                    case "Exit":
                    case "e":
                        exitApp = true;
                        break;
                    case "AddMedicine":
                    case "am":
                        AddMedicine();
                        break;
                    case "EditMedicine":
                    case "em":
                        EditMedicine();
                        break;
                    case "UpdateAmount":
                    case "ua":
                        UpdateAmount();
                        break;
                    case "DeleteMedicine":
                    case "dm":
                        DeleteMedicine();
                        break;
                    case "ShowMedicines":
                    case "sm":
                        ShowMedicines();
                        break;
                    case "SellMedicine":
                    case "sell":
                        SellMedicine();
                        break;
                    case "Help":
                    case "h":
                        HelpMenu.Help();
                        break;

                    default:
                        Console.Clear();
                        MainMenu.Menu();
                        ConsoleEx.Write("Wrong command. Press ENTER to contunue... ", ConsoleColor.Red);
                        Console.ReadLine();
                        break;
                }
            }
        }

        private static void AddMedicine()
        {
            Console.Clear();
            MainMenu.MenuS();
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            ConsoleEx.WriteLine(" :: ADD MEDICINE ::", ConsoleColor.Gray);
            Medicine medicine = new Medicine();
            medicine.Save(medicine.Reload());
            Console.WriteLine();
            ConsoleEx.WriteLine("Success!", ConsoleColor.Green);
            Console.Write("Press ENTER to continue... ");
            Console.ReadLine();
        }

        private static void EditMedicine()
        {
            Console.Clear();
            MainMenu.MenuS();
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            ConsoleEx.WriteLine(" :: EDIT MEDICINE ::", ConsoleColor.Gray);
            Console.Write("Enter Id of medicine to update: ");
            int idEditMedicine = Convert.ToInt32(Console.ReadLine().Trim());

            Medicine previousMedicine = new Medicine();
            Console.WriteLine("Enter data of new medicine:");
            Medicine currentMedicine = new Medicine();
            currentMedicine.EditSingleMedicine(idEditMedicine, previousMedicine.ReloadById(idEditMedicine), currentMedicine.ReloadToEdit(previousMedicine.Name));

            ConsoleEx.WriteLine("Success!", ConsoleColor.Green);
            Console.Write("Press ENTER to continue... ");
            Console.ReadLine();
        }

        private static void UpdateAmount()
        {
            Console.Clear();
            MainMenu.MenuS();
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            ConsoleEx.WriteLine(" :: UPDATE AMOUNT ::", ConsoleColor.Gray);
            Console.Write("Enter Id of the medicine: ");
            int idUpdateMedicine = Convert.ToInt32(Console.ReadLine().Trim());

            Console.Write("Enter amount to update: ");
            int amountUpdate = Convert.ToInt32(Console.ReadLine().Trim());

            Medicine.UpdateSingleMedicine(idUpdateMedicine, amountUpdate);
            ConsoleEx.WriteLine("Success!", ConsoleColor.Green);
            Console.Write("Press ENTER to continue... ");
            Console.ReadLine();
        }

        private static void DeleteMedicine()
        {
            Console.Clear();
            MainMenu.MenuS();
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            ConsoleEx.WriteLine(" :: DELETE MEDICINE ::", ConsoleColor.Gray);
            Console.Write("Enter Id of the medicine: ");
            int idDeleteMedicine = Convert.ToInt32(Console.ReadLine().Trim());
            Medicine medicine = new Medicine();
            medicine.Remove(idDeleteMedicine);
            ConsoleEx.WriteLine("Success!", ConsoleColor.Green);
            Console.Write("Press ENTER to continue... ");
            Console.ReadLine();
        }

        private static void ShowMedicines()
        {
            Console.Clear();
            MainMenu.MenuS();
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            ConsoleEx.WriteLine(" :: SHOW MEDICINES ::", ConsoleColor.Gray);
            Console.WriteLine();
            Console.Write(" | Id ");
            Console.Write(" | Name ".PadLeft(10));
            Console.Write(" | Manufacturer ".PadLeft(40));
            Console.Write(" | Price ".PadLeft(25));
            Console.Write(" | Amount ".PadLeft(13));
            Console.Write(" | With Prescription ");
            Console.WriteLine();
            ConsoleEx.WriteLine("".PadLeft(115, '-'), ConsoleColor.DarkYellow);

            List<Medicine> medicines = Medicine.ShowAllMedicines();

            foreach (var medicine in medicines)
            {
                Console.WriteLine();
                Console.Write($" | {medicine.Id.ToString().PadLeft(5)}");
                Console.Write($" | {medicine.Name.PadLeft(29)}");
                Console.Write($" | {medicine.Manufacturer.PadLeft(29)}");
                Console.Write($" | {medicine.Price.ToString().PadLeft(9)}");
                Console.Write($" | {medicine.Amount.ToString().PadLeft(7)}");
                Console.Write($" | {medicine.WithPrescription.ToString()}");
            }
            Console.WriteLine();
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            Console.Write("Press ENTER to continue... ");
            Console.ReadLine();
        }

        private static void SellMedicine()
        {
            Console.Clear();
            MainMenu.MenuS();
            ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
            ConsoleEx.WriteLine(" :: SELL MEDICINE ::", ConsoleColor.Gray);
            Console.WriteLine();

            Console.Write("Enter the Id of the medicine: ");
            int idSellMedicine = Convert.ToInt32(Console.ReadLine());

            Medicine medicine = new Medicine();
            Console.Write("Enter the amount: ");
            int amountSellMedicine = Convert.ToInt32(Console.ReadLine());

            if (medicine.ReloadById(idSellMedicine).WithPrescription == false)
            {
                Order.Sell(idSellMedicine, amountSellMedicine, medicine.ReloadById(idSellMedicine).Amount);
            }
            else
            {
                Console.Write("Enter customer's name: ");
                string customerName = Console.ReadLine().Trim();
                Console.Write("Enter PESEL number: ");
                string pesel = Console.ReadLine().Trim();
                Console.Write("Enter the prescription number: ");
                int prescriptionNumber = Convert.ToInt32(Console.ReadLine());

                Order.SellMedicine(idSellMedicine, amountSellMedicine, medicine.ReloadById(idSellMedicine).Amount);
                int prescriptionId = Prescription.SavePrescription(customerName, pesel, prescriptionNumber);
                Order.UpdateOrders(prescriptionId, idSellMedicine, amountSellMedicine);
            }
            ConsoleEx.WriteLine("Success!", ConsoleColor.Green);
            Console.Write("Press ENTER to continue... ");
            Console.ReadLine();
        }
    }
}
