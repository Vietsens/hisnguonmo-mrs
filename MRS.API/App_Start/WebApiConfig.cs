using Inventec.Token.ResourceSystem;
using System.Web.Http;
using System.Web.Mvc;

namespace MRS.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}", //Copy ca dong nay de su dung duoc nhieu Get, GetView...
                defaults: new { id = RouteParameter.Optional }
            );
            config.Filters.Add(new ApiAuthenticationFilter()); //Comment neu ko co nhu cau xac thuc request
            AreaRegistration.RegisterAllAreas();
        }
    }
}
