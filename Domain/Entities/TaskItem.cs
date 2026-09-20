using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;

namespace Domain.Entities
{
    public class TaskItem : BaseEntity<long>
    {
        public const string PendingStatus = "Pending";
        public const string InProgressStatus = "InProgress";
        public const string DoneStatus = "Done";

        [MaxLength(200)]
        public string Title { get; set; } = default!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [MaxLength(30)]
        public string Status { get; set; } = PendingStatus;

        public int AssignedToUserId { get; set; }

        public User AssignedToUser { get; set; } = default!;

        public int CreatedByUserId { get; set; }

        public User CreatedByUser { get; set; } = default!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        public DateTime? EstimatedFinishDate { get; set; }

        public string? AdditionalInfoJson { get; set; }
    }
}
