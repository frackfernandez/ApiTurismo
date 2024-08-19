namespace TurismoApp.Common.Dto
{
    public class RecorridoGet
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaViaje { get; set; }
        public string EstadoViaje { get; set; }
        public int CiudadOrigenId { get; set; }
        public string CiudadOrigen { get; set; }
        public int CiudadDestinoId { get; set; }
        public string CiudadDestino { get; set; }
        public double Distancia { get; set; }
        public double Precio { get; set; }
        public int CantidadPasajeros { get; set; }
    }
}
