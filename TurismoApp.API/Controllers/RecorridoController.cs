using Microsoft.AspNetCore.Mvc;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common.Dto;
using TurismoApp.Common;

namespace TurismoApp.API.Controllers
{
    [ApiController]
    [Route("api/recorrido")]
    public class RecorridoController : ControllerBase
    {
        private readonly IApplicationRecorrido appRecorrido;

        public RecorridoController(IApplicationRecorrido appRecorrido)
        {
            this.appRecorrido = appRecorrido;
        }

        [HttpGet]
        public async Task<ActionResult<List<RecorridoGet>>> Get()
        {
            var list = await appRecorrido.GetAllRecorrido();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecorridoWClientesGet>> Get(int id)
        {
            var ciu = await appRecorrido.GetByIdRecorrido(id);
            if (ciu is null)
            {
                return NotFound(new { error = "No existe" });
            }
            return Ok(ciu);
        }

        [HttpPost]
        public async Task<ActionResult> Post(RecorridoCreate recorridoCreate)
        {
            try
            {
                if (!Helper.DateTimeValido(recorridoCreate.FechaViaje))
                    return BadRequest(new { error = "El fecha no es valida" });

                await appRecorrido.CreateRecorrido(recorridoCreate);

                return Ok();
            }
            catch (CustomExceptions.DatoDuplicadoException ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }
            catch (CustomExceptions.CiudadNoEncontradaException ex)
            {
                return NotFound(new { error = $"{ex.Message}" });
            }
            catch (CustomExceptions.ClienteNoEncontradoException ex)
            {
                return NotFound(new { error = $"{ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var ciu = await appRecorrido.DeleteRecorrido(id);
            if (ciu is null)
            {
                return NotFound(new { error = "No existe" });
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, RecorridoCreate recorridoCreate)
        {
            try
            {
                if (!Helper.DateTimeValido(recorridoCreate.FechaViaje))
                    return BadRequest(new { error = "El fecha no es valida" });

                var ciu = await appRecorrido.UpdateRecorrido(id, recorridoCreate);
                if (ciu is null)
                {
                    return NotFound(new { error = "No existe" });
                }

                return Ok();
            }
            catch (CustomExceptions.DatoDuplicadoException ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }
            catch (CustomExceptions.CiudadNoEncontradaException ex)
            {
                return NotFound(new { error = $"{ex.Message}" });
            }
            catch (CustomExceptions.ClienteNoEncontradoException ex)
            {
                return NotFound(new { error = $"{ex.Message}" });
            }
        }

        [HttpGet("estado")]
        public async Task<ActionResult<List<RecorridoGet>>> Get([FromQuery]string estado)
        {
            estado = estado.ToUpper();

            if (estado != "PENDIENTE" && estado != "ENPROGRESO" && estado != "FINALIZADO")
                return BadRequest(new { error = "Se necesita un estado valido" });

            var list = new List<RecorridoGet>();

            if (estado == "PENDIENTE")
                list = await appRecorrido.GetByStateRecorrido("Pendiente");

            if (estado == "ENPROGRESO")
                list = await appRecorrido.GetByStateRecorrido("En Progreso");

            if (estado == "FINALIZADO")
                list = await appRecorrido.GetByStateRecorrido("Finalizado");

            return Ok(list);
        }

        [HttpGet("porfechas")]
        public async Task<ActionResult<List<RecorridoGet>>> Get([FromQuery] DateTime fecha1, DateTime fecha2)
        {   
            var list = await appRecorrido.GetByDateRecorrido(fecha1, fecha2);

            return Ok(list);
        }

        [HttpPut("update-pendiente/{id}")]
        public async Task<ActionResult> UpdatePendiente(int id)
        {
            try
            {
                await appRecorrido.UpdateRecorridoPendiente(id);

                return Ok();
            }
            catch (CustomExceptions.RecorridoNoEncontradoException ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }
        }

        [HttpPut("update-enprogreso/{id}")]
        public async Task<ActionResult> UpdateEnProgreso(int id)
        {
            try
            {
                await appRecorrido.UpdateRecorridoEnProgreso(id);

                return Ok();
            }
            catch (CustomExceptions.RecorridoNoEncontradoException ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }
        }
    }
}
