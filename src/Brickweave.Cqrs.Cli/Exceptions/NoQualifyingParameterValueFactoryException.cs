using System;

namespace Brickweave.Cqrs.Cli.Exceptions
{
    public class NoQualifyingParameterValueFactoryException : Exception
    {
        private const string MESSAGE = "No qualifying parameter value factory found for type: \"{0}\"";

        public NoQualifyingParameterValueFactoryException(string typeShortName)
            :base(string.Format(MESSAGE, typeShortName))
        {
            TypeShortName = typeShortName;
        }

        public string TypeShortName { get; }
    }
}