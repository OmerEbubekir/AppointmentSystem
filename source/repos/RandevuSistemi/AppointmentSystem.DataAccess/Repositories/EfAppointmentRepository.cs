using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppointmentSystem.DataAccess.Repositories
{
    public class EfAppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;

        public EfAppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAsync()
            => await _context.Appointments.ToListAsync();

        public async Task<Appointment> GetByIdAsync(int id)
            => await _context.Appointments.FindAsync(id);

        public async Task AddAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
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
