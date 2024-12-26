using System;
using System.Collections.Generic;

namespace Data_Access.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? Date { get; set; }

    public virtual List<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
