using System.ComponentModel.DataAnnotations;

namespace TurismoApp.Infraestructure.Entities
{
    public class Ciudad
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int DepartamentoId { get; set; }
        public string Descripcion { get; set; }
        public bool Eliminado { get; set; }
                
        public Departamento Departamento { get; set; }
    }
}
