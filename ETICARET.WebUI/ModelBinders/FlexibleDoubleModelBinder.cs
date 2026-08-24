using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ETICARET.WebUI.ModelBinders
{
    public sealed class FlexibleDoubleModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);
            var rawValue = valueResult.FirstValue;
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Puan alanı zorunludur.");
                return Task.CompletedTask;
            }

            var normalizedValue = rawValue.Trim().Replace(',', '.');
            if (double.TryParse(normalizedValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            {
                bindingContext.Result = ModelBindingResult.Success(value);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Puan değeri geçersizdir.");
            }

            return Task.CompletedTask;
        }
    }
}
