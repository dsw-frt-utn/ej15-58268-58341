using Dsw2026Ej15.Api.Exceptions;
using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Dsw2026Ej15.Api.Models.DoctorModel;

namespace Dsw2026Ej15.Controllers
{
    [ApiController]
    //[Route("[controller]")] //indica que esta clase es un controlador de API y que la ruta para acceder a sus métodos será "/Doctor" (el nombre del controlador sin la palabra "Controller")
    [Route("api/Doctors")] //indica que esta clase es un controlador de API y que la ruta para acceder a sus métodos será "/api/Doctors"
    public class DoctorController : ControllerBase
    {
        private readonly IPersistence _persistence;
        //private readonly ProductsManagmentService _service;

        public DoctorController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        [HttpPost] //indica que este método se ejecutará cuando se realice una solicitud HTTP POST a la ruta "/Doctor/CreateDoctor"
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorModel.Request request) //IActionResult es una interfaz que representa el resultado de una acción en un controlador, como devolver una vista, redirigir a otra acción, etc. //frombody es para decir recupera los datos del body
        {

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LicenseNumber))
            {
                throw new ValidationException("Nombre y matrícula son requeridos.");
            }

            var speciality = await _persistence.GetSpecialityById(request.SpecialityId);
            if (speciality == null)
            {
                throw new ValidationException("Especialidad no existe.");
            }

            await _persistence.InsertarDoctor(request.Name, request.LicenseNumber, speciality);
            return Created(string.Empty, "Doctor creado exitosamente."); //retorna un codigo 201

        }

        [HttpGet] //indica que este método se ejecutará cuando se realice una solicitud HTTP GET a la ruta "/Doctor/GetDoctor"
        public async Task<IActionResult> GetDoctorsActive() //IActionResult es una interfaz que representa el resultado de una acción en un controlador, como devolver una vista, redirigir a otra acción, etc. //fromquery es para decir recupera los datos de la query string
        {
            var doctors = await _persistence.GetDoctorsActive();

            var response = doctors?.Select(doctor => new DoctorModel.Response(
                doctor.Name,
                doctor.LicenseNumber,
                doctor.Speciality.Name))
                .ToList();

            //return Ok(doctors);
            return Ok(response);
        }

        [HttpGet("{id}")] //indica que este método se ejecutará cuando se realice una solicitud HTTP GET a la ruta "/Doctor/GetDoctor"
        public async Task<IActionResult> GetDoctorActiveById([FromRoute] Guid id) //IActionResult es una interfaz que representa el resultado de una acción en un controlador, como devolver una vista, redirigir a otra acción, etc. //fromquery es para decir recupera los datos de la query string
        {
            var doctor = await _persistence.GetDoctorActiveById(id);

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

        [HttpDelete("{id}")] //indica que este método se ejecutará cuando se realice una solicitud HTTP DELETE a la ruta "/Doctor/GetDoctor"
        public async Task<IActionResult> BajaLogicaDoctorById([FromRoute] Guid id) //IActionResult es una interfaz que representa el resultado de una acción en un controlador, como devolver una vista, redirigir a otra acción, etc. //fromquery es para decir recupera los datos de la query string
        {

            var doctor = await _persistence.GetDoctorActiveById(id);
            if (doctor == null)
            {
                throw new ValidationException("Doctor no encontrado.");
            }
            if (!doctor.IsActive)
            {
                throw new ValidationException("El Doctor no está activo.");
            }

            await _persistence.BajaLogicaDoctorById(id);

            return NoContent();

        }
    }
}
