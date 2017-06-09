using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace BackendsServer.App_Start
{
	public class AddAuthorizationHeader : IOperationFilter
	{
		/// <summary>
		/// Adds an authorization header to the given operation in Swagger.
		/// </summary>
		/// <param name="operation">The Swashbuckle operation.</param>
		/// <param name="schemaRegistry">The Swashbuckle schema registry.</param>
		/// <param name="apiDescription">The Swashbuckle api description.</param>
		public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
		{
			if (operation == null) return;

			if (operation.parameters == null)
			{
				operation.parameters = new List<Parameter>();
			}

			var parameterApp = new Parameter
			{
				description = "This header param contains appId value",
				@in = "header",
				name = "X-Backends-AppId",
				required = true,
				type = "string"
			};

			if (apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
			{
				parameterApp.required = false;
			}

			operation.parameters.Add(parameterApp);

			var parameterApiKey = new Parameter
			{
				description = "The REST Api Key",
				@in = "header",
				name = "X-Backends-ApiKey",
				required = true,
				type = "string"
			};

			if (apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
			{
				parameterApiKey.required = false;
			}

			operation.parameters.Add(parameterApiKey);

			var parameterSession = new Parameter
			{
				description = "The session authorization token",
				@in = "header",
				name = "X-Backends-SessionToken",
				required = false,
				type = "string"
			};

			if (apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
			{
				parameterSession.required = false;
			}

			operation.parameters.Add(parameterSession);
		}
	}
}