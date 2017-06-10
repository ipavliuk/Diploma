using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using BackendsCommon.Logging;
using BackendsServer.HttpHelper;
using System.Net;
using System.Net.Http;
using Backends.Core;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace BackendsServer.Controllers
{
	public class BaseController : ApiController
	{
		protected static string AppId_Header_Name = "X-Backends-AppId";
		protected static string ApiKey_Header_Name = "X-Backends-ApiKey";
		protected static string UserId_Header_Name = "X-Backends-UserId";
		protected static string SessionToken_Header_Name = "X-Backends-SessionToken";
		protected static string MasterKey_Header_Name = "X-Backends-MasterKey";
		private const string XForwardedFor = "x-forwarded-for";
		private ILog _log = new Log(typeof(BaseController));

		protected string UserId
		{
			get
			{
				string userId = Request.GetValueFromHeaders(UserId_Header_Name);
				if (string.IsNullOrEmpty(userId))
				{
					userId = Request.GetValueFromURL(UserId_Header_Name);
				}
				return userId;
			}
		}


		protected string ClientIp
		{
			get
			{
				var context = HttpContext.Current;
				if (context == null) return null;

				// x-forwarded-for header
				_log.InfoFormat("Got Headers:[{0}]", string.Join(",", context.Request.Headers.AllKeys));
				var ips = context.Request.Headers.Get(XForwardedFor);

				IPAddress remoteAddress;
				if ((ips != null))
				{
					_log.InfoFormat("ClientIp : ips from x-forwarded-for header = {0}", ips);
					var ip = ips.Split(',').First();

					if (IPAddress.TryParse(ip, out remoteAddress))
					{
						_log.InfoFormat("ClientIp : ip = {0},  remoteAddress = {1}", ip, remoteAddress);
						return remoteAddress.ToString();
					}

				}
				// last chance: use default address
				return context.Request.UserHostAddress;
			}
		}

		protected string SessionToken
		{
			get
			{
				string userToken = null;
				userToken = Request.GetValueFromHeaders(SessionToken_Header_Name);
				if (string.IsNullOrEmpty(userToken))
				{
					userToken = Request.GetValueFromURL(SessionToken_Header_Name);
				}
				return userToken;
			}
		}

		protected bool ValidateReceivedUserCredentials()
		{
			return UserId != string.Empty && SessionToken != string.Empty;
		}

		protected string AppId
		{
			get
			{
				string appId = Request.GetValueFromHeaders(AppId_Header_Name);
				if (string.IsNullOrEmpty(appId))
				{
					appId = Request.GetValueFromURL(AppId_Header_Name);
				}
				return appId;
			}
		}

		protected string ApiKey
		{
			get
			{
				string apiKey = Request.GetValueFromHeaders(ApiKey_Header_Name);
				if (string.IsNullOrEmpty(apiKey))
				{
					apiKey = Request.GetValueFromURL(ApiKey_Header_Name);
				}
				return apiKey;
			}
		}

		protected string MasterKey
		{
			get
			{
				string masterKey = Request.GetValueFromHeaders(MasterKey_Header_Name);
				if (string.IsNullOrEmpty(masterKey))
				{
					masterKey = Request.GetValueFromURL(ApiKey_Header_Name);
				}
				return masterKey;
			}
		}
		protected bool ValidateAppCreadentials()
		{
			return !string.IsNullOrEmpty(AppId) && !string.IsNullOrEmpty(ApiKey);
		} 

		protected HttpResponseMessage FormResponse<T>(T successResponseValue, int errorCode, string errorDesc = null)
		{
			if (errorCode != (int)BacksErrorCodes.Ok)
			{
				return Request.CreateResponse(HttpStatusCode.OK, new Error { Id = errorCode, Desc = errorDesc });
			}

			return Request.CreateResponse(HttpStatusCode.OK, successResponseValue);
		}

		protected HttpResponseMessage FormResponse<T>(BacksErrorCodes errorCode, T successResponseValue, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			if (errorCode != BacksErrorCodes.Ok)
			{
				return FormErrorResponse(errorCode);
			}

			return Request.CreateResponse(statusCode, successResponseValue);
		}

		
		protected HttpResponseMessage FormResponse(BacksErrorCodes errorCode)
		{
			if (errorCode != BacksErrorCodes.Ok)
			{
				return FormErrorResponse(errorCode);
			}

			return Request.CreateResponse(HttpStatusCode.OK);
		}

		private readonly ReadOnlyDictionary<BacksErrorCodes, string> _backsErrorsMapping = new ReadOnlyDictionary
			<BacksErrorCodes, string>(
			new Dictionary<BacksErrorCodes, string>()
			{
				{ BacksErrorCodes.Ok, "Ok"},
				{ BacksErrorCodes.SystemError, " System Error"},
				{BacksErrorCodes.AuthFailed, " Authentication Failed"},
				{BacksErrorCodes.OperationIsNotSupported, "Operation is not supported" },
				{ BacksErrorCodes.InvalidCredentials, "Invalid creadentials"},
				{ BacksErrorCodes.SessionIsNotFound,"Session is not found"},
				{BacksErrorCodes.NotAllMandatFields, "Missed mandatory fields"},
				{BacksErrorCodes.DuplicateLogin, "The record with the same login found"},
				{BacksErrorCodes.ProjectCreationFailed, "Project could not be created"},
				{BacksErrorCodes.SignUpError, "Signup error"},
				{BacksErrorCodes.UserIsNotFound, "User is not found"},
				{BacksErrorCodes.EntityNotFound,"Entity is notFound"}
				
			});

		protected HttpResponseMessage FormErrorResponse(BacksErrorCodes errorCode)
		{
			return Request.CreateResponse(HttpStatusCode.OK, new BaseRespones() { ErrorId = (int)errorCode , ErrorDesc = _backsErrorsMapping[errorCode] });
		}

		protected NameValueCollection GetQueryParameter()
		{
			return HttpUtility.ParseQueryString(Request.RequestUri.Query);
		}

		protected BacksErrorCodes ValidateAppCredentialHeaders()
		{

			BacksErrorCodes status = BacksErrorCodes.Ok;

			if (!ValidateAppCreadentials())
			{
				status = BacksErrorCodes.InvalidCredentials;
			}
			return status;
		}

		
		protected T ParseJToken<T>(JToken value)
		{
			T result = default(T);
			if (value != null)
			{
				result = value.Value<T>();
			}
			return result;
		}

		/// <summary>
		/// The error.
		/// </summary>
		[DataContract(Namespace = "")]
		private class Error
		{
			/// <summary>
			/// Gets or sets the id.
			/// </summary>
			[DataMember(Name = "Error")]
			public int Id { get; set; }

			/// <summary>
			/// Gets or sets the description
			/// </summary>
			[DataMember]
			public string Desc { get; set; }
		}
	}
}