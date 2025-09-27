using BlogWebApp.Entity.Entities;
using FluentValidation;

namespace BlogWebApp.Service.FluentValidations
{
    public class ArticleValidator:AbstractValidator<Makale>
    {

        public ArticleValidator()
        {
            RuleFor(x => x.Baslik)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(150)
                .WithName("Başlık");
            RuleFor(x => x.Icerik)
             .NotEmpty()
             .NotNull()
             .MinimumLength(10)
             .WithName("Icerik");
            RuleFor(x => x.Ozet)
          .NotEmpty()
          .NotNull()
          .MinimumLength(10)
          .MaximumLength(250)
          .WithName("Ozet");
        }
    }
}
