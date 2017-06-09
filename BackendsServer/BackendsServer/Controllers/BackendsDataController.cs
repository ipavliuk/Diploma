using Backends.Core;
using BackendsCommon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Backends.Core.Model.BackAdminData;

namespace BackendsServer.Controllers
{
    public class BackendsDataController : BaseController
	{
		private ILog _log = new Log(typeof(BaseController));


		#region _BObjects
		[HttpPost]
		[Route("v1/entities/{entityName}")]
		public async Task<ObjectsDto> CreateEntity(string entityName, Dictionary<string, object> data)
		{
			var response = new ObjectsDto();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if(errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.DataService;
					 response = await service.CreateEntity(AppId, entityName, data);
				}
				//response.ErrorId = (int)errorCode;
			}
			catch (Exception ex)
			{
				_log.Error("Exception in CreateEntity", ex);
			}
			return response;
		}

		[HttpGet]
		[Route("v1/entities/{entityName}/{entityId}")]
		public async Task<ObjectsDto> GetEntity(string entityName, string entityId)
		{
			var response = new ObjectsDto();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.DataService;
					response = await service.GetEntity(AppId, entityName, entityId);
				}

				//response.Error = string.IsNullOrEmpty(response.Id)? BacksErrorCodes.SystemError : BacksErrorCodes.Ok ;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in GetEntity", ex);
			}
			return response;
		}

		[HttpPut]
		[Route("v1/entities/{entityName}/{entityId}")]
		public async Task<ObjectsDto> UpdateEntity(string entityName, string entityId, Dictionary<string, object> data)
		{
			var response = new ObjectsDto();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.DataService;
					response = await service.UpdateEntity(AppId, entityName, entityId, data);
				}

			}
			catch (Exception ex)
			{
				_log.Error("Exception in UpdateEntity", ex);
			}
			return response;
		}

		[HttpGet]
		[Route("v1/entities/{entityName}")]
		public async Task<HttpResponseMessage> GetEntities(string entityName/*, string condition*/)
		{
			var response = new List<ObjectsDto>();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.DataService;

					var tuple  = await service.GetEntities(AppId, entityName);
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}

				//response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in GetEntities", ex);
			}

			return FormResponse(errorCode, response);
		}

		[HttpDelete]
		[Route("v1/entities/{entityName}/{entityId}")]
		public async Task<ObjectsDto> DeleteEntity(string entityName, string entityId)
		{
			var response = new ObjectsDto();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.DataService;
					response = await service.RemoveEntity(AppId, entityName, entityId);
				}

				//response.ErrorId = (int)errorCode;

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
		public async Task<UserDto> SignIn([FromBody] UserSignUpRequest request/*string username, string password, string email*/)
		{
			var response = new UserDto();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					response = await service.SignUp(AppId, request.email, request.username, request.password);
				}

				//response.ErrorId = (int)errorCode;

			}
			catch (Exception ex)
			{
				_log.Error("Exception in SignIn", ex);
			}
			return response;
		}

		[HttpGet]
		[Route("v1/login")]
		public async Task<UserDto> Login(string userName, string pwd)
		{
			var response = new UserDto();
			try
			{
				BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					response = await service.Login(AppId, userName, pwd);
				}

				//response.ErrorId = (int)errorCode;

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

		//[HttpGet]
		//[Route("v1/users")]
		//public BaseRespones GetUsers()
		//{
		//	var response = new BaseRespones();
		//	try
		//	{
		//		BacksErrorCodes errorCode = ValidateAppCredentialHeaders();
		//		if (errorCode == BacksErrorCodes.Ok)
		//		{

		//		}

		//		response.ErrorId = (int)errorCode;

		//	}
		//	catch (Exception ex)
		//	{
		//		_log.Error("Exception in GetUsers", ex);
		//	}
		//	return response;
		//}

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

        #region _schema
        #endregion

        #region _roles
        #endregion
    }
}
