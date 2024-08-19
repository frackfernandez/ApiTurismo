using Microsoft.EntityFrameworkCore;
using TurismoApp.Common;
using TurismoApp.Common.Dto;
using TurismoApp.Infraestructure.Entities;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Infraestructure.Repositories.Implementations
{
    public class RepositoryCliente : IRepositoryCliente
    {
        private readonly ApplicationDbContext context;

        public RepositoryCliente(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<List<ClienteGet>> GetAllCliente()
        {
            var list = await context.Clientes.Where(x => x.Eliminado == false).ToListAsync();

            var listGet = new List<ClienteGet>();

            foreach (var cliente in list)
            {
                listGet.Add(new ClienteGet
                {
                    Id = cliente.Id,
                    Dni = cliente.Dni,
                    Nombres = cliente.Nombres,
                    Apellidos = cliente.Apellidos,
                    Correo = cliente.Correo
                });
            }

            return listGet;
        }

        public async Task<ClienteGet> GetByIdCliente(int id)
        {
            var entity = await context.Clientes.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            var dto = new ClienteGet()
            {
                Id = entity.Id,
                Dni = entity.Dni,
                Nombres = entity.Nombres,
                Apellidos = entity.Apellidos,
                Correo = entity.Correo
            };

            return dto;
        }

        public async Task<ClienteCreate> CreateCliente(ClienteCreate clienteCreate)
        {
            // validar dni repetido
            var existsdni = await context.Clientes.Where(x => x.Eliminado == false).AnyAsync(x => x.Dni == clienteCreate.Dni);
            if (existsdni)
            {
                throw new CustomExceptions.DatoDuplicadoException("DNI ya registrado");
            }

            // validar correo repetido
            var exists = await context.Clientes.Where(x => x.Eliminado == false).AnyAsync(x => x.Correo == clienteCreate.Correo);
            if (exists)
            {
                throw new CustomExceptions.DatoDuplicadoException("Correo ya registrado");
            }

            var cliente = new Cliente()
            {
                Dni = clienteCreate.Dni,
                Nombres = clienteCreate.Nombres,
                Apellidos = clienteCreate.Apellidos,
                Correo = clienteCreate.Correo
            };

            context.Add(cliente);
            await context.SaveChangesAsync();

            return clienteCreate;
        }

        public async Task<ClienteGet> DeleteCliente(int id)
        {
            var entity = await context.Clientes.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            // bloquear si esta registrada en algun recorrido
            var existsRec = await context.RecorridoClientes.AnyAsync(x => x.ClienteId == id);
            if (existsRec)
            {
                throw new CustomExceptions.ClienteConRecorridosException("Este cliente tiene recorridos registrados");
            }

            // Borrado logico del Cliente
            entity.Eliminado = true;
            context.Update(entity);
            await context.SaveChangesAsync();

            return new ClienteGet() { Id = id };
        }

        public async Task<ClienteCreate> UpdateCliente(int id, ClienteCreate clienteCreate)
        {
            var entity = await context.Clientes.FindAsync(id);

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            // si es diferente valida 

            if (clienteCreate.Dni != entity.Dni)
            {
                // validar dni repetido
                var existsdni = await context.Clientes.Where(x => x.Eliminado == false).AnyAsync(x => x.Dni == clienteCreate.Dni);
                if (existsdni)
                {
                    throw new CustomExceptions.DatoDuplicadoException("DNI ya registrado");
                }
            }

            if (clienteCreate.Correo != entity.Correo)
            {
                // validar correo repetido
                var exists = await context.Clientes.Where(x => x.Eliminado == false).AnyAsync(x => x.Correo == clienteCreate.Correo);
                if (exists)
                {
                    throw new CustomExceptions.DatoDuplicadoException("Correo ya registrado");
                }
            }                                  

            entity.Dni = clienteCreate.Dni;
            entity.Nombres = clienteCreate.Nombres;
            entity.Apellidos = clienteCreate.Apellidos;
            entity.Correo = clienteCreate.Correo;

            context.Update(entity);
            await context.SaveChangesAsync();
            return clienteCreate;
        }

        public async Task<ClienteVerificacion> GetByIdClienteVerificacion(int id)
        {
            var entity = await context.Clientes.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (entity == null)
            {
                return null;
            }
            if (entity.Eliminado == true)
            {
                return null;
            }

            var dto = new ClienteVerificacion()
            {
                Id = entity.Id,
                Dni = entity.Dni,
                Nombres = entity.Nombres,
                Apellidos = entity.Apellidos,
                Correo = entity.Correo,
                Verificado = entity.Verificado                
            };

            return dto;
        }

        public async Task VerificarCliente(int id)
        {
            var entity = await context.Clientes.FindAsync(id);
                        
            entity.Verificado = true;

            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
