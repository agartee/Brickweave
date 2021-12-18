using AdvancedCqrs.Domain.Things.Models;

namespace AdvancedCqrs.WebApp.Models
{
    public class ThingViewModel
    {
        public ThingId Id { get; set; }
        public string Name { get; set; }
    }
}
