namespace TurismoApp.Application.Interfaces
{
    public interface IApplicationEmail
    {
        public Task SendEmailVerification(string email, string linkVerificacion);
    }
}
