using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppointmentSystem.DataAccess;
using AppointmentSystem.DataAccess.Repositories;
using AppointmentSystem.Entities;

namespace AppointmentSystem.Business.Services
{
    public class AppointmentManager : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentManager(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Appointment>> GetAllAsync() => _repository.GetAllAsync();

        public Task<Appointment> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(Appointment appointment) => _repository.AddAsync(appointment);

        public Task UpdateAsync(Appointment appointment) => _repository.UpdateAsync(appointment);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }

}
