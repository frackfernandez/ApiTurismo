using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TurismoApp.Common
{
    public static class Helper
    {
        public static bool LatitudValida(double latitud)
        {
            return latitud >= -90 && latitud <= 90;
        }
        public static bool LongitudValida(double longitud)
        {
            return longitud >= -180 && longitud <= 180;
        }

        public static bool CodigoCiudadValido(string codigo)
        {
            if (codigo.Length != 3)
                return false;

            bool letras = Regex.IsMatch(codigo, @"^[A-Z]+$");
            if (!letras)
                return false;

            return true;
        }
        public static string GenerarCodigoRecorrido(string origen, string destino)
        {
            string fechaHoy = DateTime.Now.ToString("ddMMyyyy");

            return $"{origen}{destino}{fechaHoy}";
        }

        public static double ObtenerDistanciaKm(double latitudOrigen, double longitudOrigen, double latitudDestino, double longitudDestino)
        {
            const double radioTierraKm = 6371.0; // Radio de la Tierra en kilómetros

            // Convertir las coordenadas de grados a radianes
            double latitudOrigenRad = GradosARadianes(latitudOrigen);
            double longitudOrigenRad = GradosARadianes(longitudOrigen);
            double latitudDestinoRad = GradosARadianes(latitudDestino);
            double longitudDestinoRad = GradosARadianes(longitudDestino);

            // Diferencia entre las coordenadas
            double diferenciaLat = latitudDestinoRad - latitudOrigenRad;
            double diferenciaLong = longitudDestinoRad - longitudOrigenRad;

            // Fórmula del Haversine
            double a = Math.Sin(diferenciaLat / 2) * Math.Sin(diferenciaLat / 2) +
                       Math.Cos(latitudOrigenRad) * Math.Cos(latitudDestinoRad) *
                       Math.Sin(diferenciaLong / 2) * Math.Sin(diferenciaLong / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distanciaKm = radioTierraKm * c;

            return Math.Round(distanciaKm, 1);
        }
        public static double GradosARadianes(double grados)
        {
            return grados * (Math.PI / 180);
        }

        public static readonly List<DateTime> Feriados = new List<DateTime>
        {   
            new DateTime(2024, 1, 1), // Año Nuevo
            new DateTime(2024, 5, 1), // Día del Trabajador
            new DateTime(2024, 7, 28), // Día de Independencia            
            new DateTime(2024, 12, 25), // Navidad
        };
        public static double CalcularPrecio(double distancia, DateTime fechaViaje)
        {
            string dia = fechaViaje.DayOfWeek.ToString();

            if(Feriados.Contains(fechaViaje))
            {
                return distancia * 2.6;
            }
            if (dia == "Sunday")
            {
                return distancia * 2.6;
            }
            if (dia == "Tuesday" ||  dia == "Thursday")
            {
                return distancia * 1.3;
            }
            else
            {
                return distancia * 1.8;
            }

        }

        public static bool DateTimeValido(DateTime dt)
        {
            if (DateTime.Now.Date >= dt.Date)
                return false;

            return true;
        }


        // por ahora :C
        private const string Prefix = "092309fjh8fn894f209j309h290rh0";
        private const string Suffix = "gui3093jd3284jr9fh94hr89ur832hr923hj";

        public static string Encrypt(int id)
        {
            return Prefix + id.ToString() + Suffix;
        }

        public static int Decrypt(string clave)
        {
            string numberString = clave.Substring(Prefix.Length, clave.Length - Prefix.Length - Suffix.Length);
            int.TryParse(numberString, out int number);

            return number;
        }

    }
}
