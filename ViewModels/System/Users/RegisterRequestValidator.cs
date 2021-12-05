using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.System.Users
{
   public  class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {


        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("first name is reqired")
                .MaximumLength(200).WithMessage("first name can not over 200 character");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("last namr is required")
                .MaximumLength(200).WithMessage("last name can not over 200 characters");
            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("birth day cannot greater than 100 years");
            RuleFor(x => x.Email).NotEmpty().WithMessage("email is requireds")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("email format not match");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("passord is required")
            .MinimumLength(6).WithMessage("Password is at least 6 characters");
            RuleFor(x => x).Custom((request, context) => { 
            
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("the list must contain 10 items or fewer");
                }

            
            });
        }

   }  
}
