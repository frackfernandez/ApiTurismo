using System.ComponentModel.DataAnnotations;

namespace TurismoApp.Infraestructure.Entities
{
    public class Recorrido
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaViaje { get; set; }
        public string EstadoViaje { get; set; }
        public int CiudadOrigenId { get; set; }
        public int CiudadDestinoId { get; set; }
        public double Distancia { get; set; }
        public double Precio { get; set; }
        public bool Eliminado { get; set; }
                
        public Ciudad CiudadOrigen { get; set; }
        public Ciudad CiudadDestino { get; set; }

        public List<RecorridoCliente> RecorridoClientes { get; set; }
    }
}
