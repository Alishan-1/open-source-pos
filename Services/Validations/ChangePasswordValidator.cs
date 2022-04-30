using FluentValidation;
using Models;

namespace Services.Validations
{
    public class ChangePasswordValidator : AbstractValidator<UserCred>
    {
        //constructor used only for auto fact module 
        public ChangePasswordValidator()
        {
            RuleFor(user => user.UserEmail).NotEmpty();
            RuleFor(user => user.UserID).NotEmpty().GreaterThan(0).Must(uid => uid == 1).WithMessage("Invalid Password Change Request");
            RuleFor(user => user.UserPassword).NotEmpty();
        }
        public ChangePasswordValidator(int UserIDFromToken)
        {
            RuleFor(user => user.UserEmail).NotEmpty();
            RuleFor(user => user.UserID).NotEmpty().GreaterThan(0).Must(uid => uid == UserIDFromToken).WithMessage("Invalid Password Change Request");
            RuleFor(user => user.UserPassword).NotEmpty();
        }

        
    }

    public class ForgetPasswordValidator : AbstractValidator<UserCred>
    {
        
        public ForgetPasswordValidator()
        {
            RuleFor(user => user.UserEmail).NotEmpty().EmailAddress();
            
        }
        


    }
}
