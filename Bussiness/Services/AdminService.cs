using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Interfaces;
using Data.Context;
using Data.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppointmentDbContext _context;

        public AdminService(AppointmentDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            var person = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Appointments = new List<Appointment>(),
                PasswordHash = PasswordHelper.HashPassword(user.PasswordHash), // 👈 Şifre hashleniyor
                Role = user.Role ?? "User"
            };

            await _context.Users.AddAsync(person);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> DeleteUserAsync(int userId)
        {
            var person = await _context.Users.FindAsync(userId);
            if (person == null)
                return false;

            _context.Users.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> EditAppointmentAsync(Appointment appointment)
        {
            var existingAppointment = await _context.Appointments.FirstOrDefaultAsync(u => u.Id == appointment.Id);
            if (existingAppointment != null)
            {
                existingAppointment.User = appointment.User;
                existingAppointment.AppointmentDate = appointment.AppointmentDate.Date;
                existingAppointment.PatientName = appointment.PatientName;
                existingAppointment.Description = appointment.Description;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return _context.Appointments.ToListAsync();
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserDetailsAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotImplementedException();
            }
            return user;
        }

        public async Task UpdateUserRoleAsync(User user)
        {
            var person = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (person == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }
            else
            {
                person.Role = user.Role;
                
                await _context.SaveChangesAsync();
            }
        }

    }
}
