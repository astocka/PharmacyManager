using System;
using System.Data;
using System.Data.SqlClient;

namespace PharmacyManager
{
    public class Order : ActiveRecord
    {
        static string connectionString = @"Integrated Security=SSPI;" +
                                         "Initial Catalog=Pharmacy;" +
                                         "Data Source=.\\SQLEXPRESS;";

        public override int Id { get; set; }
        public int? PrescriptionId { get; protected set; }
        public int MedicineId { get; protected set; }
        public DateTime Date { get; protected set; }
        public int Amount { get; protected set; }

        public Order(int id, int? prescriptionId, int medicineId, DateTime date, int amount)
        {
            Id = id;
            PrescriptionId = prescriptionId;
            MedicineId = medicineId;
            Date = date;
            Amount = amount;
        }

        public override Medicine Reload()
        {
            Medicine medicine = new Medicine();
            return medicine;
        }
        public override void Remove(int idDeleteMedicine) { }

        public override int Save(Medicine medicine)
        {
            return 0;
        }

        public static void Sell(int idSellWithoutPrescription, int amountSellWithoutPrescription, int currentAmount)
        {
            try
            {
                if (currentAmount > 0)
                {
                    var SqlConnection = new SqlConnection(connectionString);
                    SqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = SqlConnection;
                    sqlCommand.CommandText = $@"BEGIN TRANSACTION; UPDATE Medicines SET Amount = @AmountUpdate WHERE Id = @Id;
                                                INSERT INTO Orders (MedicineId, Date, Amount) VALUES 
                                                ((SELECT Id FROM Medicines WHERE Id = @Id), @Date, @Amount); COMMIT;";

                    var sqlIdParam = new SqlParameter
                    {
                        ParameterName = "@Id",
                        Value = idSellWithoutPrescription,
                        DbType = DbType.Int32
                    };

                    var sqlDateParam = new SqlParameter
                    {
                        ParameterName = "@Date",
                        Value = DateTime.Now,
                        DbType = DbType.DateTime
                    };

                    var sqlAmountUpdateParam = new SqlParameter
                    {
                        ParameterName = "@AmountUpdate",
                        Value = currentAmount - amountSellWithoutPrescription,
                        DbType = DbType.Int32
                    };

                    var sqlAmountParam = new SqlParameter
                    {
                        ParameterName = "@Amount",
                        Value = amountSellWithoutPrescription,
                        DbType = DbType.Int32
                    };

                    sqlCommand.Parameters.Add(sqlIdParam);
                    sqlCommand.Parameters.Add(sqlDateParam);
                    sqlCommand.Parameters.Add(sqlAmountUpdateParam);
                    sqlCommand.Parameters.Add(sqlAmountParam);

                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong" + e.Message);
            }
        }

        public static void SellMedicine(int idSellMedicine, int amountSellMedicine, int currentAmount)
        {
            if (currentAmount > 0)
            {
                var SqlConnection = new SqlConnection(connectionString);
                SqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = SqlConnection;
                sqlCommand.CommandText = $@"UPDATE Medicines SET Amount = @AmountUpdate WHERE Id = @Id;";

                var sqlIdParam = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = idSellMedicine,
                    DbType = DbType.Int32
                };

                var sqlAmountUpdateParam = new SqlParameter
                {
                    ParameterName = "@AmountUpdate",
                    Value = currentAmount - amountSellMedicine,
                    DbType = DbType.Int32
                };

                sqlCommand.Parameters.Add(sqlIdParam);
                sqlCommand.Parameters.Add(sqlAmountUpdateParam);

                sqlCommand.ExecuteNonQuery();
            }
        }

        public static void UpdateOrders(int prescriptionId, int medicineId, int amount)
        {
            var SqlConnection = new SqlConnection(connectionString);
            SqlConnection.Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = SqlConnection;
            sqlCommand.CommandText = $@"INSERT INTO Orders (PrescriptionId, MedicineId, Date, Amount) 
                                        VALUES (@PrescriptionId, @MedicineId, @Date, @Amount);";

            var sqlPrescriptionIdParam = new SqlParameter
            {
                ParameterName = "@PrescriptionId",
                Value = prescriptionId,
                DbType = DbType.Int32
            };

            var sqlMedicineIdParam = new SqlParameter
            {
                ParameterName = "@MedicineId",
                Value = medicineId,
                DbType = DbType.Int32
            };

            var sqlDateParam = new SqlParameter
            {
                ParameterName = "@Date",
                Value = DateTime.Now,
                DbType = DbType.DateTime
            };

            var sqlAmountParam = new SqlParameter
            {
                ParameterName = "@Amount",
                Value = amount,
                DbType = DbType.Int32
            };
            sqlCommand.Parameters.Add(sqlPrescriptionIdParam);
            sqlCommand.Parameters.Add(sqlMedicineIdParam);
            sqlCommand.Parameters.Add(sqlDateParam);
            sqlCommand.Parameters.Add(sqlAmountParam);

            sqlCommand.ExecuteNonQuery();
            SqlConnection.Close();
        }
    }
}
