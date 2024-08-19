using TurismoApp.Common.Dto;

namespace TurismoApp.Application.Interfaces
{
    public interface IApplicationCliente
    {
        public Task<List<ClienteGet>> GetAllCliente();
        public Task<ClienteGet> GetByIdCliente(int id);
        public Task<ClienteCreate> CreateCliente(ClienteCreate clienteCreate);
        public Task<ClienteGet> DeleteCliente(int id);
        public Task<ClienteCreate> UpdateCliente(int id, ClienteCreate clienteCreate);
        public Task<ClienteVerificacion> GetByIdClienteVerificacion(int id);
        public Task VerificarCliente(int id);
    }
}
