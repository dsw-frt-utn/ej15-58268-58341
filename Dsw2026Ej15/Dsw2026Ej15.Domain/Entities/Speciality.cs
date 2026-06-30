using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Entities
{
    public class Speciality: BaseEntity
    {
        public Speciality() { }

        public string Name { get; set; }
        public string Description { get; set; }

        public Speciality(Guid id, string name, string description) : base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
