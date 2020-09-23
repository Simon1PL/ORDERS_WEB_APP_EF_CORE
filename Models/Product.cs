using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabConsoleAplication
{
    public class Product
    {
        public Product()
        {
            Invoices = new HashSet<ProductInvoice>();
        }
        public int ProductID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int UnitsInStock { get; set; }
        [Required]
        public int SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }
        [Required(ErrorMessage = "Produkt musi mieć kategorię, możesz wybrać kategorię \"Inne\"")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductInvoice> Invoices { get; set; }
    }
}
