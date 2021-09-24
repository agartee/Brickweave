using BasicMessaging.Domain.Places.Models;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Commands
{
    public class CreatePlace : ICommand<Place>
    {
        /// <summary>
        /// Create a new place.
        /// </summary>
        /// <param name="name">The name of the place.</param>
        public CreatePlace(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
