using System.Globalization;
using System.Web.Mvc;

namespace WebApplication.Models.Binders
{
    public class FloatModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var number = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0];


            if (string.IsNullOrEmpty(number))
                return 0;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, bindingContext.ValueProvider.GetValue(bindingContext.ModelName));

            try
            {

                switch (separator)
                {
                    case ',':
                        if (number.Contains("."))
                            return float.Parse(number.Replace('.', ','));

                        return float.Parse(number);

                    case '.':
                        if (number.Contains(","))
                            return float.Parse(number.Replace(',', '.'));

                        return float.Parse(number);

                    default:
                        return 0;
                }

            }
            catch
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("\"{0}\" is invalid.", bindingContext.ModelName));

                return 0;
            }

        }
    }
}