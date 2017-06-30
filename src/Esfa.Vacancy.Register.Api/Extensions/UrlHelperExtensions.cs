using System;
using System.Web;
using System.Web.Mvc;

namespace Esfa.Vacancy.Register.Api.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string Content(this UrlHelper urlHelper, string contentPath, bool toAbsolute = false)
        {
            var path = urlHelper.Content(contentPath);
            var url = new Uri(HttpContext.Current.Request.Url, path);

            return toAbsolute ? url.AbsoluteUri : path;
        }
    }
}