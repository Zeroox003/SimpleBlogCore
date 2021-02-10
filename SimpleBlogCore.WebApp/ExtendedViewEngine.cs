using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace SimpleBlogCore.WebApp
{
    public class ExtendedViewEngine : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[] {
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Views/{1}/{0}.cshtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/Post/{0}.cshtml",
            };
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}
