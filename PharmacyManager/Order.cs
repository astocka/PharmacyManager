using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PharmacyManager
{
    public class Order : ActiveRecord
    {
        static string connectionString = @"Integrated Security=SSPI;" +
                                         "Initial Catalog=PharmacyManager;" +
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

        public override void Reload() { }
        public override void Remove() { }
        public override void Save() { }


        public static int SellSingleMedicine(int idSellmedicine, int amountSellMedicine, string customerName,
            string pesel, int prescriptionNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = $@"UPDATE Medicines SET Amount = @Amount WHERE Id = @Id;
                                        INSERT INTO Orders (Id, MedicineId, Date, Amount) VALUES 
                                        ((SELECT Id FROM Medicines WHERE Id = @Id),@Date, @Amount);
                                        INSERT INTO Prescriptions (CustomerName, Pesel, PrescriptionNumber) 
                                        VALUES (@CustomerName, @Pesel, @PrescriptionNumber);
                                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var sqlIdParam = new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = idSellmedicine,
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
                    Value = amountSellMedicine,
                    DbType = DbType.Int32
                };

                var sqlCustomerNameParam = new SqlParameter
                {
                    ParameterName = "@CustomerName",
                    Value = customerName,
                    DbType = DbType.AnsiString
                };

                var sqlPeselParam = new SqlParameter
                {
                    ParameterName = "@Pesel",
                    Value = pesel,
                    DbType = DbType.AnsiString
                };

                var sqlPrescriptionNumberParam = new SqlParameter
                {
                    ParameterName = "@PrescriptionNumber",
                    Value = prescriptionNumber,
                    DbType = DbType.Int32
                };

                sqlCommand.Parameters.Add(sqlIdParam);
                sqlCommand.Parameters.Add(sqlDateParam);
                sqlCommand.Parameters.Add(sqlAmountParam);
                sqlCommand.Parameters.Add(sqlCustomerNameParam);
                sqlCommand.Parameters.Add(sqlPeselParam);
                sqlCommand.Parameters.Add(sqlPrescriptionNumberParam);

                return (int)sqlCommand.ExecuteScalar();
            }
        }

        public static void SellWithoutPrescription(int idSellWithoutPrescription, int amountSellWithoutPrescription)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = $@"UPDATE Medicines SET Amount = @Amount WHERE Id = @Id;
                                            INSERT INTO Orders (Id, MedicineId, Date, Amount) VALUES 
                                            ((SELECT Id FROM Medicines WHERE Id = @Id),@Date, @Amount);
                                            SELECT CAST(SCOPE_IDENTITY() AS INT);";

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

                var sqlAmountParam = new SqlParameter
                {
                    ParameterName = "@Amount",
                    Value = amountSellWithoutPrescription,
                    DbType = DbType.Int32
                };

                sqlCommand.Parameters.Add(sqlIdParam);
                sqlCommand.Parameters.Add(sqlDateParam);
                sqlCommand.Parameters.Add(sqlAmountParam);
            }
        }
    }
}
