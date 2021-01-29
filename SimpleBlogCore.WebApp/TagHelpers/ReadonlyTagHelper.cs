using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SimpleBlogCore.WebApp.TagHelpers
{
    [HtmlTargetElement("input", Attributes = "asp-for")]
    [HtmlTargetElement("textarea", Attributes = "asp-for")]
    public class ReadonlyTagHelper : TagHelper
    {
        public bool IsReadonly { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (IsReadonly)
            {
                output.Attributes.SetAttribute("readonly", null);
            }
        }
    }
}
