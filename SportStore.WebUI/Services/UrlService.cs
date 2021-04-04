using SportStore.WebUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SportStore.WebUI.Services
{
    public class UrlService : IUrlService
    {
        public string ReditectUrlForDelete(int id, int pageSize, string queries)
        {
            var nvc = HttpUtility.ParseQueryString(queries);
            List<string> parameters = new List<string>();
            if (nvc.AllKeys.Contains("searchString"))
                parameters.Add($"searchString={nvc["searchString"]}");

            if (nvc.AllKeys.Contains("page"))
                parameters.Add($"page={Int32.Parse(nvc["page"]) - ((pageSize > 1) ? 0 : 1)}");

            if (nvc.AllKeys.Contains("sortOrder"))
                parameters.Add($"sortOrder={nvc["sortOrder"]}");

            string urlParameters = string.Join("&", parameters);
            string redirectUrl = @"\Products";
            if (!string.IsNullOrEmpty(urlParameters))
                redirectUrl += "?";
            redirectUrl += urlParameters;

            return redirectUrl;
        }
    }
}
