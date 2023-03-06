using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Models.Attributes
{
    public sealed class ColorValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return Const.Colors.Contains((string)value);
        }
    }
}
