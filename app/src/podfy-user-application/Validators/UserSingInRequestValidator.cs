using FluentValidation;
using podfy_user_application.Model;

namespace podfy_user_application.Validators;

internal class UserSingInRequestValidator: AbstractValidator<UserSingInRequest>
{
	public UserSingInRequestValidator()
	{
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email field is requied").EmailAddress().WithMessage("The Email address is not valid");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password field is required").Length(6, 50).WithMessage("The Password field must be between 6 to 50 characters.");
    }
}

