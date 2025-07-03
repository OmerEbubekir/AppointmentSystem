using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Adınızı girin")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyadınızı girin")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email girin")]
        [EmailAddress(ErrorMessage = "Geçerli bir email girin")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre girin")]
        public string Password { get; set; }
    }
}

