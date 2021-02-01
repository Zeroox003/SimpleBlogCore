using Microsoft.AspNetCore.Html;
using X.PagedList.Mvc.Core.Common;
using X.PagedList.Web.Common;

namespace SimpleBlogCore.WebApp
{
	public class CustomPagedListRenderOptions : PagedListRenderOptions
    {
        public CustomPagedListRenderOptions()
        {
            DisplayLinkToFirstPage = PagedListDisplayMode.Never;
            DisplayLinkToLastPage = PagedListDisplayMode.Never;
            DisplayLinkToPreviousPage = PagedListDisplayMode.Always;
            DisplayLinkToNextPage = PagedListDisplayMode.Always;
            DisplayLinkToIndividualPages = true;
            DisplayPageCountAndCurrentLocation = false;
            MaximumPageNumbersToDisplay = 5;
            DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            EllipsesFormat = "&#8230;";
            LinkToFirstPageFormat = "««";
            LinkToPreviousPageFormat = "Prev";
            LinkToIndividualPageFormat = "{0}";
            LinkToNextPageFormat = "Next";
            LinkToLastPageFormat = "»»";
            PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            FunctionToDisplayEachPageNumber = null;
            ClassToApplyToFirstListItemInPager = null;
            ClassToApplyToLastListItemInPager = null;
            ContainerDivClasses = new[] { "pgn" };
            UlElementClasses = null;
            LiElementClasses = null;
            PageClasses = new[] { "pgn__num" };

            FunctionToTransformEachPageLink = (liTag, aTag) => {
                // Clear default useless classes
                liTag.Attributes["class"] = null;

                // Arrow left class
				if (aTag.Attributes.ContainsKey("rel") && aTag.Attributes["rel"] == "prev")
				{
                    aTag.Attributes["class"] = "pgn__prev";
                }

                // Dots class
                string aTagInnerHtml = GetInnerHtmlString(aTag.InnerHtml);
                if (aTagInnerHtml == "&#8230;")
				{
                    aTag.Attributes["class"] = "pgn__num dots";
                }

                // Arrow right class
                if (aTag.Attributes.ContainsKey("rel") && aTag.Attributes["rel"] == "next")
                {
                    aTag.Attributes["class"] = "pgn__next";
                }

                if (aTag.Attributes.ContainsKey("rel") && !aTag.Attributes.ContainsKey("href"))
				{
					aTag.AddCssClass("inactive");
				}
				else if (!aTag.Attributes.ContainsKey("rel") && !aTag.Attributes.ContainsKey("href") && aTagInnerHtml != "&#8230;")
				{
					aTag.AddCssClass("current");
				}

                return aTag;
			};
		}

        private string GetInnerHtmlString(IHtmlContentBuilder htmlContentBuilder)
		{
            using (var writer = new System.IO.StringWriter())
			{
                htmlContentBuilder.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                return writer.ToString();
			}
		}
    }
}
