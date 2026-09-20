using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class User : BaseEntity<int>
    {
        [MaxLength(100)]
        public string UserName { get; set; } = default!;

        [MaxLength(255)]
        public string Email { get; set; } = default!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public ICollection<TaskItem> AssignedTasks { get; set; } = [];

        public ICollection<TaskItem> CreatedTasks { get; set; } = [];
    }
}
