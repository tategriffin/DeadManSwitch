using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace DeadManSwitch.UI.Web.AspNetMvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString CheckInLink(this HtmlHelper html, string linkText, object htmlAttributes = null)
        {
            var anchorBuilder = BuildCheckInAnchorTagBuilder(html, "CheckIn", "Home", null, htmlAttributes);
            anchorBuilder.InnerHtml = linkText;
            var rawHtml = anchorBuilder.ToString();

            return new MvcHtmlString(rawHtml);
        }


        public static MvcHtmlString DeleteScheduleLink(this HtmlHelper html, string linkText, int scheduleId)
        {
            var anchorBuilder = BuildDeleteAnchorTagBuilder(html, "Delete", "Schedule", new { id = scheduleId });
            anchorBuilder.InnerHtml = linkText;
            var rawHtml = anchorBuilder.ToString();

            return new MvcHtmlString(rawHtml);
        }

        public static MvcHtmlString DeleteScheduleImageLink(this HtmlHelper html, string imgSource, int scheduleId)
        {
            var anchorBuilder = BuildDeleteAnchorTagBuilder(html, "Delete", "Schedule", new { id = scheduleId });
            anchorBuilder.InnerHtml = BuildImgTag(html, imgSource, "Delete schedule");
            var rawHtml = anchorBuilder.ToString();

            return new MvcHtmlString(rawHtml);
        }

        public static MvcHtmlString DeleteActionStepImageLink(this HtmlHelper html, string imgSource, int stepId)
        {
            var anchorBuilder = BuildDeleteAnchorTagBuilder(html, "Delete", "Action", new { id = stepId });
            anchorBuilder.InnerHtml = BuildImgTag(html, imgSource, "Delete step");
            var rawHtml = anchorBuilder.ToString();

            return new MvcHtmlString(rawHtml);
        }

        public static MvcHtmlString ImageLink(this HtmlHelper html, string imgSource, string action, string controller, object routeValues, string altText)
        {
            var anchorBuilder = BuildAnchorTagBuilder(html, action, controller, routeValues);
            anchorBuilder.InnerHtml = BuildImgTag(html, imgSource, altText);
            var rawHtml = anchorBuilder.ToString();

            return new MvcHtmlString(rawHtml);
        }

        /// <summary>
        /// Create a hyperlink that does a post instead of a get for check in
        /// </summary>
        private static TagBuilder BuildCheckInAnchorTagBuilder(HtmlHelper html, string action, string controller, object routeValues, object htmlAttributes = null)
        {
            var anchorBuilder = BuildAnchorTagBuilder(html, action, controller, routeValues, htmlAttributes);
            anchorBuilder.MergeAttribute("onclick", "return postCheckin(this.href);");

            return anchorBuilder;
        }

        /// <summary>
        /// Create a hyperlink that does a post instead of a get for deleting items
        /// </summary>
        private static TagBuilder BuildDeleteAnchorTagBuilder(HtmlHelper html, string action, string controller, object routeValues)
        {
            var anchorBuilder = BuildAnchorTagBuilder(html, action, controller, routeValues);
            //http://haacked.com/archive/2009/01/30/simple-jquery-delete-link-for-asp.net-mvc.aspx/
            anchorBuilder.MergeAttribute("onclick", "return postDelete('Are you sure you want to delete?', this.href);");

            return anchorBuilder;
        }

        private static TagBuilder BuildAnchorTagBuilder(HtmlHelper html, string action, string controller, object routeValues, object htmlAttributes = null)
        {
            var anchorBuilder = new TagBuilder("a");
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            anchorBuilder.MergeAttribute("href", urlHelper.Action(action, controller, routeValues));

            if (htmlAttributes != null)
            {
                var attributes =
                    (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                anchorBuilder.MergeAttributes(attributes);
            }

            return anchorBuilder;
        }

        private static string BuildImgTag(HtmlHelper html, string src, string altText = null, object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string imgUrl = urlHelper.Content(src);     //Convert "~" to correct path

            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", imgUrl);
            
            if (!string.IsNullOrWhiteSpace(altText))
            {
                imgBuilder.MergeAttribute("alt", altText);
            }

            if (htmlAttributes != null)
            {
                var attributes =
                    (IDictionary<string, object>) HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                imgBuilder.MergeAttributes(attributes);
            }

            return imgBuilder.ToString(TagRenderMode.SelfClosing);
        }

        public static MvcHtmlString CheckBoxToggleButtonFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return html.CheckBoxToggleButtonFor(expression, labelText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString CheckBoxToggleButtonFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, IDictionary<string, object> htmlAttributes)
        {
            return html.LabelFor(expression, labelText, new { @class = "checkbox_label" });
        }

        public static MvcHtmlString LabelForCheckBox<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText)
        {
            return html.LabelFor(expression, labelText, new {@class = "checkbox_label"});
        }

        public static MvcHtmlString ResolveUrl(this HtmlHelper htmlHelper, string url)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            return MvcHtmlString.Create(urlHelper.Content(url));
        }

    }
}