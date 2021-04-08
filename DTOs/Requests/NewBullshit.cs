using BSTracker.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace BSTracker.DTOs.Requests
{
    public class NewBullshit
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public string Note { get; set; }

        [Required]
        public string WhoSaidIt { get; set; }

        public Bullshit GetBullshit()
            => new()
            {
                CreatedAt = DateTimeOffset.UtcNow,
                Id = Guid.NewGuid().ToString(),
                Note = Note,
                Text = Text,
                WhoSaidIt = WhoSaidIt
            };
    }
}
