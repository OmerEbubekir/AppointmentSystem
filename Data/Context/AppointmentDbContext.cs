using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppointmentDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options) { }

        
    }
}
