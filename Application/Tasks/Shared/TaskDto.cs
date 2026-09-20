namespace Application.Tasks.Shared
{
    public class TaskDto
    {
        public long Id { get; set; }

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public string Status { get; set; } = default!;

        public int AssignedToUserId { get; set; }

        public string AssignedToUserName { get; set; } = default!;

        public int CreatedByUserId { get; set; }

        public string CreatedByUserName { get; set; } = default!;

        public DateTime CreatedAtUtc { get; set; }

        public DateTime? UpdatedAtUtc { get; set; }

        public DateTime? EstimatedFinishDate { get; set; }

        public string? AdditionalInfoJson { get; set; }
    }
}
