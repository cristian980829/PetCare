using PetCare.Common.Models;

namespace PetCare.Api.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
