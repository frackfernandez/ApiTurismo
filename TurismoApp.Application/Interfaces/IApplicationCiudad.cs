using TurismoApp.Common.Dto;

namespace TurismoApp.Application.Interfaces
{
    public interface IApplicationCiudad
    {
        public Task<List<CiudadGet>> GetAllCiudad();
        public Task<CiudadGet> GetByIdCiudad(int id);
        public Task<CiudadCreate> CreateCiudad(CiudadCreate ciudadCreate);
        public Task<CiudadGet> DeleteCiudad(int id);
        public Task<CiudadUpdate> UpdateCiudad(int id, CiudadUpdate ciudadUpdate);
    }
}
