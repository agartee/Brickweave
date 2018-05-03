﻿namespace Brickweave.Samples.Domain.Persons.Models
{
    public class PhoneInfo
    {
        public PhoneInfo(PhoneId id, string number)
        {
            Id = id;
            Number = number;
        }

        public PhoneId Id { get; }
        public string Number { get; }
    }
}
