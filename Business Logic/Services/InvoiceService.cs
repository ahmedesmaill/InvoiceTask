using Business_Logic.Services.IServices;
using Data_Access.Models;
using Data_Access.Repository;
using Data_Access.Repository.IRepository;
using Data_Access.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IInvoiceDetailRepository detailRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository,IInvoiceDetailRepository detailRepository)
        {
            this.invoiceRepository = invoiceRepository;
            this.detailRepository = detailRepository;
        }

       
        public IEnumerable<Invoice> GetAllInvoices()
        {
            return invoiceRepository.Get(includeProps:[e=>e.InvoiceDetails]).ToList();
        }
        public Invoice? GetInvoiceById(int id)
        {
            var invoice = invoiceRepository.GetOne(includeProps:[e =>e.InvoiceDetails],expression: i => i.InvoiceId == id);
            if (invoice == null) return null;

            return new Invoice
            {
                InvoiceId = invoice.InvoiceId,
                Date = invoice.Date,
                TotalAmount = invoice.TotalAmount,
                InvoiceDetails = invoice.InvoiceDetails.Select(item => new InvoiceDetail
                {
                    Product = item.Product,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
        }
        public Invoice? GetInvoiceDetailById(int id)
        {
            var invoiceDetail = invoiceRepository.GetOne(includeProps:[ e => e.InvoiceDetails] , expression: i => i.InvoiceId == id);
            if (invoiceDetail == null) return null;
            return invoiceDetail;
        }
        public void CreateInvoice(InvoiceVM invoiceVM)
        {
            var invoice = new Invoice
            {
                Date = invoiceVM.Date,
                TotalAmount = invoiceVM.TotalAmount,
                InvoiceDetails = invoiceVM.Items.Select(item => new InvoiceDetail
                {
                    Product = item.Product,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            invoiceRepository.Create(invoice);
            invoiceRepository.Commit();
        }

        public void UpdateInvoice(InvoiceVM invoiceVM)
        {
            if (invoiceVM.Items == null || !invoiceVM.Items.Any())
            {
                throw new ArgumentException("You must add at least one item to the invoice.");
            }

            // Fetch the existing invoice including its details
            var invoice = invoiceRepository.GetOne(expression: i => i.InvoiceId == invoiceVM.InvoiceId, includeProps: [e => e.InvoiceDetails]);
            if (invoice == null)
            {
                throw new ArgumentException("Invoice not found.");
            }

            // Update invoice fields
            invoice.Date = invoiceVM.Date;
            invoice.TotalAmount = invoiceVM.TotalAmount;

            // Clear the existing InvoiceDetails from the database
            invoice.InvoiceDetails.Clear();

            // Add the updated InvoiceDetails from the view model
            invoice.InvoiceDetails.AddRange(invoiceVM.Items.Select(item => new InvoiceDetail
            {
                Product = item.Product,
                Quantity = item.Quantity,
                Price = item.Price
            }));

            // Save changes to the database
            invoiceRepository.Edit(invoice);
            invoiceRepository.Commit();
        }



        public void DeleteInvoice(int id)
        {
            var invoice = invoiceRepository.GetOne(expression: i => i.InvoiceId == id);
            if (invoice != null)
            {
                invoiceRepository.Delete(invoice);
                invoiceRepository.Commit();
            }
        }
    }
}