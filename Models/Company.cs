using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabConsoleAplication
{
    public abstract class Company
    {
        public int CompanyID { get; set; }
        [Required(ErrorMessage = "Musisz podać nazwę firmy")]
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}
