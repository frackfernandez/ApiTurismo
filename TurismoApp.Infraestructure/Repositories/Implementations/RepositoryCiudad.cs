using Microsoft.EntityFrameworkCore;
using TurismoApp.Common;
using TurismoApp.Common.Dto;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Infraestructure.Repositories.Implementations
{
    public class RepositoryCiudad : IRepositoryCiudad
    {
        private readonly ApplicationDbContext context;

        public RepositoryCiudad(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<List<CiudadGet>> GetAllCiudad()
        {
            var list = await context.Ciudades.Where(x => x.Eliminado == false).Include(x => x.Departamento).Where(x => x.Departamento.Eliminado == false).ToListAsync();

            var listGet = new List<CiudadGet>();

            foreach (var ciudad in list)
            {
                listGet.Add(new CiudadGet
                {
                    Id = ciudad.Id,
                    Codigo = ciudad.Codigo,
                    Latitud = ciudad.Latitud,
                    Longitud = ciudad.Longitud,
                    DepartamentoId = ciudad.DepartamentoId,
                    DepartamentoDescripcion = ciudad.Departamento.Descripcion,
                    Descripcion = ciudad.Descripcion
                });
            }

            return listGet;
        }

        public async Task<CiudadGet> GetByIdCiudad(int id)
        {
            var entity = await context.Ciudades.Include(x => x.Departamento).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            var dto = new CiudadGet()
            {
                Id = entity.Id,
                Codigo = entity.Codigo,
                Latitud = entity.Latitud,
                Longitud = entity.Longitud,
                DepartamentoId = entity.DepartamentoId,
                DepartamentoDescripcion = entity.Departamento.Descripcion,
                Descripcion = entity.Descripcion
            };

            return dto;
        }

        public async Task<CiudadCreate> CreateCiudad(CiudadCreate ciudadCreate)
        {
            // Valida que no exista otro codigo igual
            var exists = await ExistsCodigoCiudad(ciudadCreate.Codigo);
            if (exists)
            {
                throw new CustomExceptions.DatoDuplicadoException("El codigo de ciudad ya existe");
            }

            // Valida que exista el codigo del Departamento
            var existsDep = await context.Departamentos.Where(x => x.Eliminado == false).AnyAsync(x => x.Id == ciudadCreate.DepartamentoId);
            if (!existsDep)
            {
                throw new CustomExceptions.DepartamentoNoEncontradoException("El codigo de departamento no existe");
            }

            var ciudad = new Ciudad()
            {
                Codigo = ciudadCreate.Codigo,
                Latitud = ciudadCreate.Latitud,
                Longitud = ciudadCreate.Longitud,
                DepartamentoId = ciudadCreate.DepartamentoId,
                Descripcion = ciudadCreate.Descripcion,
                Eliminado = false
            };

            context.Add(ciudad);
            await context.SaveChangesAsync();

            return ciudadCreate;
        }

        public async Task<CiudadGet> DeleteCiudad(int id)
        {
            var entity = await context.Ciudades.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }                        

            // bloquear si esta registrada en algun recorrido
            var existsRec = await context.Recorridos.Where(x => x.Eliminado == false).AnyAsync(r => r.CiudadOrigenId == id || r.CiudadDestinoId == id);
            if (existsRec)
            {
                throw new CustomExceptions.CiudadConRecorridosException("Esta ciudad tiene recorridos registrados");
            }

            // Borrado logico de la Ciudad
            entity.Eliminado = true;
            context.Update(entity);
            await context.SaveChangesAsync();

            return new CiudadGet() { Id = id };
        }
        
        public async Task<CiudadUpdate> UpdateCiudad(int id, CiudadUpdate ciudadUpdate)
        {
            var entity = await context.Ciudades.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            // Valida que exista el codigo del Departamento
            var existsDep = await context.Departamentos.Where(x => x.Eliminado == false).AnyAsync(x => x.Id == ciudadUpdate.DepartamentoId);
            if (!existsDep)
            {
                throw new CustomExceptions.DepartamentoNoEncontradoException("El codigo de departamento no existe");
            }

            entity.Latitud = ciudadUpdate.Latitud;
            entity.Longitud = ciudadUpdate.Longitud;
            entity.DepartamentoId = ciudadUpdate.DepartamentoId;
            entity.Descripcion = ciudadUpdate.Descripcion;

            context.Update(entity);
            await context.SaveChangesAsync();
            return new CiudadUpdate() { Descripcion = ciudadUpdate.Descripcion };
        }

        public async Task<bool> ExistsCodigoCiudad(string codigo)
        {
            var exists = await context.Ciudades.Where(x => x.Eliminado == false)
                .Include(x => x.Departamento).Where(x => x.Departamento.Eliminado == false)
                .AnyAsync(x => x.Codigo == codigo);

            return exists;
        }
    }
}
