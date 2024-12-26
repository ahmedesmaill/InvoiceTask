using Data_Access.Models;
using Data_Access.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Services.IServices
{
    public interface IInvoiceService
    {
        IEnumerable<Invoice> GetAllInvoices();
        Invoice? GetInvoiceById(int id);
       
        Invoice? GetInvoiceDetailById(int id);
        void CreateInvoice(InvoiceVM invoiceVM);
        void UpdateInvoice(InvoiceVM invoiceVM);
        void DeleteInvoice(int id);
      
    }
}
