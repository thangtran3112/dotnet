using System.ComponentModel.DataAnnotations;

namespace backend.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public long ID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // MsSql Server does not have native support for boolean, the entity framework will convert it to bit (1/0)
        public bool IsActive { get; set; } = true;
    }
}