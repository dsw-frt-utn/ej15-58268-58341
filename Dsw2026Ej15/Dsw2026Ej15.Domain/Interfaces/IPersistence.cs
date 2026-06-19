using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistence
    {
        public Speciality? GetSpecialityById(Guid id);
        public List<Doctor> GetDoctorsActive();
        public void InsertarDoctor(string name, string licenseNumber, Speciality speciality);
        public Doctor? GetDoctorActiveById(Guid id);
        public Doctor? BajaLogicaDoctorById(Guid id);

    }
}
