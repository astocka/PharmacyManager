using System.Data.SqlClient;

namespace PharmacyManager
{
    public abstract class ActiveRecord
    {
        private static string connectionString = @"Integrated Security=SSPI;" +
                                                 "Data Source=.\\SQLEXPRESS;" +
                                                 "Initial Catalog=Pharmacy;";

        public abstract int Id { get; set; }

        public abstract int Save(Medicine medicine);

        public abstract Medicine Reload();

        public abstract void Remove(int idDeleteMedicine);

        protected virtual SqlConnection Open()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        protected virtual void Close()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Close();
        }
    }
}
