using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PharmacyManager
{
    public class Medicine : ActiveRecord
    {
        static string connectionString = @"Integrated Security=SSPI;" +
                                         "Initial Catalog=Pharmacy;" +
                                         "Data Source=.\\SQLEXPRESS;";

        public override int Id { get; set; }
        public string Name { get; protected set; }
        public string Manufacturer { get; protected set; }
        public decimal Price { get; protected set; }
        public int Amount { get; protected set; }
        public bool WithPrescription { get; set; }
        public SqlConnection SqlConnection;

        public Medicine(int id, string name, string manufacturer, decimal price, int amount, bool withPrescription)
        {
            Id = id;
            Name = name;
            Manufacturer = manufacturer;
            Price = price;
            Amount = amount;
            WithPrescription = withPrescription;
        }

        public Medicine(string name, string manufacturer, decimal price, int amount, bool withPrescription)
        {
            Name = name;
            Manufacturer = manufacturer;
            Price = price;
            Amount = amount;
            WithPrescription = withPrescription;
        }

        public Medicine()
        {
        }

        public override Medicine Reload()
        {
            Console.Clear();
            MainMenu.MenuS();

            string getName = "";
            string getManufacturer = "";
            decimal getPrice = 0;
            int getAmount = 0;
            bool getWithPrescription = false;

            try
            {
                ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
                ConsoleEx.WriteLine(" Please enter data of the medicine:", ConsoleColor.Gray);
                Console.Write(" >> Name: ");
                getName = Console.ReadLine().Trim();
                Console.Write(" >> Manufacturer: ");
                getManufacturer = Console.ReadLine().Trim();
                Console.Write(" >> Price: ");
                getPrice = decimal.Parse(Console.ReadLine().Trim().Replace(".", ","));
                Console.Write(" >> Amount: ");
                getAmount = Convert.ToInt32(Console.ReadLine().Trim());
                Console.Write(" >> Is it a medicine with a prescription [Y/N]: ");
                string isPrescription = Console.ReadLine().Trim().ToLower();
                if (isPrescription == "y")
                {
                    getWithPrescription = true;
                }
            }
            catch (FormatException e)
            {
                Console.Write("Wrong data format. " + e.Message);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.Write("Something went wrong... " + e.Message);
                Console.ReadLine();
            }
            return new Medicine(getName, getManufacturer, getPrice, getAmount, getWithPrescription);
        }

        public Medicine ReloadById(int idSellMedicine)
        {
            var SqlConnection = new SqlConnection(connectionString);
            SqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = SqlConnection;
            sqlCommand.CommandText = $"SELECT * FROM Medicines WHERE Id = @Id;";

            var sqIdParam = new SqlParameter
            {
                ParameterName = "@Id",
                Value = idSellMedicine,
                DbType = DbType.Int32
            };

            sqlCommand.Parameters.Add(sqIdParam);

            SqlDataReader sqlReader = sqlCommand.ExecuteReader();
            sqlReader.Read();

            Medicine medicine = new Medicine(sqlReader.GetInt32(0), sqlReader.GetString(1),
                sqlReader.GetString(2), sqlReader.GetDecimal(3), sqlReader.GetInt32(4), sqlReader.GetBoolean(5));
            return medicine;
        }

        public Medicine ReloadToEdit(string previousMedicineName)
        {
            Console.Clear();
            MainMenu.MenuS();

            string getManufacturer = "";
            decimal getPrice = 0;
            int getAmount = 0;
            bool getWithPrescription = false;

            try
            {
                ConsoleEx.WriteLine("".PadLeft(115, '='), ConsoleColor.DarkMagenta);
                ConsoleEx.WriteLine(" Please enter data of the medicine:", ConsoleColor.Gray);
                Console.Write(" >> Manufacturer: ");
                getManufacturer = Console.ReadLine().Trim();
                Console.Write(" >> Price: ");
                getPrice = decimal.Parse(Console.ReadLine().Trim().Replace(".", ","));
                Console.Write(" >> Amount: ");
                getAmount = Convert.ToInt32(Console.ReadLine().Trim());
                Console.Write(" >> Is it a medicine with a prescription [Y/N]: ");
                string isPrescription = Console.ReadLine().Trim().ToLower();
                if (isPrescription == "y")
                {
                    getWithPrescription = true;
                }
            }
            catch (FormatException e)
            {
                Console.Write("Wrong data format. " + e.Message);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.Write("Something went wrong... " + e.Message);
                Console.ReadLine();
            }
            return new Medicine(previousMedicineName, getManufacturer, getPrice, getAmount, getWithPrescription);
        }

        public override void Remove(int idDeleteMedicine)
        {
            try
            {
                var SqlConnection = new SqlConnection(connectionString);
                SqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = SqlConnection;
                sqlCommand.CommandText = $"DELETE FROM Medicines WHERE Id = @Id;";

                SqlParameter sqlIdParam = new SqlParameter();
                sqlIdParam.ParameterName = "@Id";
                sqlIdParam.Value = idDeleteMedicine;
                sqlIdParam.DbType = DbType.Int32;

                sqlCommand.Parameters.Add(sqlIdParam);

                sqlCommand.ExecuteNonQuery();
                SqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong..." + e.Message);
            }
        }

        public override int Save(Medicine medicine)
        {
            SqlConnection = Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = SqlConnection;
            sqlCommand.CommandText = $@"INSERT INTO Medicines (Name, Manufacturer, Price, Amount, WithPrescription) 
                                        VALUES (@Name, @Manufacturer, @Price, @Amount, @WithPrescription);
                                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var sqlNameParam = new SqlParameter
            {
                ParameterName = "@Name",
                Value = medicine.Name,
                DbType = DbType.AnsiString
            };

            var sqlManufacturerParam = new SqlParameter
            {
                ParameterName = "@Manufacturer",
                Value = medicine.Manufacturer,
                DbType = DbType.AnsiString
            };

            var sqlPriceParam = new SqlParameter
            {
                ParameterName = "@Price",
                Value = medicine.Price,
                DbType = DbType.Decimal
            };

            var sqlAmountParam = new SqlParameter
            {
                ParameterName = "@Amount",
                Value = medicine.Amount,
                DbType = DbType.Int32
            };

            var sqlWithPrescriptionParam = new SqlParameter
            {
                ParameterName = "@WithPrescription",
                Value = medicine.WithPrescription,
                DbType = DbType.Boolean
            };

            sqlCommand.Parameters.Add(sqlNameParam);
            sqlCommand.Parameters.Add(sqlManufacturerParam);
            sqlCommand.Parameters.Add(sqlPriceParam);
            sqlCommand.Parameters.Add(sqlAmountParam);
            sqlCommand.Parameters.Add(sqlWithPrescriptionParam);

            return (int)sqlCommand.ExecuteScalar();
        }

        public void EditSingleMedicine(int id, Medicine previousMedicine, Medicine currentMedicine)
        {

            Close();
            var SqlConnection = new SqlConnection(connectionString);
            SqlConnection.Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = SqlConnection;
            sqlCommand.CommandText = $"UPDATE Medicines SET Manufacturer = @Manufacturer, Price = @Price, " + // bez @Name
                                     $"Amount = @Amount, WithPrescription = @WithPrescription WHERE Id = @Id;";

            var sqIdParam = new SqlParameter
            {
                ParameterName = "@Id",
                Value = id,
                DbType = DbType.Int32
            };

            var sqlManufacturerParam = new SqlParameter
            {
                ParameterName = "@Manufacturer",
                Value = currentMedicine.Manufacturer,
                DbType = DbType.AnsiString
            };

            var sqlPriceParam = new SqlParameter
            {
                ParameterName = "@Price",
                Value = currentMedicine.Price,
                DbType = DbType.Decimal
            };

            var sqlAmountParam = new SqlParameter
            {
                ParameterName = "@Amount",
                Value = currentMedicine.Amount,
                DbType = DbType.Int32
            };

            var sqlWithPrescriptionParam = new SqlParameter
            {
                ParameterName = "@WithPrescription",
                Value = currentMedicine.WithPrescription,
                DbType = DbType.Boolean
            };

            sqlCommand.Parameters.Add(sqIdParam);
            sqlCommand.Parameters.Add(sqlManufacturerParam);
            sqlCommand.Parameters.Add(sqlPriceParam);
            sqlCommand.Parameters.Add(sqlAmountParam);
            sqlCommand.Parameters.Add(sqlWithPrescriptionParam);

            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }

        public static void UpdateSingleMedicine(int idUpdateMedicine, int amountUpdate)
        {
            var SqlConnection = new SqlConnection(connectionString);
            SqlConnection.Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = SqlConnection;
            sqlCommand.CommandText = $"UPDATE Medicines SET Amount = @Amount WHERE Id = @Id;";

            var sqIdParam = new SqlParameter
            {
                ParameterName = "@Id",
                Value = idUpdateMedicine,
                DbType = DbType.Int32
            };

            var sqAmountParam = new SqlParameter
            {
                ParameterName = "@Amount",
                Value = amountUpdate,
                DbType = DbType.Int32
            };

            sqlCommand.Parameters.Add(sqIdParam);
            sqlCommand.Parameters.Add(sqAmountParam);

            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }

        public static List<Medicine> ShowAllMedicines()
        {
            List<Medicine> medicinesList = new List<Medicine>();

            try
            {
                var SqlConnection = new SqlConnection(connectionString);
                SqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = SqlConnection;
                sqlCommand.CommandText = $"SELECT * FROM Medicines;";
                SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                while (sqlReader.HasRows && sqlReader.Read())
                {
                    medicinesList.Add(new Medicine(sqlReader.GetInt32(0), sqlReader.GetString(1),
                        sqlReader.GetString(2), sqlReader.GetDecimal(3), sqlReader.GetInt32(4), sqlReader.GetBoolean(5)));
                }
                SqlConnection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong..." + e.Message);
            }
            return medicinesList;
        }
    }
}
