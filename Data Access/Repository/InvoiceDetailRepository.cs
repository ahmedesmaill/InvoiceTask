using Data_Access.Data;
using Data_Access.Models;
using Data_Access.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Repository
{
    public class InvoiceDetailRepository : Repository<InvoiceDetail>,IInvoiceDetailRepository
    {
        private readonly ApplicationDbContext dbContext;

        public InvoiceDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
