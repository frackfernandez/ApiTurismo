namespace TurismoApp.Common.Dto
{
    public class ClienteVerificacion
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public bool Verificado { get; set; }
    }
}
