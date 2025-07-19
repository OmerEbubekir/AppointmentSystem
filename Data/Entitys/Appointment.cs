using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.Entitys
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]

        public DateTime AppointmentDate { get; set; }

        
        public string? PatientName { get; set; }
        public int? UserID { get; set; }
        public User? User { get; set; }
        public string Description { get; set; }
    }
}
