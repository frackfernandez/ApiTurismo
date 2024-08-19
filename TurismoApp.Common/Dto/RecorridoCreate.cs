namespace TurismoApp.Common.Dto
{
    public class RecorridoCreate
    {
        public DateTime FechaViaje { get; set; }
        public int CiudadOrigenId { get; set; }
        public int CiudadDestinoId { get; set; }
        public List<int> Pasajeros { get; set; }
    }
}
