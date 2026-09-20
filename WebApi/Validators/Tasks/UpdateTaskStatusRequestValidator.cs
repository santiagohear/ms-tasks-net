using FluentValidation;
using WebApi.Contracts.Tasks;

namespace WebApi.Validators.Tasks
{
    public class UpdateTaskStatusRequestValidator : AbstractValidator<UpdateTaskStatusRequest>
    {
        public UpdateTaskStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(BeValidStatus).WithMessage("Status must be one of: Pending, InProgress, Done");
        }

        private static bool BeValidStatus(string status)
        {
            return status == "Pending" || status == "InProgress" || status == "Done";
        }
    }
}
