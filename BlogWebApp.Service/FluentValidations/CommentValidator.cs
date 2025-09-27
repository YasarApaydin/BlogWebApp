using BlogWebApp.Entity.Entities;
using FluentValidation;

namespace BlogWebApp.Service.FluentValidations
{
    public class CommentValidator: AbstractValidator<Yorum>
    {

        public CommentValidator()
        {
            RuleFor(x => x.Icerik)
              .NotEmpty()
              .MinimumLength(5)
              .MaximumLength(1000);

            RuleFor(x => x.MakaleId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
