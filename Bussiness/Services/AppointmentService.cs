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
    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentDbContext _context;

        public AppointmentService(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

     
        public async Task<List<Appointment>> GetUserAppointmentAsync(int userId)
        {
            return await _context.Appointments
                .Where(a => a.UserID == userId)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public async Task UpdateAsync(Appointment updated)
        {
            var existing = await _context.Appointments.FindAsync(updated.Id);
            if (existing != null)
            {
                existing.Title = updated.Title;
                existing.Description = updated.Description;
                existing.AppointmentDate = updated.AppointmentDate;
                existing.PatientName = updated.PatientName;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }


    }

}
