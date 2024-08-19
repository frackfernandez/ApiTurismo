using Microsoft.EntityFrameworkCore;
using TurismoApp.Common;
using TurismoApp.Common.Dto;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Infraestructure.Repositories.Implementations
{
    public class RepositoryRecorrido : IRepositoryRecorrido
    {
        private readonly ApplicationDbContext context;

        public RepositoryRecorrido(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public async Task<List<RecorridoGet>> GetAllRecorrido()
        {
            var list = await context.Recorridos
                .Where(x => x.Eliminado == false)
                .Include(x => x.CiudadOrigen)
                .Include(x => x.CiudadDestino)
                .Include(x => x.RecorridoClientes)
                .ToListAsync();

            var listDto = new List<RecorridoGet>();

            foreach (var recorrido in list)
            {
                listDto.Add(new RecorridoGet
                {
                    Id = recorrido.Id,
                    Codigo = recorrido.Codigo,
                    FechaViaje = recorrido.FechaViaje,
                    EstadoViaje = recorrido.EstadoViaje,
                    CiudadOrigenId = recorrido.CiudadOrigenId,
                    CiudadOrigen = recorrido.CiudadOrigen.Descripcion,
                    CiudadDestinoId = recorrido.CiudadDestinoId,
                    CiudadDestino = recorrido.CiudadDestino.Descripcion,
                    Distancia = recorrido.Distancia,
                    Precio = recorrido.Precio,
                    CantidadPasajeros = recorrido.RecorridoClientes.Count()
                });
            }

            return listDto;
        }

        public async Task<RecorridoWClientesGet> GetByIdRecorrido(int id)
        {
            var recorrido = await context.Recorridos
                .Where(x => x.Eliminado == false)
                .Include(x => x.CiudadOrigen)
                .Include(x => x.CiudadDestino)
                .Include(x => x.RecorridoClientes)
                .ThenInclude(rc => rc.Cliente)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (recorrido == null)
                return null;

            var pasajeros = recorrido.RecorridoClientes
                .Select(rc => new ClienteGet
                    {
                        Id = rc.Cliente.Id,
                        Dni = rc.Cliente.Dni,
                        Nombres = rc.Cliente.Nombres,
                        Apellidos = rc.Cliente.Apellidos,
                        Correo = rc.Cliente.Correo
                    }).ToList();

            var dto = new RecorridoWClientesGet
            {
                Id = recorrido.Id,
                Codigo = recorrido.Codigo,
                FechaViaje = recorrido.FechaViaje,
                EstadoViaje = recorrido.EstadoViaje,
                CiudadOrigenId = recorrido.CiudadOrigenId,
                CiudadOrigen = recorrido.CiudadOrigen.Descripcion,
                CiudadDestinoId = recorrido.CiudadDestinoId,
                CiudadDestino = recorrido.CiudadDestino.Descripcion,
                Distancia = recorrido.Distancia,
                Precio = recorrido.Precio,
                CantidadPasajeros = recorrido.RecorridoClientes.Count(),
                Pasajeros = pasajeros 
            };

            return dto;
        }

        public async Task<RecorridoCreate> CreateRecorrido(RecorridoCreate recorridoCreate)
        {       
            // Validaciones id ciudades validas y pasajeros validos
            var exists = await context.Ciudades.Where(x => x.Eliminado == false).AnyAsync(x => x.Id == recorridoCreate.CiudadOrigenId);
            if (!exists)
                throw new CustomExceptions.CiudadNoEncontradaException("La ciudad de origen no existe");

            var existstwo = await context.Ciudades.Where(x => x.Eliminado == false).AnyAsync(x => x.Id == recorridoCreate.CiudadDestinoId);
            if (!existstwo)
                throw new CustomExceptions.CiudadNoEncontradaException("La ciudad de destino no existe");

            if (recorridoCreate.Pasajeros.Count() != 0)
            {
                var existsClientes = await context.Clientes.Where(x => x.Eliminado == false)
                .CountAsync(c => recorridoCreate.Pasajeros.Contains(c.Id)) == recorridoCreate.Pasajeros.Count();
                if (!existsClientes)
                    throw new CustomExceptions.ClienteNoEncontradoException("Uno o más clientes no existen");
            }

            // 
            var ciudadOri = await context.Ciudades.FindAsync(recorridoCreate.CiudadOrigenId);
            var ciudadDes = await context.Ciudades.FindAsync(recorridoCreate.CiudadDestinoId);

            var codigoH = Helper.GenerarCodigoRecorrido(ciudadOri.Codigo, ciudadDes.Codigo);
            var distanciaH = Helper.ObtenerDistanciaKm(ciudadOri.Latitud, ciudadOri.Longitud, ciudadDes.Latitud, ciudadDes.Longitud);

            // Validacion Codigo de Recorrido
            var existsCodigo = await context.Recorridos.Where(x => x.Eliminado == false).AnyAsync(x => x.Codigo == codigoH);
            if (existsCodigo)
                throw new CustomExceptions.DatoDuplicadoException("Ya existe un recorrido igual generado hoy");

            var recorrido = new Recorrido()
            {
                Codigo = codigoH,
                FechaViaje = recorridoCreate.FechaViaje,
                EstadoViaje = "Pendiente",
                CiudadOrigenId = recorridoCreate.CiudadOrigenId,
                CiudadDestinoId = recorridoCreate.CiudadDestinoId,
                Distancia = distanciaH,
                Precio = Helper.CalcularPrecio(distanciaH, recorridoCreate.FechaViaje),
                Eliminado = false
            };

            context.Add(recorrido);
            await context.SaveChangesAsync();
            
            var idRecorrido = await context.Recorridos.Where(x => x.Codigo == codigoH).FirstOrDefaultAsync();

            if (recorridoCreate.Pasajeros.Count() != 0)
            {
                foreach (var cliente in recorridoCreate.Pasajeros)
                {
                    context.Add(new RecorridoCliente()
                    {
                        RecorridoId = idRecorrido.Id,
                        ClienteId = cliente
                    });
                    await context.SaveChangesAsync();
                }
            }
            
            return recorridoCreate;
        }

        public async Task<RecorridoGet> DeleteRecorrido(int id)
        {
            var entity = await context.Recorridos.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            // Borrado logico del recorrido
            entity.Eliminado = true;
            context.Update(entity);
            await context.SaveChangesAsync();

            return new RecorridoGet() { Id = id };
        }

        public async Task<RecorridoCreate> UpdateRecorrido(int id, RecorridoCreate recorridoCreate)
        {
            var entity = await context.Recorridos.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            // Validaciones id ciudades validas y pasajeros validos
            var exists = await context.Ciudades.Where(x => x.Eliminado == false).AnyAsync(x => x.Id == recorridoCreate.CiudadOrigenId);
            if (!exists)
                throw new CustomExceptions.CiudadNoEncontradaException("La ciudad de origen no existe");

            var existstwo = await context.Ciudades.Where(x => x.Eliminado == false).AnyAsync(x => x.Id == recorridoCreate.CiudadDestinoId);
            if (!existstwo)
                throw new CustomExceptions.CiudadNoEncontradaException("La ciudad de destino no existe");
                        
            if (recorridoCreate.Pasajeros.Count() != 0)
            {
                var existsClientes = await context.Clientes.Where(x => x.Eliminado == false)
                .CountAsync(c => recorridoCreate.Pasajeros.Contains(c.Id)) == recorridoCreate.Pasajeros.Count();
                if (!existsClientes)
                    throw new CustomExceptions.ClienteNoEncontradoException("Uno o más clientes no existen");
            }

            // 
            var ciudadOri = await context.Ciudades.FindAsync(recorridoCreate.CiudadOrigenId);
            var ciudadDes = await context.Ciudades.FindAsync(recorridoCreate.CiudadDestinoId);

            var codigoH = Helper.GenerarCodigoRecorrido(ciudadOri.Codigo, ciudadDes.Codigo);
            var distanciaH = Helper.ObtenerDistanciaKm(ciudadOri.Latitud, ciudadOri.Longitud, ciudadDes.Latitud, ciudadDes.Longitud);

            // Validacion Codigo de Recorrido
            var existsCodigo = await context.Recorridos.Where(x => x.Eliminado == false).AnyAsync(x => x.Codigo == codigoH);
            if (existsCodigo)
                throw new CustomExceptions.DatoDuplicadoException("Ya existe un recorrido igual generado hoy");

            entity.Codigo = codigoH;
            entity.FechaViaje = recorridoCreate.FechaViaje;
            entity.EstadoViaje = "Pendiente";
            entity.CiudadOrigenId = recorridoCreate.CiudadOrigenId;
            entity.CiudadDestinoId = recorridoCreate.CiudadDestinoId;
            entity.Distancia = distanciaH;
            entity.Precio = Helper.CalcularPrecio(distanciaH, recorridoCreate.FechaViaje);
            entity.Eliminado = false;

            context.Update(entity);
            await context.SaveChangesAsync();

            var idRecorrido = await context.Recorridos.Where(x => x.Codigo == codigoH).FirstOrDefaultAsync();

            if (recorridoCreate.Pasajeros.Count() != 0)
            {
                foreach (var cliente in recorridoCreate.Pasajeros)
                {
                    context.Add(new RecorridoCliente()
                    {
                        RecorridoId = idRecorrido.Id,
                        ClienteId = cliente
                    });
                    await context.SaveChangesAsync();
                }
            }

            return recorridoCreate;
        }

        public async Task<List<RecorridoGet>> GetByStateRecorrido(string filtro)
        {
            var list = await context.Recorridos
                .Where(x => x.Eliminado == false)
                .Where(x => x.EstadoViaje == filtro)
                .Include(x => x.CiudadOrigen)
                .Include(x => x.CiudadDestino)
                .Include(x => x.RecorridoClientes)
                .ToListAsync();

            var listDto = new List<RecorridoGet>();

            foreach (var recorrido in list)
            {
                listDto.Add(new RecorridoGet
                {
                    Id = recorrido.Id,
                    Codigo = recorrido.Codigo,
                    FechaViaje = recorrido.FechaViaje,
                    EstadoViaje = recorrido.EstadoViaje,
                    CiudadOrigenId = recorrido.CiudadOrigenId,
                    CiudadOrigen = recorrido.CiudadOrigen.Descripcion,
                    CiudadDestinoId = recorrido.CiudadDestinoId,
                    CiudadDestino = recorrido.CiudadDestino.Descripcion,
                    Distancia = recorrido.Distancia,
                    Precio = recorrido.Precio,
                    CantidadPasajeros = recorrido.RecorridoClientes.Count()
                });
            }

            return listDto;
        }

        public async Task<List<RecorridoGet>> GetByDateRecorrido(DateTime date1, DateTime date2)
        {
            var list = await context.Recorridos
                .Where(x => x.Eliminado == false)
                .Where(x => x.FechaViaje.Date >= date1.Date && x.FechaViaje.Date <= date2.Date)
                .Include(x => x.CiudadOrigen)
                .Include(x => x.CiudadDestino)
                .Include(x => x.RecorridoClientes)
                .ToListAsync();

            var listDto = new List<RecorridoGet>();

            foreach (var recorrido in list)
            {
                listDto.Add(new RecorridoGet
                {
                    Id = recorrido.Id,
                    Codigo = recorrido.Codigo,
                    FechaViaje = recorrido.FechaViaje,
                    EstadoViaje = recorrido.EstadoViaje,
                    CiudadOrigenId = recorrido.CiudadOrigenId,
                    CiudadOrigen = recorrido.CiudadOrigen.Descripcion,
                    CiudadDestinoId = recorrido.CiudadDestinoId,
                    CiudadDestino = recorrido.CiudadDestino.Descripcion,
                    Distancia = recorrido.Distancia,
                    Precio = recorrido.Precio,
                    CantidadPasajeros = recorrido.RecorridoClientes.Count()
                });
            }

            return listDto;
        }

        public async Task UpdateRecorridoPendiente(int id)
        {
            var entity = await context.Recorridos.FindAsync(id);

            if (entity == null)
            {
                throw new CustomExceptions.RecorridoNoEncontradoException("Recorrido no encontrado");
            }
            if (entity.Eliminado == true)
            {
                throw new CustomExceptions.RecorridoNoEncontradoException("Recorrido no encontrado");
            }
            if (entity.EstadoViaje != "Pendiente")
            {
                throw new CustomExceptions.RecorridoNoEncontradoException("El recorrido no tiene el estado requerido");
            }

            entity.EstadoViaje = "En Progreso";

            context.Update(entity);
            await context.SaveChangesAsync();   
        }

        public async Task UpdateRecorridoEnProgreso(int id)
        {
            var entity = await context.Recorridos.FindAsync(id);

            if (entity == null)
            {
                throw new CustomExceptions.RecorridoNoEncontradoException("Recorrido no encontrado");
            }
            if (entity.Eliminado == true)
            {
                throw new CustomExceptions.RecorridoNoEncontradoException("Recorrido no encontrado");
            }
            if (entity.EstadoViaje != "En Progreso")
            {
                throw new CustomExceptions.RecorridoNoEncontradoException("El recorrido no tiene el estado requerido");
            }

            entity.EstadoViaje = "Finalizado";

            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
