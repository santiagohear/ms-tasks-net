using Application.Tasks.CreateTask;
using FluentValidation;

namespace WebApi.Validators.Tasks
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Task title is required")
                .MaximumLength(200).WithMessage("Task title must be at most 200 characters long");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must be at most 2000 characters long");

            RuleFor(x => x.AssignedToUserId)
                .GreaterThan(0).WithMessage("Assigned user id must be greater than 0");

            RuleFor(x => x.CreatedByUserId)
                .GreaterThan(0).WithMessage("Created by user id must be greater than 0");
        }
    }
}
