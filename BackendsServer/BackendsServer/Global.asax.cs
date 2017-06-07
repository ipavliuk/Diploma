using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Backends.Core;

namespace BackendsServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

			BackendsServerManager.Instance.Start(ConfigurationManager.AppSettings["connectionStringMongoDB"],
								ConfigurationManager.AppSettings["database"]);

		}
    }
}
