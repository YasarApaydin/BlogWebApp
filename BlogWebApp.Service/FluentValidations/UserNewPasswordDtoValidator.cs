using BlogWebApp.Entity.DTOs.Users;
using FluentValidation;

namespace BlogWebApp.Service.FluentValidations
{
    public class UserNewPasswordDtoValidator : AbstractValidator<UserNewPasswordDto>
    {
        public UserNewPasswordDtoValidator()
        {
            RuleFor(x => x.MevcutSifre)
       .NotEmpty().WithMessage("Mevcut şifre boş olamaz.");

            RuleFor(x => x.YeniSifre)
          .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
          .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalı.");



            RuleFor(x => x.YeniSifreTekrar)
                .Equal(x => x.YeniSifre);

        }

    }
}
