using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using LabConsoleAplication;

namespace LabConsoleAplication
{
    public class DbContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ProductInvoice> ProductInvoice { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Primary keys
            builder.Entity<ProductInvoice>().HasKey(q =>
                new {
                    q.ProductID,
                    q.InvoiceID
                });

            // Relationships
            builder.Entity<ProductInvoice>()
                .HasOne(pi => pi.Invoice)
                .WithMany(i => i.Products)
                .HasForeignKey(pi => pi.InvoiceID);

            builder.Entity<ProductInvoice>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Invoices)
                .HasForeignKey(pi => pi.ProductID);

            // Inheritance (Company - Discriminator Column)
            builder.Entity<Company>()
                .HasDiscriminator<string>("CompanyType")
                .HasValue<Supplier>("Supplier")
                .HasValue<Customer>("Customer");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseLazyLoadingProxies().UseSqlite("DataSource = DB.db");
        public DbSet<LabConsoleAplication.Customer> Customer { get; set; }
        public DbSet<LabConsoleAplication.Supplier> Supplier { get; set; }
    }
}
