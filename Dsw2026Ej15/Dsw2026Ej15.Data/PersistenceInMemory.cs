using Dsw2026Ej15.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.Json;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;

namespace Dsw2026Ej15.Data
{
    
    public class PersistenceInMemory : IPersistence
    {
        private List<Speciality> _specialities = new List<Speciality>();
        private List<Doctor> _doctors = new List<Doctor>();

        public PersistenceInMemory() {
            LoadSpecialities();
        }

        public Speciality? GetSpecialityById(Guid id) 
        {
            return _specialities.SingleOrDefault(x => x.Id == id);
        }

        public List<Doctor>? GetDoctorsActive()
        {
            return _doctors.Where(d => d.IsActive == true).ToList();
        }

        public void InsertarDoctor(string name, string licenseNumber, Speciality speciality)
        {
            _doctors.Add(new Doctor(name, licenseNumber, speciality));
        }

        public Doctor? GetDoctorActiveById(Guid id)
        {
            return _doctors.SingleOrDefault(d => d.Id == id && d.IsActive == true);
        }

        public Doctor? BajaLogicaDoctorById(Guid id)
        {
            var doctor = GetDoctorActiveById(id);

            if (doctor == null)
            {
                return null;
            }
            else
            {
                doctor.IsActive = false;
                return doctor;
            }
        }
        private void LoadSpecialities()
        {
            try 
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources", "specialities.json");
                var json = File.ReadAllText(jsonPath);
                var specialities = JsonSerializer.Deserialize<List<SpecialityDto>>(json,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    }) ?? [];
                _specialities = [.. specialities.Select(s=>new Speciality(s.Name,s.Description,s.Id))];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


}
}
}
