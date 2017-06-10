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
		/// <summary>
		/// Create new entity
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("1/entities/{entityName}")]
		public async Task<HttpResponseMessage> CreateEntity(string entityName, Dictionary<string, object> data)
		{
			var response = new ObjectsDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.DataService;
					var tuple  = await service.CreateEntity(AppId, entityName, data);

					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}
			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in CreateEntity", ex);
			}
			return FormResponse(errorCode, response);
		}

		/// <summary>
		/// Get entity by Id
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="entityId"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("1/entities/{entityName}/{entityId}")]
		public async Task<HttpResponseMessage> GetEntity(string entityName, string entityId)
		{
			var response = new ObjectsDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.DataService;
					var tuple = await service.GetEntity(AppId, entityName, entityId);
					
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}
			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in GetEntity", ex);
			}
			return FormResponse(errorCode, response); ;
		}
		/// <summary>
		/// Update entity
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="entityId"></param>
		/// <param name="data">user data JSON object</param>
		/// <returns></returns>
		[HttpPut]
		[Route("1/entities/{entityName}/{entityId}")]
		public async Task<HttpResponseMessage> UpdateEntity(string entityName, string entityId, Dictionary<string, object> data)
		{
			var response = new ObjectsDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.DataService;
					var tuple = await service.UpdateEntity(AppId, entityName, entityId, data);
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}

			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in UpdateEntity", ex);
			}

			return FormResponse(errorCode, response);
		}
		/// <summary>
		/// Get entities by entityName/filter
		/// </summary>
		/// <param name="entityName"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("1/entities/{entityName}")]
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
			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in GetEntities", ex);
			}

			return FormResponse(errorCode, response);
		}
		/// <summary>
		/// Delete entity
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="entityId"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("1/entities/{entityName}/{entityId}")]
		public async Task<HttpResponseMessage> DeleteEntity(string entityName, string entityId)
		{
			var response = new ObjectsDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.DataService;
					var tuple  = await service.RemoveEntity(AppId, entityName, entityId);
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}
				
			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in DeleteEntity", ex);
			}
			return FormResponse(errorCode, response);
		}
	
		#endregion

		#region _BUsers
		/// <summary>
		/// Create user => SignIn
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("1/users")]
		//create user
		public async Task<HttpResponseMessage> SignIn([FromBody] UserSignUpRequest request/*string username, string password, string email*/)
		{
			var response = new UserDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					var tuple = await service.SignUp(AppId, request.email, request.username, request.password);
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
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in SignIn", ex);
			}
			return FormResponse(errorCode, response);
		}

		/// <summary>
		/// User Login into app
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="pwd"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("1/login")]
		public async Task<HttpResponseMessage> Login(string userName, string pwd)
		{
			var response = new UserDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					var tuple = await service.Login(AppId, userName, pwd);
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}
				
			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in Login", ex);
			}
			return FormResponse(errorCode, response);
		}
		/// <summary>
		/// Logout user from app
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="pwd"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("1/logout")]
		public async Task<HttpResponseMessage> Logout()
		{
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					errorCode = await service.Logout(AppId, null, SessionToken);
				}

			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in Logout", ex);
			}
			return FormResponse(errorCode);
		}

		/// <summary>
		/// Get user By Id
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="entityId"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("1/users/{userId}")]
		public async Task<HttpResponseMessage> GetUser(string userId)
		{
			var response = new UserDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;

					var tuple = await service.GetUser(AppId, userId);
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}

			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in GetUser", ex);
			}
			return FormResponse(errorCode, response);
		}

		/// <summary>
		/// Update user data
		/// </summary>
		/// <param name="entityName"></param>
		/// <param name="entityId"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("1/users/{userId}")]
		public async Task<HttpResponseMessage> UpdateUser(string userId, Dictionary<string, object> data)
		{
			var response = new UserDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;

					var tuple = await service.UpdateUser(AppId, userId, SessionToken, data);
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}
			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in UpdateUser", ex);
			}
			return FormResponse(errorCode, response);
		}

		//[HttpGet]
		//[Route("1/users")]
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

		/// <summary>
		/// Remove user by Id
		/// </summary>
		/// <returns></returns>
		[HttpDelete]
		[Route("1/users/{userId}")]
		public async Task<HttpResponseMessage> DeleteUser(string userId)
		{
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					errorCode = await service.RemoveUser(AppId, userId);
				}
				
			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in DeleteEntity", ex);
			}
			return FormResponse(errorCode);
		}
		/// <summary>
		/// Reset userPassword
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="newPwd"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("1/requestPasswordReset")]
		public async Task<HttpResponseMessage> PasswordReset(string userId, string password)
		{
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					errorCode = await service.PasswrodReset(AppId, userId, password);
				}

			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in Logout", ex);
			}
			return FormResponse(errorCode);
		}
		#endregion

		#region _bSession
		/// <summary>
		/// Get session by Id
		/// </summary>
		/// <param name="sessionId"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("1/sessions/{sessionId}")]
		public async Task<HttpResponseMessage> GetSession(string sessionId)
		{
			var response = new SessionDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					var tuple = await service.GetSession(AppId, sessionId);
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}


			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in GetSession", ex);
			}
			return FormResponse(errorCode, response);
		}
		/// <summary>
		/// Update user session
		/// </summary>
		/// <param name="sessionId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("1/sessions/{sessionId}")]
		public async Task<HttpResponseMessage> UpdateSession(string sessionId, Dictionary<string, object> data)
		{
			var response = new SessionDto();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					var tuple = await service.UpdateSession(AppId, sessionId, data);
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}

			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in UpdateSession", ex);
			}
			return FormResponse(errorCode, response);
		}
		/// <summary>
		/// Get all user sessions
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("1/sessions")]
		public async Task<HttpResponseMessage> GetSessions()
		{
			var response = new List<SessionDto>();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					var tuple = await service.GetUserSessions(AppId, SessionToken);
					if (tuple == null)
					{
						errorCode = BacksErrorCodes.SystemError;
						return FormResponse(errorCode, response);
					}

					errorCode = tuple.Item1;
					response = tuple.Item2;
				}
			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in GetSessions", ex);
			}
			return FormResponse(errorCode, response);
		}
		/// <summary>
		/// Delete user session
		/// </summary>
		/// <param name="sessionId"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("1/sessions/{sessionId}")]
		public async Task<HttpResponseMessage> DeleteSession(string sessionId)
		{
			var response = new BaseRespones();
			BacksErrorCodes errorCode = BacksErrorCodes.Ok;
			try
			{
				errorCode = ValidateAppCredentialHeaders();
				if (errorCode == BacksErrorCodes.Ok)
				{
					var service = BackendsServerManager.Instance.UserService;
					errorCode = await service.RemoveSession(AppId, SessionToken);
					
				}
			}
			catch (Exception ex)
			{
				errorCode = BacksErrorCodes.SystemError;
				_log.Error("Exception in DeleteEntity", ex);
			}
			return FormResponse(errorCode, response);
		}
        #endregion

        #region _schema
        #endregion

        #region _roles
        #endregion
    }
}
