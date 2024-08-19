using TurismoApp.Application.Interfaces;
using TurismoApp.Common.Dto;
using TurismoApp.Infraestructure.Repositories.Interfaces;

namespace TurismoApp.Application.Implementations
{
    public class ApplicationRecorrido : IApplicationRecorrido
    {
        private readonly IRepositoryRecorrido repository;

        public ApplicationRecorrido(IRepositoryRecorrido repository)
        {
            this.repository = repository;
        }

        public async Task<List<RecorridoGet>> GetAllRecorrido()
        {
            return await repository.GetAllRecorrido();
        }

        public async Task<RecorridoWClientesGet> GetByIdRecorrido(int id)
        {
            return await repository.GetByIdRecorrido(id);
        }

        public async Task<RecorridoCreate> CreateRecorrido(RecorridoCreate recorridoCreate)
        {
            return await repository.CreateRecorrido(recorridoCreate);
        }

        public async Task<RecorridoGet> DeleteRecorrido(int id)
        {
            return await repository.DeleteRecorrido(id);
        }              

        public async Task<RecorridoCreate> UpdateRecorrido(int id, RecorridoCreate recorridoCreate)
        {
            return await repository.UpdateRecorrido(id, recorridoCreate);
        }

        public async Task<List<RecorridoGet>> GetByStateRecorrido(string filtro)
        {
            return await repository.GetByStateRecorrido(filtro);
        }

        public async Task<List<RecorridoGet>> GetByDateRecorrido(DateTime date1, DateTime date2)
        {
            return await repository.GetByDateRecorrido(date1, date2);
        }

        public async Task UpdateRecorridoPendiente(int id)
        {
            await repository.UpdateRecorridoPendiente(id);
        }

        public async Task UpdateRecorridoEnProgreso(int id)
        {
            await repository.UpdateRecorridoEnProgreso(id);
        }
    }
}
