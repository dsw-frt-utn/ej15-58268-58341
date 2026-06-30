using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Data.Utils;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace Dsw2026Ej15.Data
{
    public class PersistenceEF : IPersistence
    {
        private readonly Dsw2026Ej16DbContext _context;

        public PersistenceEF(Dsw2026Ej16DbContext context)
        {
            _context = context;
            InitializeData();
        }

        public async Task<Speciality?> GetSpecialityByIdAsync(Guid id)
        {
            return await _context.Specialities.SingleOrDefaultAsync(s => s.Id == id);

        }

        public async Task<List<Doctor>?> GetDoctorsActiveAsync()
        {
            return await _context.Doctors.Include(nameof(Doctor.Speciality)).Where(d => d.IsActive).ToListAsync();

        }

        public async Task InsertarDoctorAsync(string name, string licenseNumber, Speciality speciality)
        {
           _context.Doctors.Add(new Doctor(name, licenseNumber, speciality));
           await _context.SaveChangesAsync();
        }

        public async Task<Doctor?> GetDoctorActiveByIdAsync(Guid id)
        {
            return await _context.Doctors.Include(d => d.Speciality).SingleOrDefaultAsync(d => d.Id == id && d.IsActive);
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
        public void InitializeData()
        {
            _context.Seedwork<Speciality>("specialities");
            _context.Seedwork<Doctor>("doctors");

        }

    }
    }