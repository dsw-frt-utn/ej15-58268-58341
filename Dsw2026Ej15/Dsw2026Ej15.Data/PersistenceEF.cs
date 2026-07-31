using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Dsw2026Ej15.Data.Util;

namespace Dsw2026Ej15.Data
{
    public class PersistenceEf : IPersistence
    {
        private readonly Dsw2026Ej16DbContext _context;

        public PersistenceEf(Dsw2026Ej16DbContext context)
        {
            _context = context;
            InicializarDatos();
        }

        public async Task<Speciality?> GetSpecialityById(Guid id)
        {
            //return _specialities.SingleOrDefault(s => s.Id == id); este es con la memoria

            return await _context.Specialities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task<List<Doctor>?> GetDoctorsActive()
        {
            //return _doctors.Where(d => d.IsActive == true).ToList();este es con la memoria

            return await _context.Doctors
                .Include(nameof(Doctor.Speciality)) //include es como hacer un join, en este caso para traer la especialidad del doctor ya que la respuesta manda el nombre de la especialidad, y doctor tiene como atributo la clase especialidad
                .Where(d => d.IsActive)
                .ToListAsync();
        }
        public async Task InsertarDoctor(string name, string licenseNumber, Speciality speciality)
        {
            //_doctors.Add(new Doctor(name, licenseNumber, speciality)); // en memoria

            Doctor doctor = new Doctor(name, licenseNumber, speciality);

            _context.Add(doctor); //infiere que nos referimos al DbSet<Doctor> y lo agrega

            //_context.Doctors.Add(doctor); 
            //_context.Add<Doctor>(doctor);
            //_context.Set<Doctor>().Add(doctor); // es util cuando trabajamos con codigo generico, en este caso no es necesario

            await _context.SaveChangesAsync();
        }
        public async Task<Doctor?> GetDoctorActiveById(Guid id)
        {
            //return _doctors.SingleOrDefault(d => d.Id == id && d.IsActive == true); este es con la memoria

            return await _context.Doctors
                .Include(d => d.Speciality)
                .SingleOrDefaultAsync(d => d.Id == id && d.IsActive);
        }
        public async Task BajaLogicaDoctorById(Guid id)
        {
            /*var doctor = GetDoctorActiveById(id);
            doctor?.Desactivate();*/ // este es con la memoria

            var doctor = await GetDoctorActiveById(id);

            if (doctor != null)
            {
                doctor.Desactivate();
                _context.Update(doctor);
                await _context.SaveChangesAsync();
            }
        }

        public void InicializarDatos()
        {
            _context.Seedwork<Speciality>("specialities");
            _context.Seedwork<Doctor>("doctors");
        }
    }
}