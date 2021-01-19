using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace SimpleBlogCore.WebApp
{
    public class ExtendedViewEngine : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[] {
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
