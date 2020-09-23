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
    public class InvoicesController : Controller
    {
        private readonly LabConsoleAplication.DbContext _context;

        public InvoicesController(LabConsoleAplication.DbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invoices.Include(i => i.Customer).Include(i => i.Products).ThenInclude(p => p.Product).ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.Include(i => i.Customer).Include(i => i.Products).ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(m => m.InvoiceNumber == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Invoice invoice;
            if (id == null)
            {
                invoice = new Invoice();
            }
            else
            {
                invoice = _context.Invoices.Include(i => i.Customer).Include(i => i.Products).ThenInclude(p => p.Product).FirstOrDefault(i => i.InvoiceNumber == id);
            }
            if (invoice == null)
            {
                return NotFound();
            }
            invoice.SelectedProducts = invoice.Products.OrderByDescending(p => p.ProductID).ToArray();
            if (invoice.SelectedProducts.Length == 0)
            {
                invoice.SelectedProducts = new ProductInvoice[1];
                invoice.SelectedProducts[0] = new ProductInvoice() { InvoiceID = invoice.InvoiceNumber };
            }
            ViewBag.AvailableProducts = _context.Products.ToList();
            ViewBag.AvailableCustomers = _context.Customer.ToList();
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Invoice invoice)
        {
            if (id != invoice.InvoiceNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_context.Invoices.Any(i => i.InvoiceNumber == invoice.InvoiceNumber))
                    {
                        _context.Invoices.Add(invoice);
                        _context.SaveChanges();
                        id = invoice.InvoiceNumber;
                        foreach (ProductInvoice productInvoice in invoice.SelectedProducts)
                        {
                            productInvoice.InvoiceID = invoice.InvoiceNumber;
                        }
                    }
                    var RemovedPositions = _context.ProductInvoice.Where(pi => pi.InvoiceID == id).ToList().Where(pi => !invoice.SelectedProducts.Any(sp => sp.InvoiceID == pi.InvoiceID && sp.ProductID == pi.ProductID));
                    foreach (ProductInvoice position in RemovedPositions)
                    {
                        _context.ProductInvoice.Remove(position);
                    }
                    foreach (ProductInvoice position in invoice.SelectedProducts)
                    {
                        ProductInvoice oldPosition = _context.ProductInvoice.Where(pi => pi.InvoiceID == position.InvoiceID && pi.ProductID == position.ProductID).SingleOrDefault();
                        if (oldPosition == null)
                        {
                            _context.ProductInvoice.Add(position);
                        }
                        else if (oldPosition.Quantity != position.Quantity)
                        {
                            oldPosition.Quantity = position.Quantity;
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceNumber))
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
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.Include(i => i.Customer).Include(i => i.Products).ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(m => m.InvoiceNumber == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveProduct(int id, Invoice invoice)
            {
            await Edit(invoice.InvoiceNumber, invoice);
            return RedirectToAction(nameof(Edit), invoice.InvoiceNumber);
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.InvoiceNumber == id);
        }
    }
}
