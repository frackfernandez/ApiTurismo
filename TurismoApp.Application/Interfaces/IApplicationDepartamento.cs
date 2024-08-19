using TurismoApp.Common.Dto;

namespace TurismoApp.Application.Interfaces
{
    public interface IApplicationDepartamento
    {
        public Task<List<DepartamentoGet>> GetAllDepartamento();
        public Task<DepartamentoGet> GetByIdDepartamento(int id);
        public Task<DepartamentoCreate> CreateDepartamento(DepartamentoCreate departamentoCreate);
        public Task<DepartamentoGet> DeleteDepartamento(int id);
        public Task<DepartamentoCreate> UpdateDepartamento(int id, DepartamentoCreate departamentoCreate);
    }
}
