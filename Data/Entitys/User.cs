using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entitys
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad gereklidir")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string? Role { get; set; } = "User";

        public ICollection<Appointment> Appointments { get; set; }=new List<Appointment>();

    }
}
