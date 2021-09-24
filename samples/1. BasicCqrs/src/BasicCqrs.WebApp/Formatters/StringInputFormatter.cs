using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace BasicCqrs.WebApp.Formatters
{
    /// <summary>
    /// Formatter that allows ASP.NET to process plain-text request data, which is sent by the 
    /// demo CLI client (/scripts/cli-client-nosecurity.ps1).
    /// </summary>
    public class StringInputFormatter : TextInputFormatter
    {
        public StringInputFormatter()
        {
            SupportedMediaTypes.Add("text/plain");
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);
        }

        public async override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context,
            Encoding encoding)
        {
            var httpContext = context.HttpContext;

            using (var reader = new StreamReader(httpContext.Request.Body, encoding))
            {
                return InputFormatterResult.Success(await reader.ReadToEndAsync());
            }
        }
    }
}
