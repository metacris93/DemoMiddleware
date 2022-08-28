using System;
using System.Text;

namespace DemoMiddleware.Middleware
{
    public class RequestResponseLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //First, get the incoming request
            var request = await FormatRequest(context.Request);
            Console.WriteLine($"REQUEST --> {request}");
            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;
                //Continue down the Middleware pipeline, eventually returning to this class
                await next(context);
                //Format the response from the server
                var response = await FormatResponse(context.Response);
                // Save Log to chosen datastore
                Console.WriteLine($"RESPONSE --> {response}");
                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        private async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;
            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();
            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            //var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            //...Then we copy the entire request stream into the new buffer.
            //await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = await new StreamReader(request.Body).ReadToEndAsync();
            //We convert the byte[] into a string using UTF8 encoding...
            //var bodyAsText = Encoding.UTF8.GetString(buffer);
            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body.Position = 0;
            var requestHeaders = request.Headers.Select(x => $"{x.Key} => {x.Value}");
            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {string.Join(", ", requestHeaders)} {bodyAsText}";
        }
        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseHeaders = response.Headers.Select(x => $"{x.Key} => {x.Value}");
            return $"{response.StatusCode} {string.Join(", ", responseHeaders)}: {text}";
        }
    }
}

