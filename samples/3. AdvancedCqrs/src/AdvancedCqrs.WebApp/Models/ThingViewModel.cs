using System.ComponentModel.DataAnnotations;
using AdvancedCqrs.Domain.Things.Models;

namespace AdvancedCqrs.WebApp.Models
{
    public class ThingViewModel
    {
        public ThingId Id { get; init; }
        [Required]
        public string Name { get; init; }
    }
}
