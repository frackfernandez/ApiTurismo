using System.ComponentModel.DataAnnotations;

namespace TurismoApp.Infraestructure.Entities
{
    public class RecorridoCliente
    {
        public int RecorridoId { get; set; }
        public int ClienteId { get; set; }

        public Recorrido Recorrido { get; set; }
        public Cliente Cliente { get; set; }
    }
}
