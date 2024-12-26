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


        // Display the Create page
        public IActionResult Create() 
        {
            var model = new InvoiceVM
            {
                Items = new List<InvoiceItemVM>() 
            };
            return View(model);
        }
         

        // Handle Create post 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InvoiceVM invoiceVM)
        { 
            if(!ModelState.IsValid) return View(invoiceVM);
            invoiceService.CreateInvoice(invoiceVM);
            TempData["Message"] = "Invoice created successfully!";
            return RedirectToAction("Index");
        }
         

        // Display the Edit page
        public IActionResult Edit(int id)
        {
            var invoice = invoiceService.GetInvoiceById(id);
            if (invoice == null)
            {
                return NotFound();
            }

            // Map to ViewModel
            var invoiceVM = new InvoiceVM
            {
                InvoiceId = invoice.InvoiceId,
                Date = invoice.Date ?? DateTime.Now,
                TotalAmount = invoice.TotalAmount ?? 0,
                Items = invoice.InvoiceDetails.Select(detail => new InvoiceItemVM
                {
                    Product = detail.Product,
                    Quantity = detail.Quantity ?? 0,
                    Price = detail.Price ?? 0
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
                try
                {
                    // Update invoice via the service
                    invoiceService.UpdateInvoice(invoiceVM);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // If validation fails, ensure the model is not duplicated
            if (invoiceVM.Items == null || !invoiceVM.Items.Any())
            {
                invoiceVM.Items = new List<InvoiceItemVM>(); 
            }

            return View(invoiceVM);
        }


        // Display the Details page
        public IActionResult Details(int invoiceId) 
        {
            var invoice = invoiceService.GetInvoiceById(invoiceId);

            if (invoice == null)
            {
                return NotFound(); 
            }

            return View(invoice);
        }


        // Handle Delete
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
