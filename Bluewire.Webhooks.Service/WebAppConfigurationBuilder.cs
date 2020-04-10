using System.Web.Http;

namespace Bluewire.Webhooks.Service
{
    public class WebAppConfigurationBuilder
    {
        public HttpConfiguration BuildConfiguration(DependencyResolver resolver)
        {
            var webApiConfiguration = new HttpConfiguration { DependencyResolver = resolver };

            webApiConfiguration.Filters.Add(new ExceptionLogFilter());
            webApiConfiguration.MapHttpAttributeRoutes();

            webApiConfiguration.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { },
                constraints: new { }
            );

            return webApiConfiguration;
        }
    }
}
