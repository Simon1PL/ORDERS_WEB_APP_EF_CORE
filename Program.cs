using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabConsoleAplication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZAD7.Controllers;

namespace ZAD7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DbContext dbContext = new DbContext();
            if (!dbContext.Categories.Any())
            {
                dbContext.Categories.Add(new Category { CategoryID = 1, Name = "EMPTY" });
                dbContext.Companies.Add(new Supplier { CompanyID = 1, CompanyName = "EMPTY" });
                dbContext.Products.Add(new Product() { ProductID = 1, Name = "--Choose Product--", UnitsInStock = 1, CategoryID = 1, SupplierID = 1 });

                int productNumber = 0;
                Random rnd = new Random();

                List<Category> categories = new List<Category>() {
                    new Category { Name = "Artyku³y spo¿ywcze" },
                    new Category { Name = "Ksi¹¿ki" },
                    new Category { Name = "Inne" } 
                };
                List<Customer> customers = new List<Customer>() {
                    new Customer { CompanyName = "Klient1", City = "KRK", Street = "Kwiatowa", ZipCode = "30-100", Discount = 10 },
                    new Customer { CompanyName = "Klient2", City = "KRK", Street = "Kwiatowa", ZipCode = "30-100", Discount = 0 },
                    new Customer { CompanyName = "Klient3", City = "KRK", Street = "Kwiatowa", ZipCode = "30-100", Discount = 12 }
                };
                List<Supplier> suppliers = new List<Supplier>() {
                    new Supplier { CompanyName = "Dostawca1", City = "KRK", Street = "Kwiatowa", ZipCode = "30-100", BankAccountNumber = "1111111111" },
                    new Supplier { CompanyName = "Dostawca2", City = "KRK", Street = "Kwiatowa", ZipCode = "30-100", BankAccountNumber = "2222222222" },
                    new Supplier { CompanyName = "Dostawca3", City = "KRK", Street = "Kwiatowa", ZipCode = "30-100", BankAccountNumber = "3333333333" }
                };
                List<Product> products = new List<Product>();

                for (int i = 0; i < categories.Count; i++) {
                    products = new List<Product>() {
                    new Product {
                            Name = "Product" + ++productNumber,
                            UnitsInStock = 1,
                            Category = categories[rnd.Next(0, categories.Count())],
                            Supplier = suppliers[rnd.Next(0, suppliers.Count())]
                        },
                        new Product {
                            Name = "Product" + ++productNumber,
                            UnitsInStock = 5,
                            Category = categories[rnd.Next(0, categories.Count())],
                            Supplier = suppliers[rnd.Next(0, suppliers.Count())]
                        },
                        new Product {
                            Name = "Product" + ++productNumber,
                            UnitsInStock = 2,
                            Category = categories[rnd.Next(0, categories.Count())],
                            Supplier = suppliers[rnd.Next(0, suppliers.Count())]
                        }
                    };
                    dbContext.Categories.Add(categories[i]);
                    dbContext.Companies.Add(customers[i]);
                    dbContext.Companies.Add(suppliers[i]);
                    dbContext.Products.Add(products[0]);
                    dbContext.Products.Add(products[1]);
                    dbContext.Products.Add(products[2]);

                    Invoice invoice = new Invoice() { Customer = customers[0] };
                    invoice.Products.Add(new ProductInvoice() { Product = products[0], Quantity = 2 });
                    invoice.Products.Add(new ProductInvoice() { Product = products[1], Quantity = 1 });
                    dbContext.Invoices.Add(invoice);
                }
               
                dbContext.SaveChanges();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
