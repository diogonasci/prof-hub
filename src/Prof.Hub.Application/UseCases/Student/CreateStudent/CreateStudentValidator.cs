using FluentValidation;

namespace Prof.Hub.Application.UseCases.Student.CreateStudent
{
    public class CreateStudentValidator : AbstractValidator<CreateStudentInput>
    {
        public CreateStudentValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("O primeiro nome é obrigatório.")
                .Length(1, 50).WithMessage("O primeiro nome deve ter entre 1 e 50 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("O sobrenome é obrigatório.")
                .Length(1, 50).WithMessage("O sobrenome deve ter entre 1 e 50 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("Formato de email inválido.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("O número de telefone é obrigatório.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("O número de telefone deve estar em um formato válido.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("O endereço é obrigatório.")
                .Length(1, 100).WithMessage("O endereço deve ter entre 1 e 100 caracteres.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("A cidade é obrigatória.")
                .Length(1, 50).WithMessage("A cidade deve ter entre 1 e 50 caracteres.");

            RuleFor(x => x.State)
                .NotEmpty().WithMessage("O estado é obrigatório.")
                .Length(2, 50).WithMessage("O estado deve ter entre 2 e 50 caracteres.");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("O código postal é obrigatório.")
                .Length(5, 10).WithMessage("O código postal deve ter entre 5 e 10 caracteres.");
        }
    }
}
