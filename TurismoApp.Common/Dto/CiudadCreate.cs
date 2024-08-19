namespace TurismoApp.Common.Dto
{
    public class CiudadCreate
    {
        public string Codigo { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public int DepartamentoId { get; set; }
        public string Descripcion { get; set; }
    }
}
