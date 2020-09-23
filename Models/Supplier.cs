using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabConsoleAplication
{
    public class Supplier: Company
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }
        public string BankAccountNumber { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
