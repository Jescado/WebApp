using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore
{
    public class HttpServiceResponseBase<TData>
    {
        [JsonProperty("data")]
        public TData Data { get; set; }

        [JsonProperty("errCode")]
        public string ErrorCode { get; set; }
    }
}
