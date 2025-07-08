using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entitys;

namespace Bussiness.Interfaces
{
    public interface IAppointmentService
    {
        Task CreateAsync(Appointment appointment);
        Task<List<Appointment>> GetUserAppointmentAsync(int  userId);

        Task<Appointment?> GetByIdAsync(int id);
        Task UpdateAsync(Appointment updated);
        Task DeleteAsync(int id);
    }
}
