using BlogWebApp.Entity.Entities;
using FluentValidation;

namespace BlogWebApp.Service.FluentValidations
{
    public class CategoryValidator:AbstractValidator<Kategori>
    {

        public CategoryValidator()
        {
            RuleFor(x => x.Ad)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(100)
                .WithName("Kategori Adı");
        }
    }
}
