using System;
using System.Collections.Generic;
using System.Text;
using Dsw2026Ej15.Domain.Entities;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistence
    {
        Task<Speciality?> GetSpecialityById(Guid id);
        Task<List<Doctor>?> GetDoctorsActive();
        Task InsertarDoctor(string name, string licenseNumber, Speciality speciality);
        Task<Doctor?> GetDoctorActiveById(Guid id);
        Task BajaLogicaDoctorById(Guid id);
    }
}