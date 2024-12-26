using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data_Access.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal? TotalAmount { get; set; }
    [Required]
    public DateTime? Date { get; set; }
    [Required]

    public virtual List<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
