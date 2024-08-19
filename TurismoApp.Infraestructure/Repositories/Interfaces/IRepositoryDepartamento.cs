using TurismoApp.Common.Dto;

namespace TurismoApp.Infraestructure.Repositories.Interfaces
{
    public interface IRepositoryDepartamento
    {
        public Task<List<DepartamentoGet>> GetAllDepartamento();
        public Task<DepartamentoGet> GetByIdDepartamento(int id);
        public Task<DepartamentoCreate> CreateDepartamento(DepartamentoCreate departamentoCreate);
        public Task<DepartamentoGet> DeleteDepartamento(int id);
        public Task<DepartamentoCreate> UpdateDepartamento(int id, DepartamentoCreate departamentoCreate);
    }
}
