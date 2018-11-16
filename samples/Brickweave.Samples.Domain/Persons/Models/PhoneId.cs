using System;
using Brickweave.Domain;

namespace Brickweave.Samples.Domain.Persons.Models
{
    public class PhoneId : ValueObject<Guid>
    {
        public PhoneId(Guid value) : base(value)
        {
        }

        public static PhoneId NewId()
        {
            return new PhoneId(Guid.NewGuid());
        }
    }
}
