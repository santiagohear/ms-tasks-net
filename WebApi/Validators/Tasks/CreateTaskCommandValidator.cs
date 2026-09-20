using Application.Tasks.CreateTask;
using Domain.Entities;
using Domain.Exceptions;
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

            RuleFor(x => x.AdditionalInfoJson)
                .Must(BeValidAdditionalInfoJson)
                .WithMessage("Additional info must be a valid JSON object");

            RuleFor(x => x.Priority)
                .Must(priority => priority is null || !string.IsNullOrWhiteSpace(priority))
                .WithMessage("Priority cannot be empty");

            RuleForEach(x => x.Tags)
                .NotEmpty().WithMessage("Tags cannot contain empty values");
        }

        private static bool BeValidAdditionalInfoJson(string? additionalInfoJson)
        {
            try
            {
                TaskAdditionalInfo.EnsureValidJson(additionalInfoJson);
                return true;
            }
            catch (Domain.Exceptions.ValidationException)
            {
                return false;
            }
        }
    }
}
