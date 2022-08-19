using Business.DTOs.File;
using FluentValidation;

namespace Business.Validations
{
    public class FileValidation : AbstractValidator<FileUploadDto>
    {
        public FileValidation()
        {
            RuleFor(f => f.File)
                .NotNull()
                .NotEmpty();
        }
    }
}
