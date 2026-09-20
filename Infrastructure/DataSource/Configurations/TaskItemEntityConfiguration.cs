using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataSource.Configurations
{
    public class TaskItemEntityConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("CK_Tasks_Status", "[Status] IN (N'Pending', N'InProgress', N'Done')");
                tableBuilder.HasCheckConstraint("CK_Tasks_AdditionalInfoJson_IsJson", "[AdditionalInfoJson] IS NULL OR ISJSON([AdditionalInfoJson]) = 1");
            });

            builder.HasKey(x => x.Id)
                .HasName("PK_Tasks");

            builder.Property(x => x.Id)
                .HasColumnName("TaskId")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("Title")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("Description")
                .HasMaxLength(2000);

            builder.Property(x => x.Status)
                .HasColumnName("Status")
                .HasMaxLength(30)
                .HasDefaultValue(TaskItem.PendingStatus)
                .IsRequired();

            builder.Property(x => x.AssignedToUserId)
                .HasColumnName("AssignedToUserId")
                .IsRequired();

            builder.Property(x => x.CreatedByUserId)
                .HasColumnName("CreatedByUserId")
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .HasColumnName("CreatedAtUtc")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.Property(x => x.UpdatedAtUtc)
                .HasColumnName("UpdatedAtUtc");

            builder.Property(x => x.EstimatedFinishDate)
                .HasColumnName("EstimatedFinishDate")
                .HasColumnType("date");

            builder.Property(x => x.AdditionalInfoJson)
                .HasColumnName("AdditionalInfoJson")
                .HasColumnType("nvarchar(max)");

            builder.HasOne(x => x.AssignedToUser)
                .WithMany(x => x.AssignedTasks)
                .HasForeignKey(x => x.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Tasks_AssignedToUser");

            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CreatedTasks)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Tasks_CreatedByUser");

            builder.HasIndex(x => new { x.AssignedToUserId, x.Status })
                .HasDatabaseName("IX_Tasks_AssignedToUserId_Status");

            builder.HasIndex(x => x.EstimatedFinishDate)
                .HasDatabaseName("IX_Tasks_EstimatedFinishDate");
        }
    }
}
