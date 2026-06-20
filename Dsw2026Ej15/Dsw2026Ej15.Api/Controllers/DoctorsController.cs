using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Api.Exceptions; // O donde tengas tu DoctorModel

namespace Dsw2026Ej15.Api.Controllers
{
    [ApiController]
    [Route("api/doctors")] // Recuerda la ruta en minúsculas
    public class DoctorsController : ControllerBase
    {
        private readonly IPersistence _persistence;

        // El constructor que recibe tu interfaz
        public DoctorsController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        // AQUÍ ES DONDE VA EL CÓDIGO QUE ME PREGUNTASTE:
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorModel.Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                throw new ValidationException("Nombre y Licencia requeridos");
            }

            var speciality = await _persistence.GetSpecialityByIdAsync(request.SpecialityId);

            if (speciality == null)
            {
                throw new ValidationException("Esa especialidad no existe.");
            }

            await _persistence.InsertarDoctorAsync(request.Name, request.LicenseNumber, speciality);

            return Created(string.Empty, "Doctor ha sido creado exitosamente");
        }

    }
}