using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistence
    {
        Task<Speciality?> GetSpecialityByIdAsync(Guid id);
        Task<List<Doctor>?> GetDoctorsActiveAsync();
        Task InsertarDoctorAsync(string name, string licenseNumber, Speciality speciality);
        Task<Doctor?> GetDoctorActiveByIdAsync(Guid id);
        Task<Doctor?> BajaLogicaDoctorByIdAsync(Guid id);
    }
}