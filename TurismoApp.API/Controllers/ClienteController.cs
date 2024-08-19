using Microsoft.AspNetCore.Mvc;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common;
using TurismoApp.Common.Dto;

namespace TurismoApp.API.Controllers
{
    [ApiController]
    [Route("api/cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IApplicationCliente appCliente;

        public ClienteController(IApplicationCliente appCliente)
        {
            this.appCliente = appCliente;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteGet>>> Get()
        {
            var list = await appCliente.GetAllCliente();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteGet>> Get(int id)
        {
            var dto = await appCliente.GetByIdCliente(id);
            if (dto is null)
            {
                return NotFound(new { error = "No existe" });
            }
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Post(ClienteCreate clienteCreate)
        {
            try
            {
                // validacion dni y correo

                await appCliente.CreateCliente(clienteCreate);

                return Ok();
            }
            catch (CustomExceptions.DatoDuplicadoException ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var dto = await appCliente.DeleteCliente(id);
                if (dto is null)
                {
                    return NotFound(new { error = "No existe" });
                }
                return Ok();
            }
            catch (CustomExceptions.ClienteConRecorridosException ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ClienteCreate clienteCreate)
        {
            try
            {
                // validacion dni correo

                var ciu = await appCliente.UpdateCliente(id, clienteCreate);
                if (ciu is null)
                {
                    return NotFound(new { error = "No existe" });
                }
                return Ok();
            }
            catch (CustomExceptions.DatoDuplicadoException ex)
            {
                return NotFound(new { error = $"{ex.Message}" });
            }
        }
    }
}
