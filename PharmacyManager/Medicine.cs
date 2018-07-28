using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PharmacyManager
{
    public class Medicine : ActiveRecord
    {
        static string connectionString = @"Integrated Security=SSPI;" +
                                         "Initial Catalog=PharmacyManager;" +
                                         "Data Source=.\\SQLEXPRESS;";

        public override int Id { get; set; }
        public string Name { get; protected set; }
        public string Manufacturer { get; protected set; }
        public decimal Price { get; protected set; }
        public int Amount { get; protected set; }
        public bool WithPrescription { get; set; }

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

        public Medicine() { }

        public override void Reload() { }
        public override void Remove() { }
        public override void Save() { }

        public Medicine GetMedicineById(int idSellMedicine)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Something wen wrong..." + e.Message);
            }
            return null;
        }


        public int AddSingleMedicine(Medicine medicine)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
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
        }

        public void EditSingleMedicine(int id, Medicine previousMedicine, Medicine currentMedicine)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = $"UPDATE Medicines SET Name = @Name, Manufacturer = @Manufacturer, Price = @Price, " +
                                         $"Amount = @Amount, WithPrescription = @WithPrescription WHERE Id = @Id;";

                var sqIdParam = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = previousMedicine.Id,
                    DbType = DbType.Int32
                };

                var sqlNameParam = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = currentMedicine.Name,
                    DbType = DbType.AnsiString
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
                sqlCommand.Parameters.Add(sqlNameParam);
                sqlCommand.Parameters.Add(sqlManufacturerParam);
                sqlCommand.Parameters.Add(sqlPriceParam);
                sqlCommand.Parameters.Add(sqlAmountParam);
                sqlCommand.Parameters.Add(sqlWithPrescriptionParam);
                sqlConnection.Close();
            }
        }

        public static void UpdateSingleMedicine(int idUpdateMedicine, int amountUpdate)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
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
            }
        }

        public static void DeleteSingleMedicine(int idDeleteMedicine)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = $"DELETE FROM Medicines WHERE Id = @Id;";

                    SqlParameter sqlIdParam = new SqlParameter();
                    sqlIdParam.ParameterName = "@Id";
                    sqlIdParam.Value = idDeleteMedicine;
                    sqlIdParam.DbType = DbType.Int32;

                    sqlCommand.Parameters.Add(sqlIdParam);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something wen wrong..." + e.Message);
            }
        }

        public static List<Medicine> ShowAllMedicines()
        {
            List<Medicine> medicinesList = new List<Medicine>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandText = $"SELECT * FROM Medicines;";
                    SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                    while (sqlReader.HasRows && sqlReader.Read())
                    {
                        medicinesList.Add(new Medicine(sqlReader.GetInt32(0), sqlReader.GetString(1),
                            sqlReader.GetString(2), sqlReader.GetDecimal(3), sqlReader.GetInt32(4), sqlReader.GetBoolean(5)));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something wen wrong..." + e.Message);
            }
            return medicinesList;
        }
    }
}
