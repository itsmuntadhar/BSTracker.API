using System;
using System.ComponentModel.DataAnnotations;

namespace BSTracker.Entities
{
    public abstract class Entity
    {
        [MaxLength(36)]
        public string Id { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
