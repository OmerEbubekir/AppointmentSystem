using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string PatientName { get; set; }
    }

}
