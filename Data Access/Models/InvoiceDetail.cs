using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data_Access.Models;

public partial class InvoiceDetail
{
    public int Id { get; set; }
    [Required]
    public int? InvoiceId { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    public string? Product { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int? Quantity { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal? Price { get; set; }
    [ValidateNever]
    public virtual Invoice? Invoice { get; set; }
}

