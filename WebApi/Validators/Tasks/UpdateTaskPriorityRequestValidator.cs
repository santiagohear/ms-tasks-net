using FluentValidation;
using WebApi.Contracts.Tasks;

namespace WebApi.Validators.Tasks
{
    public class UpdateTaskPriorityRequestValidator : AbstractValidator<UpdateTaskPriorityRequest>
    {
        public UpdateTaskPriorityRequestValidator()
        {
            RuleFor(x => x.Priority)
                .NotEmpty().WithMessage("Priority is required");
        }
    }
}
