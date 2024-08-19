using TurismoApp.Application.Interfaces;
using TurismoApp.Infraestructure;
using TurismoApp.Services.Interfaces;

namespace TurismoApp.Application.Implementations
{
    public class ApplicationEmail : IApplicationEmail
    {
        private readonly IServiceEmail serviceEmail;

        public ApplicationEmail(IServiceEmail serviceEmail)
        {
            this.serviceEmail = serviceEmail;
        }
        public async Task SendEmailVerification(string email, string linkVerificacion)
        {
            await serviceEmail.SendEmailVerification(email, linkVerificacion);
        }
    }
}
