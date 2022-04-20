using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace AppCore
{
    public class ServiceHelper
    {
        public async Task<T> GetServiceResponse<T>(string serviceUrl, 
                                                   string resource, 
                                                   List<Parameter> parameters, 
                                                   Method method = Method.GET, 
                                                   object postObject = null, 
                                                   int? timeout = null) where T : new()
        {
            var client = new RestClient(serviceUrl);
            if (timeout != null)
                client.Timeout = (int)timeout;

            var request = new RestRequest(resource, method);
            if (method == Method.POST || method == Method.PUT)
            {
                request.AddJsonBody(postObject);
                //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            }
            else
            {
                foreach (var parameter in parameters)
                {
                    if (parameter.Value != null && parameter.Value.ToString().Contains("?"))
                    {
                        var paramVal = parameter.Value.ToString();
                        parameter.Value = paramVal.Substring(0, paramVal.IndexOf('?'));
                    }
                    request.AddParameter(parameter.Name, parameter.Value ?? string.Empty, parameter.Type);
                }
            }

            request.RequestFormat = DataFormat.Json;
            IRestResponse response;

            response = await client.ExecuteAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            else
            {
                throw new Exception("Service Call Error: Message" + response.Content);
            }
        }
    }
}