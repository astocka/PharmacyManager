using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace PharmacyManager
{
    public abstract class ActiveRecord
    {
        static string connectionString = @"Integrated Security=SSPI;" +
                                          "Initial Catalog=PharmacyManager;" +
                                          "Data Source=.\\SQLEXPRESS;";

        public abstract int Id { get; set; }

        public abstract void Save();

        public abstract void Reload();

        public abstract void Remove();

        protected virtual void Open() { }

        protected virtual void Close() { }
    }
}
