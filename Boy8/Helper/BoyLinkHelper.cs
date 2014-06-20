using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Boy8.Helper
{

    public static class BoyLinkHelper
    {
        //
        // Summary:
        //     Returns an anchor element (a element) that contains the virtual path of the
        //     specified action.
        //
        // Parameters:
        //   htmlHelper:
        //     The HTML helper instance that this method extends.
        //
        //   linkText:
        //     The inner text of the anchor element.
        //
        //   actionName:
        //     The name of the action.
        //
        //   controllerName:
        //     The name of the controller.
        //
        // Returns:
        //     An anchor element (a element).
        //
        // Exceptions:
        //   System.ArgumentException:
        //     The linkText parameter is null or empty.
        public static MvcHtmlString ActionLinkWithBadge(string messagebadge, string linkText, string actionName, string controllerName)
        {
            return new MvcHtmlString(string.Format("<a href='{0}'>{1}<span class='badge'>{2}</span></a>", System.Web.VirtualPathUtility.ToAbsolute("~/") + controllerName + "/" + actionName, linkText, messagebadge));
        }
    }
}