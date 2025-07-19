using Data.Entitys;
using System.Threading.Tasks;

namespace Bussiness.Interfaces
{
    public interface IUserService
    {
        //Giriş işlemleri
        Task<User?> LoginAsync(string userEmail, string password);
        Task<bool> RegisterAsync(User user, string password);

        //Randevu işlemleri 

        Task<List<Appointment>> GetAvailableAppointmentsAsync(DateTime date);
        Task<Appointment> BookAppointmentAsync(int userId, int appointmentId);

    }
}
