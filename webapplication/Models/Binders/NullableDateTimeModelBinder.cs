using System;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication.Models.Binders
{
    public class NullableDateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ValueProvider.GetValue(bindingContext.ModelName) == null)
                return null;

            var date = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;

            if (string.IsNullOrEmpty(date))
                return null;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, bindingContext.ValueProvider.GetValue(bindingContext.ModelName));

            try
            {
                if (date.Contains(":"))
                {
                    if (date.Contains("/"))
                    {
                        var dateToReturn = new DateTime();
                        try
                        {
                            //english default date
                            dateToReturn = DateTime.ParseExact(date, "d/M/yyyy h:mm:ss tt", null);
                        }
                        catch
                        {
                            try
                            {
                                //english date #2
                                dateToReturn = DateTime.ParseExact(date, "M/d/yyyy h:mm:ss tt", null);
                            }
                            catch
                            {
                                //french default date
                                dateToReturn = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss", null);
                            }
                        }

                        return dateToReturn;
                    }

                    if (date.Count(x => x == ':') == 1)
                    {
                        return DateTime.ParseExact(date, "yyyy-MM-dd HH:mm", null);
                    }

                    return DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", null);
                }

                if (date.Contains("/"))
                    return DateTime.ParseExact(date, "d/M/yyyy", null);

                return DateTime.ParseExact(date, "yyyy-MM-dd", null);
            }
            catch
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format("\"{0}\" is invalid.", bindingContext.ModelName));

                return null;
            }

        }

    }
}