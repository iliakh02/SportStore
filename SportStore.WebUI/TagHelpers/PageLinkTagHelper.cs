using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }
        public object SortOrder { get; set; }
        public string SearchString { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PageModel.TotalPages == 1)
                return;

            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            TagBuilder tag = new TagBuilder("ul");
            tag.AddCssClass("pagination");

            TagBuilder currentItem = CreateTag(PageModel.PageNumber, urlHelper);

            if(PageModel.HasPreviousPage)
            {
                // Go to previous page.
                TagBuilder item = new TagBuilder("li");
                TagBuilder link = new TagBuilder("a");

                link.Attributes["href"] = urlHelper.Action(PageAction, new { searchString = SearchString, page = PageModel.PageNumber - 1, sortOrder = SortOrder });

                item.AddCssClass("page-item");
                link.AddCssClass("page-link");
                link.InnerHtml.Append("<");
                item.InnerHtml.AppendHtml(link);
                tag.InnerHtml.AppendHtml(item);

                // Go to first page.
                if(PageModel.NeedGoToFirstPage)
                {
                    TagBuilder firstPage = CreateTag(1, urlHelper);
                    tag.InnerHtml.AppendHtml(firstPage);
                }

                // Points.
                if(PageModel.HasPrePoints)
                {
                    TagBuilder pointsItem = new TagBuilder("li");
                    TagBuilder points = new TagBuilder("span");
                    points.InnerHtml.Append("...");
                    pointsItem.InnerHtml.AppendHtml(points);
                    tag.InnerHtml.AppendHtml(pointsItem);
                }

                // Show link on previous page.
                TagBuilder prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            // Current page.
            tag.InnerHtml.AppendHtml(currentItem);

            if(PageModel.HasNextPage)
            {
                // Show link on next page.
                TagBuilder nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);

                // Points.
                if (PageModel.HasPostPoints)
                {
                    TagBuilder pointsItem = new TagBuilder("li");
                    TagBuilder points = new TagBuilder("span");
                    points.InnerHtml.Append("...");
                    pointsItem.InnerHtml.AppendHtml(points);
                    tag.InnerHtml.AppendHtml(pointsItem);
                }

                // Go to last page.
                if (PageModel.NeedGoToLastPage)
                {
                    TagBuilder firstPage = CreateTag(PageModel.TotalPages, urlHelper);
                    tag.InnerHtml.AppendHtml(firstPage);
                }

                // Go to next page.
                TagBuilder item = new TagBuilder("li");
                TagBuilder link = new TagBuilder("a");

                link.Attributes["href"] = urlHelper.Action(PageAction, new { searchString = SearchString, page = PageModel.PageNumber + 1, sortOrder = SortOrder });

                item.AddCssClass("page-item");
                link.AddCssClass("page-link");
                link.InnerHtml.Append(">");
                item.InnerHtml.AppendHtml(link);
                tag.InnerHtml.AppendHtml(item);
            }

            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");
            if(pageNumber == this.PageModel.PageNumber)
            {
                item.AddCssClass("active");
            } 
            else
            {
                link.Attributes["href"] = urlHelper.Action(PageAction, new { searchString = SearchString, page = pageNumber, sortOrder = SortOrder});
            }

            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
