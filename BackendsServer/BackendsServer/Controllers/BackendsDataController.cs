using Backends.Core;
using BackendsCommon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackendsServer.Controllers
{
    public class BackendsDataController : BaseController
	{
		private ILog _log = new Log(typeof(BaseController));


		#region _BObjects
		[HttpPost]
		[Route("v1/entities/{entityName}")]
		public BaseRespones CreateEntity(string entityName)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if(errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in CreateEntity", ex);
			}
			return response;
		}

		[HttpGet]
		[Route("v1/entities/{entityName}/{entityId}")]
		public BaseRespones GetEntity(string entityName, string entityId)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in GetEntity", ex);
			}
			return response;
		}

		[HttpPut]
		[Route("v1/entities/{entityName}/{entityId}")]
		public BaseRespones UpdateEntity(string entityName, string entityId)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in UpdateEntity", ex);
			}
			return response;
		}

		[HttpGet]
		[Route("v1/entities")]
		public BaseRespones GetEntities()
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in GetEntities", ex);
			}
			return response;
		}

		[HttpDelete]
		[Route("v1/entities/{entityName}/{entityId}")]
		public BaseRespones DeleteEntity(string entityName, string entityId)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in DeleteEntity", ex);
			}
			return response;
		}
		#endregion

		#region _BUsers
		[HttpPost]
		[Route("v1/users")]
		//create user
		public BaseRespones SignIn()
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in SignIn", ex);
			}
			return response;
		}

		[HttpGet]
		[Route("v1/login")]
		public BaseRespones Login(string userName, string pwd)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in Login", ex);
			}
			return response;
		}
		[HttpPost]
		[Route("v1/logout")]
		public BaseRespones Logout(string userName, string pwd)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in Logout", ex);
			}
			return response;
		}

		[HttpGet]
		[Route("v1/users/{userId}")]
		public BaseRespones GetUser(string entityName, string entityId)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in GetUser", ex);
			}
			return response;
		}
		[HttpPut]
		[Route("v1/users/{userId}")]
		public BaseRespones UpdateUser(string entityName, string entityId)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in UpdateUser", ex);
			}
			return response;
		}

		[HttpGet]
		[Route("v1/users")]
		public BaseRespones GetUsers()
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in GetUsers", ex);
			}
			return response;
		}

		[HttpDelete]
		[Route("v1/users/{userId}")]
		public BaseRespones DeleteUser()
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in DeleteEntity", ex);
			}
			return response;
		}
		#endregion

		#region _bSession
		[HttpGet]
		[Route("v1/sessions/{sessionId}")]
		public BaseRespones GetSession(string sessionId)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in GetSession", ex);
			}
			return response;
		}

		[HttpPut]
		[Route("v1/sessions/{sessionId}")]
		public BaseRespones UpdateSession(string sessionId)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in UpdateSession", ex);
			}
			return response;
		}

		[HttpGet]
		[Route("v1/sessions")]
		public BaseRespones GetSessions()
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in GetSessions", ex);
			}
			return response;
		}

		[HttpDelete]
		[Route("v1/sessions/{sessionId}")]
		public BaseRespones DeleteSession(string sessionId)
		{
			var response = new BaseRespones();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{

				}

				response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in DeleteEntity", ex);
			}
			return response;
		}
		#endregion
	}
}
