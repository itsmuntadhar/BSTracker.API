using System;
using System.ComponentModel.DataAnnotations;

namespace BSTracker.Entities
{
    public class Bullshit : Entity
    {
        [MaxLength(1024)]
        [Required]
        public string Text { get; set; }

        [MaxLength(4096)]
        public string Note { get; set; }

        [MaxLength(512)]
        [Required]
        public string WhoSaidIt { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
