﻿using System.ComponentModel.DataAnnotations;

namespace InvoiceTask.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string? RecaptchaToken { get; set; }
    }
}
