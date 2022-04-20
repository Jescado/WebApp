using AppCore.Models;

namespace AppServiceOne
{
    public interface IServiceOne
    {
        Task AddContact(ContactInfo contact);
        Task GetContact(int id);
        Task RemoveContact(int id);
    }
}
