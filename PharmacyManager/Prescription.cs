using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PharmacyManager
{
    public class Prescription : ActiveRecord
    {
        static string connectionString = @"Integrated Security=SSPI;" +
                                         "Initial Catalog=PharmacyManager;" +
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

        public override void Reload() { }
        public override void Remove() { }
        public override void Save() { }
    }
}
