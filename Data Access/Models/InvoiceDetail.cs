using System;
using System.Collections.Generic;

namespace Data_Access.Models;

public partial class InvoiceDetail
{
    public int Id { get; set; }

    public int? InvoiceId { get; set; }

    public string? Product { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
