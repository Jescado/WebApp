using AppCore;
using AppCore.Models;
using RestSharp;

namespace AppServiceOne
{
    public class ServiceOne : IServiceOne
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceHelper _serviceHelper;
        private static string? _serviceUrl;

        public ServiceOne(IConfiguration configuration, ServiceHelper serviceHelper)
        {
            _configuration = configuration;
            _serviceHelper = serviceHelper;
            _serviceUrl = _configuration.GetSection("BaseUrl").Value;
        }

        public async Task AddContact(ContactInfo contact)
        {
            try
            {
                var resource = "/AddContact";
                object obj = new { contact };
                await _serviceHelper.GetServiceResponse<HttpServiceResponseBase<string>>(_serviceUrl, resource, null, Method.POST, obj);
            }
            catch (Exception)
            {
            }
        }

        public async Task GetContact(int id)
        {
            try
            {
                var resource = "/GetContact";
                object obj = new { id };
                await _serviceHelper.GetServiceResponse<HttpServiceResponseBase<string>>(_serviceUrl, resource, null, Method.GET, obj);
            }
            catch (Exception)
            {
            }
        }

        public async Task RemoveContact(int id)
        {
            try
            {
                var resource = "/RemoveContact";
                object obj = new { id };
                await _serviceHelper.GetServiceResponse<HttpServiceResponseBase<string>>(_serviceUrl, resource, null, Method.DELETE, obj);
            }
            catch (Exception)
            {
            }
        }
    }
}
