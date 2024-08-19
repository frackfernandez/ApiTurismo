using TurismoApp.Application.Interfaces;
using TurismoApp.Common.Dto;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Application.Implementations
{
    public class ApplicationCliente : IApplicationCliente
    {
        private readonly IRepositoryCliente repository;

        public ApplicationCliente(IRepositoryCliente repository)
        {
            this.repository = repository;
        }

        public async Task<List<ClienteGet>> GetAllCliente()
        {
            return await repository.GetAllCliente();
        }

        public async Task<ClienteGet> GetByIdCliente(int id)
        {
            return await repository.GetByIdCliente(id);
        }

        public async Task<ClienteCreate> CreateCliente(ClienteCreate clienteCreate)
        {
            return await repository.CreateCliente(clienteCreate);
        }

        public async Task<ClienteGet> DeleteCliente(int id)
        {
            return await repository.DeleteCliente(id);
        }

        public async Task<ClienteCreate> UpdateCliente(int id, ClienteCreate clienteCreate)
        {
            return await repository.UpdateCliente(id, clienteCreate);
        }

        public async Task<ClienteVerificacion> GetByIdClienteVerificacion(int id)
        {
            return await repository.GetByIdClienteVerificacion(id);
        }

        public async Task VerificarCliente(int id)
        {
            await repository.VerificarCliente(id);
        }
    }
}
