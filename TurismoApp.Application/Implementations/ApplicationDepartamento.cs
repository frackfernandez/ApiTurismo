using TurismoApp.Application.Interfaces;
using TurismoApp.Common.Dto;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Application.Implementations
{
    public class ApplicationDepartamento : IApplicationDepartamento
    {
        private readonly IRepositoryDepartamento repository;

        public ApplicationDepartamento(IRepositoryDepartamento repository)
        {
            this.repository = repository;
        }               

        public async Task<List<DepartamentoGet>> GetAllDepartamento()
        {
            return await repository.GetAllDepartamento();
        }

        public async Task<DepartamentoGet> GetByIdDepartamento(int id)
        {
            return await repository.GetByIdDepartamento(id);
        }

        public async Task<DepartamentoCreate> CreateDepartamento(DepartamentoCreate departamentoCreate)
        {
            return await repository.CreateDepartamento(departamentoCreate);
        }

        public async Task<DepartamentoGet> DeleteDepartamento(int id)
        {
            return await repository.DeleteDepartamento(id);
        }

        public async Task<DepartamentoCreate> UpdateDepartamento(int id, DepartamentoCreate departamentoCreate)
        {
            return await repository.UpdateDepartamento(id, departamentoCreate);
        }
    }
}
