using Dsw2026Ej15.Data.Dtos;
using System.Text.Json;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
        private List<Speciality> _specialities = new List<Speciality>();
        private List<Doctor> _doctors = new List<Doctor>();

        public PersistenceInMemory()
        {
            LoadSpecialities();
        }
        private void LoadSpecialities()
        {
            try
            {
                var path = Path.Combine(AppContext.BaseDirectory, "Sources", "specialities.json");

                if (System.IO.File.Exists(path))
                {
                    var json = System.IO.File.ReadAllText(path);
                    var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    var list = System.Text.Json.JsonSerializer.Deserialize<List<Speciality>>(json, options);
                    if (list != null)
                    {
                        _specialities.AddRange(list);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando el archivo JSON: {ex.Message}");
            }
        }
        public async Task<Speciality?> GetSpecialityByIdAsync(Guid id)
        {
            var spec = _specialities.SingleOrDefault(x => x.Id == id);
            return await Task.FromResult(spec);
        }

        public async Task<List<Doctor>?> GetDoctorsActiveAsync()
        {
            var list = _doctors.Where(d => d.IsActive == true).ToList();
            return await Task.FromResult(list);
        }

        public async Task InsertarDoctorAsync(string name, string licenseNumber, Speciality speciality)
        {
            _doctors.Add(new Doctor(name, licenseNumber, speciality));
            await Task.CompletedTask; 
        }

        public async Task<Doctor?> GetDoctorActiveByIdAsync(Guid id)
        {
            var doc = _doctors.SingleOrDefault(d => d.Id == id && d.IsActive == true);
            return await Task.FromResult(doc);
        }

        public async Task<Doctor?> BajaLogicaDoctorByIdAsync(Guid id)
        {
            var doctor = await GetDoctorActiveByIdAsync(id);

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
    }
}