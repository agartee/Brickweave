using System;

namespace EventSourcing.Domain.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        private const string MESSAGE = "Unable to find {0} with ID: {1}";

        public EntityNotFoundException(object id, string typeName) : base(string.Format(MESSAGE, typeName, id))
        {
            Id = id;
            TypeName = typeName;

            Data.Add("Id", id.ToString());
            Data.Add("TypeName", typeName);
        }

        public object Id { get; }
        public string TypeName { get; }
    }
}
