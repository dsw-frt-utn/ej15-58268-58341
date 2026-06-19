using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Api.Exceptions;
using Dsw2026Ej15.Api.Models;


namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence) 
    { 
        _persistence = persistence;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] DoctorModel.Request request)
    {
        try 
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LicenseNumber)) 
            {
                throw new ValidationException("Nombre y Licencia requeridos");
            }

            var speciality = _persistence.GetSpecialityById(request.SpecialityId);
            if (speciality == null) 
            {
                throw new ValidationException("Esa especialidad no existe.");
            }

            _persistence.InsertarDoctor(request.Name, request.LicenseNumber, speciality);
            return Created(string.Empty, "Doctor ha sido creado exitosamente");
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }

    }


    [HttpGet] 
    public async Task<IActionResult> GetDoctorsActive() 
    {
        var doctors = _persistence.GetDoctorsActive();
        return Ok(doctors);
    }

    [HttpGet("{id}")] 
    public async Task<IActionResult> GetDoctorActiveById([FromRoute] Guid id) 
    {
        try
        {
            var doctor = _persistence.GetDoctorActiveById(id);

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
        catch (ValidationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> BajaLogicaDoctorById([FromRoute] Guid id)
    {
        try
        {
            var doctor = _persistence.GetDoctorActiveById(id);
            if (doctor == null)
            {
                throw new ValidationException("Doctor no encontrado.");
            }
            if (!doctor.IsActive)
            {
                throw new ValidationException("El Doctor no está activo.");
            }

            var doctor1 = _persistence.BajaLogicaDoctorById(id);

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return NotFound(ex.Message);
        }
    }
}