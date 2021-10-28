using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace AdvancedCqrs.WebApp.Formatters
{
    public class PlainTextInputFormatter : TextInputFormatter
    {
        public PlainTextInputFormatter()
        {
            SupportedMediaTypes.Add("text/plain");
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(string);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var httpContext = context.HttpContext;

            using (var reader = new StreamReader(httpContext.Request.Body, encoding))
            {
                return InputFormatterResult.Success(await reader.ReadToEndAsync());
            }
        }
    }
}
