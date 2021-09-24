using BasicCqrs.Domain.People.Models;
using BasicCqrs.WebApp.Models;

namespace BasicCqrs.WebApp.Extensions
{
    public static class PersonExtensions
    {
        /// <summary>
        /// Helper method to convert the domain model into a view model for the ASP.NET Razor pages.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public static PersonViewModel ToViewModel(this Person person)
        {
            return new PersonViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName
            };
        }
    }
}
