using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabConsoleAplication
{
    public class Invoice
    {
        public Invoice()
        {
            Products = new HashSet<ProductInvoice>();
        }
        [Key]
        public int InvoiceNumber { get; set; }
        public virtual ICollection<ProductInvoice> Products { get; set; }
        [NotMapped]
        public ProductInvoice[] SelectedProducts { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
