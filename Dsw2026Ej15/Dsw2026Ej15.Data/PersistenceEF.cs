using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Dsw2026Ej15.Data.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Data
{
    public class PersistenceEF : IPersistence
    {
        private readonly Dsw2026Ej16DbContext _context;

        public PersistenceEF(Dsw2026Ej16DbContext context)
        {
            _context = context;
            InicializarDatos();
        }

             
        public async Task<Speciality?> GetSpecialityByIdAsync(Guid id)
        {
            return await _context.Specialities.SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Doctor>?> GetDoctorsActiveAsync()
        {
            return await _context.Doctors
                .Include(nameof(Doctor.Speciality))
                .Where(d=>d.IsActive)
                .ToListAsync();
        }

        public async Task InsertarDoctorAsync(string name, string licenseNumber, Speciality speciality)
        {
            //_doctors.Add(new Doctor(name, licenseNumber, speciality)); // en memoria

            Doctor doctor = new Doctor(name, licenseNumber, speciality);

            _context.Add(doctor); 

            await _context.SaveChangesAsync();
        }

        public async Task<Doctor?> GetDoctorActiveByIdAsync(Guid id)
        {
            return await _context.Doctors
                .Include(d => d.Speciality)
                .SingleOrDefaultAsync(d => d.Id == id && d.IsActive);
        }

        public async Task BajaLogicaDoctorByIdAsync(Guid id)
        {
            var doctor = await GetDoctorActiveByIdAsync(id);

            if (doctor != null)
            {
                doctor.Deactivate();
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
