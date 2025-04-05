using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class BaseEntity<TEntityId> 
    {
        [Key]
        public TEntityId Id { get; set; } = default!;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
