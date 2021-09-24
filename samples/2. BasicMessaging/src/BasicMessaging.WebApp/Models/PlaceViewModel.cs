using System;
using System.ComponentModel.DataAnnotations;

namespace BasicMessaging.WebApp.Models
{
    public class PlaceViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
