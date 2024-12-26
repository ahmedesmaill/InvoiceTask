using System.ComponentModel.DataAnnotations;

namespace InvoiceTask.ViewModels
{
    public class LoginVM
    {
        public string Username { get; set; }
        public string Password { get; set; }

        [Required]
        public string? RecaptchaToken { get; set; }
    }
}
