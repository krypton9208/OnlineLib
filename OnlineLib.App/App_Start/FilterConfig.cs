using System.Runtime.InteropServices;
using System.Web.Mvc;

namespace OnlineLib.App
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            
        }

    }

   
}
