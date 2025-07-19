using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Interfaces;
using Data.Context;
using Data.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Services
{
    public class UserService : IUserService
    {
        private readonly AppointmentDbContext _context;

        public UserService(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return null;

            if (PasswordHelper.VerifyPassword(password, user.PasswordHash))
                return user;

            return null;
        }


        public async Task<bool> RegisterAsync(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            user.PasswordHash = PasswordHelper.HashPassword(password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<Appointment>> GetAvailableAppointmentsAsync(DateTime date)
        {
            return await _context.Appointments
        .Where(a => a.AppointmentDate.Date >= date.Date && a.UserID == null)
        .OrderBy(a => a.AppointmentDate)
        .ToListAsync();
        }
        public async Task<Appointment> BookAppointmentAsync(int userId, int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null || appointment.UserID != null)
            {
                return null; 
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            appointment.UserID = userId;
            appointment.PatientName = user.FirstName + " " + user.LastName;

            await _context.SaveChangesAsync();
            return appointment;
        }

    }
}
