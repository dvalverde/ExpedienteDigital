using System.Web;
using System.Web.Mvc;

namespace ExpDigital
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filters.Verificar());
        }
    }
}
