using System.ComponentModel.DataAnnotations;

namespace Shoppng_Tutorial.Repository.Validation
{
    public class FileExtentionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName); //123.jpg
                string[] extensions = { "jpg", "png", "jpeg" };

                bool result = extensions.Any(x => extension.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult("Chỉ cho phép các định dạng ảnh: jpg, png, hoặc jpeg");
                }
            }
            return ValidationResult.Success;
        }

    }
}
