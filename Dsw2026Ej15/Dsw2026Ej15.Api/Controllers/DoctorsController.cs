using Dsw2026Ej15.Api.Exceptions;
using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Dsw2026Ej15.Api.Models.DoctorModel;

namespace Dsw2026Ej15.Controllers
{
    [ApiController]
   
    [Route("api/Doctors")] 
    public class DoctorController : ControllerBase
    {
        private readonly IPersistence _persistence;
        

        public DoctorController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        [HttpPost] 
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorModel.Request request) 
        {

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                throw new ValidationException("Nombre y matrícula son requeridos.");
            }

            var speciality = await _persistence.GetSpecialityByIdAsync(request.SpecialityId);

            if (speciality == null)
            {
                throw new ValidationException("Especialidad no existe.");
            }

            await _persistence.InsertarDoctorAsync(request.Name, request.LicenseNumber, speciality);
            return Created(string.Empty, "Doctor creado exitosamente."); 

        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorsActive() 
        {
            var doctors = await _persistence.GetDoctorsActiveAsync();

            var response = doctors?.Select(doctor => new DoctorModel.Response(
                doctor.Name,
                doctor.LicenseNumber,
                doctor.Speciality.Name))
                .ToList();

            
            return Ok(response);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetDoctorActiveById([FromRoute] Guid id)
        {
            var doctor = await _persistence.GetDoctorActiveByIdAsync(id);

            if (doctor == null)
            {
                throw new ValidationException("Doctor no encontrado.");
            }
            if (!doctor.IsActive)
            {
                throw new ValidationException("El Doctor no está activo.");
            }

            return Ok(new DoctorModel.Response(doctor.Name, doctor.LicenseNumber, doctor.Speciality.Name));

        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> BajaLogicaDoctorById([FromRoute] Guid id) { 

            var doctor = await _persistence.GetDoctorActiveByIdAsync(id);
            if (doctor == null)
            {
                throw new ValidationException("Doctor no encontrado.");
            }
            if (!doctor.IsActive)
            {
                throw new ValidationException("El Doctor no está activo.");
            }

            await _persistence.BajaLogicaDoctorByIdAsync(id);

            return NoContent();

        }
    }
}