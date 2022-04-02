using Introduccion.NET.Ejercicio.Common.Contracts.Middle;
using Introduccion.NET.Ejercicio.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    public class SenderManager : ISenderManager
    {

        private const string Mediatypejson = "application/json";
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpFactory;
        private readonly IHttpContextAccessor _http;

        public SenderManager(IConfiguration config, IHttpClientFactory httpFactory, IHttpContextAccessor http)
        {
            _config = config;
            _httpFactory = httpFactory;
            _http = http;
        }

        public async Task<ResponseBase<T>> SendRequest<T>(HttpRequestMessage requestMessage, string clientBase)
        {
            ResponseBase<T> responseBase = new ResponseBase<T>();
            try
            {
                Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                using (HttpClient client = _httpFactory.CreateClient(clientBase))
                {
                    client.Timeout = TimeSpan.FromSeconds(3000);
                    HttpResponseMessage responseMessage;
                    using (responseMessage = await client.SendAsync(requestMessage).ConfigureAwait(true))
                    {
                        responseBase.Code = (int)responseMessage.StatusCode;
                        responseBase.Message = responseMessage.StatusCode.ToString();

                        var contentResponse = await responseMessage.Content.ReadAsStringAsync();
                        ValidateResponse(responseBase, responseMessage, type, contentResponse);
                        responseMessage.Dispose();

                        return responseBase;
                    }
                }
            }
            catch (Exception ex)
            {
                return GenericUtility.ResponseBaseCatch<T>(true, ex, HttpStatusCode.InternalServerError, null);
            }
        }

        public HttpRequestMessage BuildMessageByConfig<T>(HttpMethod method, T content, RequestConfig configParams, bool isStandar = true, bool includeJsonIgnore = false)
        {
            var paramsUrl = configParams.QueryParams.Count > 0 ? $"{configParams.URL}?{configParams.QueryParams.ConvertToQueryString()}" : configParams.URL;
            var url = isStandar ? configParams.QueryParams.ConvertToQueryString(configParams.URL) : paramsUrl;

            var requestMessage = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(string.Concat(_config["BaseAddress:" + configParams.BaseAddress], url)),
            };

            string autToken = _http.GetToken();
            if (!autToken.IsEmpty())
                configParams.Headers.Add("Authorization", autToken);

            configParams.Headers.ConvertToHttpHeaders(requestMessage);

            var serializerSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            if (includeJsonIgnore)
                serializerSetting.ContractResolver = new IgnoreJsonAttributesResolver();
            if (content != null)
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(content, Formatting.None, serializerSetting), System.Text.Encoding.UTF8, Mediatypejson);

            return requestMessage;
        }

        public RequestConfig GetConfiguration(string configName)
        {
            return JsonConvert.DeserializeObject<RequestConfig>(_config[configName].Replace("{{", "{").Replace("}}", "}"));
        }

        public void ValidateResponse<T>(ResponseBase<T> responseBase, HttpResponseMessage responseMessage, Type type, string contentResponse)
        {
            string contentResponseCopy = contentResponse;
            if (string.IsNullOrEmpty(contentResponse))
            {
                responseBase.Code = responseBase.Code == (int)HttpStatusCode.OK ? (int)HttpStatusCode.NoContent : (int)responseMessage.StatusCode;
                responseBase.Data = default(T);
                responseBase.Message = $"Respuesta sin contenido";
            }
            else if (Guid.TryParse(contentResponseCopy.Replace("\"", ""), out _))
            {
                responseBase.Data = JsonConvert.DeserializeObject<T>(contentResponseCopy);
            }
            else if (typeof(T).IsPrimitive || typeof(T) == typeof(string))
            {
                contentResponse = contentResponse.Replace("\"", "");
                responseBase.Data = (T)Convert.ChangeType(contentResponse, type);
            }
            else if (contentResponse.StartsWith("{") || contentResponse.EndsWith("}"))
            {
                responseBase.Data = JsonConvert.DeserializeObject<T>(contentResponse);
            }
            else if (contentResponse.StartsWith("[") || contentResponse.EndsWith("]"))
            {
                responseBase.Data = JsonConvert.DeserializeObject<T>(contentResponse);
            }
            else
            {
                responseBase.Data = default(T);
                responseBase.Code = (int)HttpStatusCode.NotAcceptable;
                responseBase.Message = "El contenido no se puede convertir al objeto esperado";
            }
        }
    }

    public class IgnoreJsonAttributesResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            foreach (var prop in props)
            {
                prop.Ignored = false;   // Ignore [JsonIgnore]
                //prop.Converter = null;  // Ignore [JsonConverter]
                //prop.PropertyName = prop.UnderlyingName;  // restore original property name
            }
            return props;
        }
    }
}
