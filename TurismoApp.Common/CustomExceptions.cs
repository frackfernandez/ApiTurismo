namespace TurismoApp.Common
{
    public static class CustomExceptions
    {
        
        public class DatoDuplicadoException : Exception
        {
            public DatoDuplicadoException(string message) : base(message)
            {
            }
        }

        public class DepartamentoNoEncontradoException : Exception
        {
            public DepartamentoNoEncontradoException(string message) : base(message)
            {
            }
        }

        public class CiudadNoEncontradaException : Exception
        {
            public CiudadNoEncontradaException(string message) : base(message)
            {
            }
        }
        public class ClienteNoEncontradoException : Exception
        { 
            public ClienteNoEncontradoException(string message) : base(message)
            {
            }
        }

        public class RecorridoNoEncontradoException : Exception
        {
            public RecorridoNoEncontradoException(string message) : base(message)
            {
            }
        }

        public class DepartamentoConCiudadesException : Exception
        {
            public DepartamentoConCiudadesException(string message) : base(message) 
            {
                
            }
        }

        public class CiudadConRecorridosException : Exception
        {
            public CiudadConRecorridosException(string message) : base(message)
            {

            }
        }

        public class ClienteConRecorridosException : Exception
        {
            public ClienteConRecorridosException(string message) : base(message)
            {

            }
        }
    }
}
