using System.ComponentModel.DataAnnotations;

namespace TurismoApp.Infraestructure.Entities
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public bool Verificado { get; set; }
        public bool Eliminado { get; set; }

        public List<RecorridoCliente> RecorridoClientes { get; set; }
    }
}
