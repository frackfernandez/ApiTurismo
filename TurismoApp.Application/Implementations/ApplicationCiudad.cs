using TurismoApp.Application.Interfaces;
using TurismoApp.Common.Dto;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Application.Implementations
{
    public class ApplicationCiudad : IApplicationCiudad
    {
        private readonly IRepositoryCiudad repository;

        public ApplicationCiudad(IRepositoryCiudad repository)
        {
            this.repository = repository;
        }
        public async Task<List<CiudadGet>> GetAllCiudad()
        {
            return await repository.GetAllCiudad();
        }

        public async Task<CiudadGet> GetByIdCiudad(int id)
        {
            return await repository.GetByIdCiudad(id);
        }

        public async Task<CiudadCreate> CreateCiudad(CiudadCreate ciudadCreate)
        {
            return await repository.CreateCiudad(ciudadCreate);
        }

        public async Task<CiudadGet> DeleteCiudad(int id)
        {
            return await repository.DeleteCiudad(id);
        }

        public async Task<CiudadUpdate> UpdateCiudad(int id, CiudadUpdate ciudadUpdate)
        {
            return await repository.UpdateCiudad(id, ciudadUpdate);
        }
    }
}
