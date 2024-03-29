﻿using BasicMessaging.Domain.Places.Models;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Commands
{
    public class CreatePlace : ICommand<Place>
    {
        public CreatePlace(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
