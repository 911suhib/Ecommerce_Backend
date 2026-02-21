using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EcommerceBackend.Domain.Entities
{
     public class BaseEntity
    {
        public int Id { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
