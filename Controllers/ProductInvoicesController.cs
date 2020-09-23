using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabConsoleAplication;

namespace ZAD7.Controllers
{
    public class ProductInvoicesController : Controller
    {
        private readonly LabConsoleAplication.DbContext _context;

        public ProductInvoicesController(LabConsoleAplication.DbContext context)
        {
            _context = context;
        }

       // GET: ProductInvoices
        public async Task<IActionResult> Index()
        {
            var dbContext = _context.ProductInvoice.Include(p => p.Invoice).Include(p => p.Product);
            return View(await dbContext.ToListAsync());
        }

        // GET: ProductInvoices/Details/5
        public async Task<IActionResult> Details(int? productID, int? invoiceID)
        {
            if (productID == null || invoiceID == null)
            {
                return NotFound();
            }

            var productInvoice = await _context.ProductInvoice
                .Include(p => p.Invoice)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductID == productID && m.InvoiceID == invoiceID);
            if (productInvoice == null)
            {
                return NotFound();
            }

            return View(productInvoice);
        }

        // GET: ProductInvoices/Create
        public IActionResult Create()
        {
            ViewData["InvoiceID"] = new SelectList(_context.Invoices, "InvoiceNumber", "InvoiceNumber");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");
            return View();
        }

        // POST: ProductInvoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,InvoiceID,Quantity")] ProductInvoice productInvoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productInvoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceID"] = new SelectList(_context.Invoices, "InvoiceNumber", "InvoiceNumber", productInvoice.InvoiceID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", productInvoice.ProductID);
            return View(productInvoice);
        }

        // GET: ProductInvoices/Edit/5
        public async Task<IActionResult> Edit(int? productID, int? invoiceID)
        {
            if (productID == null || invoiceID == null)
            {
                return NotFound();
            }

            var productInvoice = await _context.ProductInvoice
                .Include(p => p.Invoice)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductID == productID && m.InvoiceID == invoiceID);
            if (productInvoice == null)
            {
                return NotFound();
            }
            ViewData["InvoiceID"] = new SelectList(_context.Invoices, "InvoiceNumber", "InvoiceNumber", productInvoice.InvoiceID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", productInvoice.ProductID);
            return View(productInvoice);
        }

        // POST: ProductInvoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int productID, int invoiceID, [Bind("ProductID,InvoiceID,Quantity")] ProductInvoice productInvoice)
        {
            if (productID != productInvoice.ProductID || invoiceID != productInvoice.InvoiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInvoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInvoiceExists(productInvoice.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceID"] = new SelectList(_context.Invoices, "InvoiceNumber", "InvoiceNumber", productInvoice.InvoiceID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", productInvoice.ProductID);
            return View(productInvoice);
        }

        // GET: ProductInvoices/Delete/5
        public async Task<IActionResult> Delete(int? productID, int? invoiceID)
        {
            if (productID == null || invoiceID == null)
            {
                return NotFound();
            }

            var productInvoice = await _context.ProductInvoice
                .Include(p => p.Invoice)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductID == productID && m.InvoiceID == invoiceID);
            if (productInvoice == null)
            {
                return NotFound();
            }

            return View(productInvoice);
        }

        // POST: ProductInvoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int productID, int invoiceID)
        {
            var productInvoice = await _context.ProductInvoice
                .Include(p => p.Invoice)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.ProductID == productID && m.InvoiceID == invoiceID);
            _context.ProductInvoice.Remove(productInvoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInvoiceExists(int id)
        {
            return _context.ProductInvoice.Any(e => e.ProductID == id);
        }
    }
}
