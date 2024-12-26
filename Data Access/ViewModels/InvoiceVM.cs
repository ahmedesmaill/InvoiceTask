using System.ComponentModel.DataAnnotations;

namespace Data_Access.ViewModels
{
    public class InvoiceVM
    {
        public int InvoiceId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        [Required]
        public List<InvoiceItemVM> Items { get; set; } = new List<InvoiceItemVM>();

    }
}
