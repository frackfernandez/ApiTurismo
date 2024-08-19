using System.ComponentModel.DataAnnotations;

namespace TurismoApp.Infraestructure.Entities
{
    public class Departamento
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Eliminado { get; set; }
    }
}
