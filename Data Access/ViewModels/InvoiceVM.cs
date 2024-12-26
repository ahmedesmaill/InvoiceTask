namespace Data_Access.ViewModels
{
    public class InvoiceVM
    {
        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceItemVM> Items { get; set; } = new List<InvoiceItemVM>();

    }
}
