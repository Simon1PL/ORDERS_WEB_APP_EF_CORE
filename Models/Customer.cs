using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabConsoleAplication
{
    public class Customer: Company
    {
        public int Discount { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
