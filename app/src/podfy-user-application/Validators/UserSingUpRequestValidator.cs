using FluentValidation;
using podfy_user_application.Model;
using System.Diagnostics.CodeAnalysis;

namespace podfy_user_application.Validators;

[ExcludeFromCodeCoverage]
public class UserSingUpRequestValidator : AbstractValidator<UserSingUpRequest>
{
    public UserSingUpRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email field is requied").EmailAddress().WithMessage("The Email address is not valid");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password field is required").Length(6, 50).WithMessage("The Password field must be between 6 to 50 characters.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name field is required").Length(3, 155).WithMessage("The Name field must be between 6 to 50 characters.");
    }
}