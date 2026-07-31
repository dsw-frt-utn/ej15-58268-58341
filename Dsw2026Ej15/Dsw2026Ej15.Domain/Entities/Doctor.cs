using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Dsw2026Ej15.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; init; }
        public string LicenseNumber { get; init; }
        [JsonInclude]
        public bool IsActive { get; private set; } = true;
        public Guid SpecialityId { get; init; } 
        public Speciality Speciality { get; init; }


        public Doctor() { }

        public Doctor(Guid id, string name, string licenseNumber, Speciality speciality) : base(id)
        {
            Name = name;
            LicenseNumber = licenseNumber;
            Speciality = speciality;
            IsActive = true;
        }
        public Doctor(string name, string licenseNumber, Speciality speciality, Guid? id = null) : base(id)
        {
            Name = name;
            LicenseNumber = licenseNumber;
            Speciality = speciality;
            IsActive = true;
        }

        public void Desactivate()
        {
            IsActive = false;
        }
    }
}