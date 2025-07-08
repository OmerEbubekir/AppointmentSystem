using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entitys;


namespace Bussiness.Interfaces
{
    public interface IAdminService
    {
        // Kullanıcı işlemleri
        Task<List<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId); 
        Task UpdateUserRoleAsync(User user);
        Task<User?> GetUserDetailsAsync(int userId);

        // Randevu yönetimi
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<bool> EditAppointmentAsync(Appointment appointment);

        // Genel sistem yönetimi için metodlar eklenebilir
    }
}

