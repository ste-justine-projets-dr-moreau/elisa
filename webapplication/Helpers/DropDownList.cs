using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace WebApplication.Helpers
{
    public static class DropDownList
    {
        public static IEnumerable<SelectListItem> LeftRight(string langue)
        {
            // Remplit le DropDown statiquement

            var leftright = new List<SelectListItem>
                {
                    new SelectListItem
                        {
                            Text = Languages.Global.Right,
                            Value = "true"
                        },
                    new SelectListItem
                        {
                            Text = Languages.Global.Left,
                            Value = "false"
                        }
                };


            return leftright;
        }

    }
}