using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Text;
using System.Web.WebPages.Html;
using System.Security.Principal;
using DA.Dinners.Domain.Concrete;

namespace UI.Heplpers
{

    //public static class Extensions
    //{
    //    public static string GetDomain(this IIdentity identity)
    //    {
    //        string s = identity.Name;
    //        int stop = s.IndexOf("\\");
    //        return (stop > -1) ? s.Substring(0, stop) : string.Empty;
    //    }

    //    public static string GetLogin(this IIdentity identity)
    //    {
    //        string s = identity.Name;
    //        int stop = s.IndexOf("\\");
    //        return (stop > -1) ? s.Substring(stop + 1, s.Length - stop - 1) : string.Empty;
    //    }
    //}

    public static class HeplerExtentions
    {
        public static HtmlString ADUserInfo(this System.Web.Mvc.HtmlHelper helper, System.Security.Principal.IPrincipal user)
        {
            StringBuilder info = new StringBuilder();
            info.AppendFormat("<b>{0}</b>: {1}<br/>", "Имя", DomainService.Instance.GetFullName(user.Identity.Name));
            info.AppendFormat("<b>{0}</b>: {1}<br/>", "Город", DomainService.Instance.GetCity(user.Identity.Name));
            info.AppendFormat("<b>{0}</b>: {1}<br/>", "Позиция", DomainService.Instance.GetPosition(user.Identity.Name));
            info.AppendFormat("<b>{0}</b>: {1}<br/>", "Оффис", DomainService.Instance.GetOffice(user.Identity.Name));
            info.AppendFormat("<b>{0}</b>: {1}<br/>", "Страна", DomainService.Instance.GetCountry(user.Identity.Name));
            info.AppendFormat("<b>{0}</b>: {1}<br/>", "Email", DomainService.Instance.GetEmail(user.Identity.Name));
            return new HtmlString(info.ToString());
        }
    }
}