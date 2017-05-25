using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace BackendsServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			//CORS settings
			//var cors = new EnableCorsAttribute("*", "*", "*");
			//config.EnableCors(cors);

			var jsonFormatter = config.Formatters.JsonFormatter;
			config.Formatters.Clear();
			config.Formatters.Add(jsonFormatter);

			// Web API routes
			//config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

			//remove default routes
			config.MapHttpAttributeRoutes();

			 config.Routes.MapHttpRoute(
				 name: "DefaultApi",
				 routeTemplate: "api/{controller}/{id}",
				 defaults: new { id = RouteParameter.Optional }
			 );
		}
		public class CustomDirectRouteProvider : DefaultDirectRouteProvider
		{
			protected override IReadOnlyList<IDirectRouteFactory>
				GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
			{
				// inherit route attributes decorated on base class controller's actions
				return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>
					(inherit: true);
			}
		}
	}
}
