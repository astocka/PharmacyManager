using System.Data;
using System.Data.SqlClient;

namespace PharmacyManager
{
    public class Prescription : ActiveRecord
    {
        static string connectionString = @"Integrated Security=SSPI;" +
                                         "Initial Catalog=Pharmacy;" +
                                         "Data Source=.\\SQLEXPRESS;";

        public override int Id { get; set; }
        public string CustomerName { get; protected set; }
        public string Pesel { get; protected set; }
        public int PrescriptionNumber { get; protected set; }

        public Prescription(int id, string customerName, string pesel, int prescriptionNumber)
        {
            Id = id;
            CustomerName = customerName;
            Pesel = pesel;
            PrescriptionNumber = prescriptionNumber;
        }

        public Prescription(string customerName, string pesel, int prescriptionNumber)
        {
            CustomerName = customerName;
            Pesel = pesel;
            PrescriptionNumber = prescriptionNumber;
        }

        public Prescription()
        {
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

        public static int SavePrescription(string customerName, string pesel, int prescriptionNumber)
        {
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = $@"INSERT INTO Prescriptions (CustomerName, Pesel, PrescriptionNumber)
                                        VALUES (@CustomerName, @Pesel, @PrescriptionNumber);
                                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

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

            sqlCommand.Parameters.Add(sqlCustomerNameParam);
            sqlCommand.Parameters.Add(sqlPeselParam);
            sqlCommand.Parameters.Add(sqlPrescriptionNumberParam);

            return (int)sqlCommand.ExecuteScalar();
        }
    }
}
