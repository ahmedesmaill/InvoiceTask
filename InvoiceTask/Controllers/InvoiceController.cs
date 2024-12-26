using Business_Logic.Services;
using Business_Logic.Services.IServices;
using Data_Access.Models;
using Data_Access.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceTask.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;
        }
        public IActionResult Index()
        {
            var invoices=invoiceService.GetAllInvoices();
            return View(invoices);
        }
        public IActionResult Create() 
        {
            var model = new InvoiceVM
            {
                Items = new List<InvoiceItemVM>() // Ensure that Items is initialized
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InvoiceVM invoiceVM)
        { 
            if(!ModelState.IsValid) return View(invoiceVM);
            invoiceService.CreateInvoice(invoiceVM);
            TempData["Message"] = "Invoice created successfully!";
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var invoice = invoiceService.GetInvoiceById(id);
            if (invoice == null)
            {
                return NotFound(); // Invoice not found
            }

            // Map invoice data to InvoiceVM
            var invoiceVM = new InvoiceVM
            {
                InvoiceId = invoice.InvoiceId,
                Date = invoice.Date ?? DateTime.Now,
                TotalAmount = invoice.TotalAmount ?? 0,
                Items = invoice.InvoiceDetails.Select(d => new InvoiceItemVM
                {
                    Product = d.Product,
                    Quantity = d.Quantity ?? 0,
                    Price = d.Price ?? 0
                }).ToList()
            };

            return View(invoiceVM);
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(InvoiceVM invoiceVM)
        {
            if (ModelState.IsValid)
            {
                // Check if at least one item is provided in the invoice
                if (invoiceVM.Items == null || !invoiceVM.Items.Any())
                {
                    ModelState.AddModelError("", "You must add at least one item to the invoice.");
                    return View(invoiceVM);
                }

                // Map InvoiceVM to Invoice model
                var invoice = new Invoice
                {
                    InvoiceId = invoiceVM.InvoiceId,
                    Date = invoiceVM.Date,
                    TotalAmount = invoiceVM.TotalAmount,
                    InvoiceDetails = invoiceVM.Items.Select(item => new InvoiceDetail
                    {
                        Product = item.Product,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };

                // Update the invoice
                invoiceService.UpdateInvoice(invoice);

                return RedirectToAction(nameof(Index)); // Redirect to the Index page after successful update
            }

            // If model validation fails, return the view with the current invoice data
            return View(invoiceVM);
        }

        public IActionResult Delete(int invoiceId)
        {
            var invoice = invoiceService.GetInvoiceById(invoiceId);

            if (invoice == null)
                return RedirectToAction("NotFound", "Home");
            invoiceService.DeleteInvoice(invoiceId);
            return RedirectToAction(nameof(Index));

        }

    }
}
