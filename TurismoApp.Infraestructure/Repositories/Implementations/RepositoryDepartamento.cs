using Microsoft.EntityFrameworkCore;
using TurismoApp.Common;
using TurismoApp.Common.Dto;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Infraestructure.Repositories.Implementations
{
    public class RepositoryDepartamento : IRepositoryDepartamento
    {
        private readonly ApplicationDbContext context;

        public RepositoryDepartamento(ApplicationDbContext context)
        {
            this.context = context;
        }        

        public async Task<List<DepartamentoGet>> GetAllDepartamento()
        {
            var list = await context.Departamentos.Where(x => x.Eliminado == false).ToListAsync();

            var listGet = new List<DepartamentoGet>();

            foreach (var departamento in list)
            {
                listGet.Add(new DepartamentoGet
                {
                    Id = departamento.Id,
                    Descripcion = departamento.Descripcion
                });
            }

            return listGet;
        }

        public async Task<DepartamentoGet> GetByIdDepartamento(int id)
        {
            var entity = await context.Departamentos.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            var dto = new DepartamentoGet()
            {
                Id = entity.Id,
                Descripcion = entity.Descripcion
            };

            return dto;
        }

        public async Task<DepartamentoCreate> CreateDepartamento(DepartamentoCreate departamentoCreate)
        {
            var departamento = new Departamento
            {
                Descripcion = departamentoCreate.Descripcion,
                Eliminado = false
            };

            context.Add(departamento);
            await context.SaveChangesAsync();

            return departamentoCreate;
        }

        public async Task<DepartamentoGet> DeleteDepartamento(int id)
        {
            var entity = await context.Departamentos.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            // Borrado logico Ciudades del Departamento
            //var listCiudades = await context.Ciudades.Where(c => c.DepartamentoId == id).ToListAsync();

            //foreach (var ciudad in listCiudades)
            //{
            //    ciudad.Eliminado = true;
            //    context.Update(ciudad);
            //    await context.SaveChangesAsync();
            //}

            // o Restricción
            var ciudadesDep = await context.Ciudades.AnyAsync(x => x.DepartamentoId == id);
            if (ciudadesDep)
            {
                throw new CustomExceptions.DepartamentoConCiudadesException("El departamento tiene ciudades registradas");
            }

            
            // Borrado logico del Departamento
            entity.Eliminado = true;
            context.Update(entity);
            await context.SaveChangesAsync();

            return new DepartamentoGet() { Id = id };
        }

        public async Task<DepartamentoCreate> UpdateDepartamento(int id, DepartamentoCreate departamentoCreate)
        {
            var entity = await context.Departamentos.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            entity.Descripcion = departamentoCreate.Descripcion;

            context.Update(entity);
            await context.SaveChangesAsync();
            return new DepartamentoCreate() { Descripcion = departamentoCreate.Descripcion };
        }
    }
}
