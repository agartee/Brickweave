using System;
using System.Text;

namespace Brickweave.Cqrs.SqlServer.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetFullMessage(this Exception exception)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(exception.Message);

            var innerException = exception.InnerException;
            while(innerException != null)
            {
                stringBuilder.Append(Environment.NewLine + innerException.Message);
                innerException = innerException.InnerException;
            }

            return stringBuilder.ToString();
        }
    }
}
