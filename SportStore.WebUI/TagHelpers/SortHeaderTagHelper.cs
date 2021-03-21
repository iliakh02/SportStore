using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportStore.WebUI.Areas.Admin.Models;

namespace SportStore.WebUI.TagHelpers
{
    public class SortHeaderTagHelper : TagHelper
    {
        public UsersSortState Property { get; set; }
        public UsersSortState Current { get; set; }
        public string Action { get; set; }

        private IUrlHelperFactory urlHelperFactory;

        public SortHeaderTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";

            string url = urlHelper.Action(Action, new { sortOrder = Property });
            output.Attributes.SetAttribute("href", url);
            output.Attributes.SetAttribute("class", "text-light");
        }
    }
}
