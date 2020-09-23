using System;
using System.ComponentModel.DataAnnotations;

namespace LabConsoleAplication
{
    public class ProductInvoice
    {
        [Required]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int InvoiceID { get; set; }

        public virtual Invoice Invoice { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Ilość nie może być mniejsza od 1")]
        public int Quantity { get; set; }
    }
}