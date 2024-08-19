namespace TurismoApp.Services.Interfaces
{
    public interface IServiceEmail
    {
        public Task SendEmailVerification(string email, string linkVerificacion);
    }
}
