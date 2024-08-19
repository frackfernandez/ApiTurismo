using Microsoft.AspNetCore.Mvc;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common;
using TurismoApp.Common.Dto;

namespace TurismoApp.API.Controllers
{
    [ApiController]
    [Route("api/verificacion")]
    public class VerificacionController : ControllerBase
    {
        private readonly IApplicationEmail appEmail;
        private readonly IApplicationCliente appCliente;

        public VerificacionController(IApplicationEmail appEmail, IApplicationCliente appCliente)
        {
            this.appEmail = appEmail;
            this.appCliente = appCliente;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> Verificar(int id)
        {
            var dto = await appCliente.GetByIdClienteVerificacion(id);
            if (dto is null)
            {
                return NotFound(new { error = "No existe" });
            }
            if (dto.Verificado == true)
            {
                return BadRequest(new { error = "Ya esta verificado" });
            }

            var key = Helper.Encrypt(id);

            try
            {
                await appEmail.SendEmailVerification(dto.Correo, $"https://localhost:7237/api/verificacion/confirmacion/{key}");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }

            return Ok("Revise su correo");
        }

        [HttpGet("confirmacion/{key}")]
        public async Task<ActionResult> Confirmacion(string key)
        {
            try
            {
                var id = Helper.Decrypt(key);

                var dto = await appCliente.GetByIdClienteVerificacion(id);
                if (dto is null)
                {
                    return NotFound(new { error = "No existe" });
                }
                if (dto.Verificado == true)
                {
                    return BadRequest(new { error = "Link invalido1" });
                }

                await appCliente.VerificarCliente(id);

                return Ok("Verificado");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Link invalido" });
            }
        }
    }
}
