using Microsoft.AspNetCore.Mvc;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common;
using TurismoApp.Common.Dto;

namespace TurismoApp.API.Controllers
{
    [ApiController]
    [Route("api/ciudad")]
    public class CiudadController : ControllerBase
    {
        private readonly IApplicationCiudad appCiudad;

        public CiudadController(IApplicationCiudad appCiudad)
        {
            this.appCiudad = appCiudad;
        }

        [HttpGet]
        public async Task<ActionResult<List<CiudadGet>>> Get()
        {
            var list = await appCiudad.GetAllCiudad();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CiudadGet>> Get(int id)
        {
            var ciu = await appCiudad.GetByIdCiudad(id);
            if (ciu is null)
            {
                return NotFound(new { error = "No existe" });
            }
            return Ok(ciu);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CiudadCreate ciudadCreate)
        {
            try
            {
                if (!Helper.CodigoCiudadValido(ciudadCreate.Codigo))
                    return BadRequest(new { error = "El codigo debe seguir el siguiente formato 'LIM' " });

                if (!Helper.LatitudValida(ciudadCreate.Latitud))
                    return BadRequest(new { error = "El valor de latitud no es valido" });

                if (!Helper.LongitudValida(ciudadCreate.Longitud))
                    return BadRequest(new { error = "El valor de longitud no es valido" });

                await appCiudad.CreateCiudad(ciudadCreate);

                return Ok();
            }
            catch (CustomExceptions.DatoDuplicadoException ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }
            catch (CustomExceptions.DepartamentoNoEncontradoException ex)
            {
                return NotFound(new { error = $"{ex.Message}" });
            }            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var ciu = await appCiudad.DeleteCiudad(id);
                if (ciu is null)
                {
                    return NotFound(new { error = "No existe" });
                }
                return Ok();
            }
            catch (CustomExceptions.CiudadConRecorridosException ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CiudadUpdate ciudadUpdate)
        {
            try
            {
                if (!Helper.LatitudValida(ciudadUpdate.Latitud))
                    return BadRequest(new { error = "El valor de latitud no es valido" });

                if (!Helper.LongitudValida(ciudadUpdate.Longitud))
                    return BadRequest(new { error = "El valor de longitud no es valido" });

                var ciu = await appCiudad.UpdateCiudad(id, ciudadUpdate);
                if (ciu is null)
                {
                    return NotFound(new { error = "No existe" });
                }
                return Ok();
            }
            catch (CustomExceptions.DepartamentoNoEncontradoException ex)
            {
                return NotFound(new { error = $"{ex.Message}" });
            }            
        }
    }
}
