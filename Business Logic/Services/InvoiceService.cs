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
        public InvoiceDetail? GetInvoiceDetailById(int id)
        {
            var invoiceDetail = detailRepository.GetOne( expression: i => i.Id == id);
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

        public void UpdateInvoice(Invoice invoice)
        {
            
            
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