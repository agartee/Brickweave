﻿using System;
using Brickweave.Domain;

namespace Brickweave.Samples.Domain.Persons.Models
{
    public class PhoneId : Id<Guid>
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
