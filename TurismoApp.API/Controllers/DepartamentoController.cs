using Microsoft.AspNetCore.Mvc;
using TurismoApp.Application.Interfaces;
using TurismoApp.Common;
using TurismoApp.Common.Dto;

namespace TurismoApp.API.Controllers
{
    [ApiController]
    [Route("api/departamento")]
    public class DepartamentoController : ControllerBase
    {
        private readonly IApplicationDepartamento appDepartamento;

        public DepartamentoController(IApplicationDepartamento appDepartamento)
        {
            this.appDepartamento = appDepartamento;
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartamentoGet>>> Get()
        {            
            var list = await appDepartamento.GetAllDepartamento();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartamentoGet>> Get(int id)
        {
            var dep = await appDepartamento.GetByIdDepartamento(id);
            if (dep is null)
            {
                return NotFound(new { error = "No existe" });
            }
            return Ok(dep);
        }

        [HttpPost]
        public async Task<ActionResult> Post(DepartamentoCreate departamentoCreate)
        {
            if (string.IsNullOrEmpty(departamentoCreate.Descripcion))
                return BadRequest(new { error = "No puede estar en blanco" });

            await appDepartamento.CreateDepartamento(departamentoCreate);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var dep = await appDepartamento.DeleteDepartamento(id);
                if (dep is null)
                {
                    return NotFound(new { error = "No existe" });
                }
                return Ok();
            }
            catch (CustomExceptions.DepartamentoConCiudadesException ex)
            {
                return BadRequest(new { error = $"{ex.Message}" });
            }            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, DepartamentoCreate departamentoCreate)
        {
            var dep = await appDepartamento.UpdateDepartamento(id, departamentoCreate);
            if (dep is null)
            {
                return NotFound(new { error = "No existe" });
            }
            return Ok();
        }
    }
}
