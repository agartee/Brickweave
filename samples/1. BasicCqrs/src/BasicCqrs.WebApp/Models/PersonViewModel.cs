using System.ComponentModel.DataAnnotations;
using BasicCqrs.Domain.People.Models;

namespace BasicCqrs.WebApp.Models
{
    /// <summary>
    /// This class must exist because at the time of this writing, System.Text.Json deserialization 
    /// does not support deserialization of immutable objects. Since we do not want to compromise 
    /// our domain libraries by making command and query object mutable, a view model is utilized.
    /// </summary>
    public class PersonViewModel
    {
        public PersonId Id { get; init; }
        [Required]
        public string FirstName { get; init; }
        [Required]
        public string LastName { get; init; }
    }
}
