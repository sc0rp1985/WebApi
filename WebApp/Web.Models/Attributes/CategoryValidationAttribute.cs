using Common;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Attributes
{
    public sealed class CategoryValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return Const.Categories.Contains((string)value);
        }
    }
}