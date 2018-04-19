using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Jahshaka.API.ViewModels.Shared;

namespace Jahshaka.API.ViewModels.Mappers
{
    public static partial class Mappers
    {
        public static string GetFirstError(this ModelStateDictionary source)
        {
            if (source == null)
            {
                return null;
            }

            return source?.Values.FirstOrDefault(m => m.ValidationState == ModelValidationState.Invalid)?.Errors.FirstOrDefault()?.ErrorMessage;
        }

        public static ErrorViewModel GetErrorResponse(this ModelStateDictionary source)
        {
            return new ErrorViewModel() { ErrorDescription = source.GetFirstError() };
        }
    }
}
