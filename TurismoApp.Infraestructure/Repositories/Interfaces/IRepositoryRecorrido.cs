using TurismoApp.Common.Dto;

namespace TurismoApp.Infraestructure.Repositories.Interfaces
{
    public interface IRepositoryRecorrido
    {
        public Task<List<RecorridoGet>> GetAllRecorrido();
        public Task<RecorridoWClientesGet> GetByIdRecorrido(int id);
        public Task<RecorridoCreate> CreateRecorrido(RecorridoCreate recorridoCreate);
        public Task<RecorridoGet> DeleteRecorrido(int id);
        public Task<RecorridoCreate> UpdateRecorrido(int id, RecorridoCreate recorridoCreate);

        public Task<List<RecorridoGet>> GetByStateRecorrido(string filtro);
        public Task<List<RecorridoGet>> GetByDateRecorrido(DateTime date1, DateTime date2);
        public Task UpdateRecorridoPendiente(int id);
        public Task UpdateRecorridoEnProgreso(int id);
    }
}
