using System.ComponentModel.DataAnnotations;
using BasicCqrs.Domain.People.Models;

namespace BasicCqrs.WebApp.Models
{
    public class PersonViewModel
    {
        public PersonId Id { get; init; }
        [Required]
        public string FirstName { get; init; }
        [Required]
        public string LastName { get; init; }
    }
}
