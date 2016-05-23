using System.Web.Mvc;

namespace WebApplication.Models.Binders
{
    public class StringModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueResult == null || string.IsNullOrEmpty(valueResult.AttemptedValue))
                return null;

            //in case someone just added whitespaces we don't want to save "" in the db
            if (string.IsNullOrEmpty(valueResult.AttemptedValue.Trim()))
                return null;

            return valueResult.AttemptedValue.Trim();
        }
    }
}