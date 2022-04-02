using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    public class HttpResponseMessageResult : IActionResult
    {
        private readonly HttpResponseMessage _responseMessage;
        private readonly string contentType;

        public HttpResponseMessageResult(HttpResponseMessage responseMessage, string contentType = "application/json")
        {
            _responseMessage = responseMessage; // could add throw if null
            this.contentType = contentType;
        }

        public HttpResponseMessage ResponseMessage => _responseMessage;

        public async Task ExecuteResultAsync(ActionContext context)
        {
            try
            {
                context.HttpContext.Response.StatusCode = (int)ResponseMessage.StatusCode;

                foreach (var header in ResponseMessage.Headers)
                {
                    context.HttpContext.Response.Headers.TryAdd(header.Key, new StringValues(header.Value.ToArray()));
                }
                if (!string.IsNullOrEmpty(contentType))
                    context.HttpContext.Response.Headers.Add("content-type", contentType);

                if (context.HttpContext.Response.Body != null)
                    using (var stream = await ResponseMessage.Content.ReadAsStreamAsync())
                    {
                        await stream.CopyToAsync(context.HttpContext.Response.Body);
                        await context.HttpContext.Response.Body.FlushAsync();
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }


        }

    }
}
