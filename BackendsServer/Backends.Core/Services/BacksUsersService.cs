using Backends.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Backends.Core.DataEngine;
using Backends.Core.Model.BackAdminData;
using BackendsCommon.Logging;
using BackendsCommon.Types;
using Backends.Core.Utils;
using AutoMapper;
using Backends.Core.Extension;

namespace Backends.Core.Services
{
	public class BacksUsersService
	{
		private readonly SchemaHandler _handler;

		private readonly IRepositoryAsync _repo;

		private ILog _log = new Log(typeof(BacksUsersService));

		public BacksUsersService(IRepositoryAsync repository)
		{
			_repo = repository;
			_handler = new SchemaHandler(_repo);
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<BacksUsers, UserDto>();
			});
		}


		public UserDto SignUp(string appId, string email, string userName, string pwd, out BacksErrorCodes error) // create user
		{
			error = BacksErrorCodes.Ok;
			try
			{
				var user = new BacksUsers()
				{
					AppId = appId,
					UserName = userName,
					Password = pwd.CreateMD5Hash(),
					Email = email,
					CreatedAt = DateTime.UtcNow
				};

				if (_repo.FindDuplicates(userName).Result.Any())
				{
					error = BacksErrorCodes.DuplicateLogin;
					return null;
				}

				_repo.AddUser(appId, user).Wait();
				if (user.Id == null)
				{
					error = BacksErrorCodes.SignUpError;
					return null;
				}

				//create session
				var session = new BacksSessions()
				{
					PUser = user.Id,
					CreatedAt = DateTime.UtcNow,
					ExpiresAt = DateTime.UtcNow.AddMinutes(30),
					AppId = appId,
				};
				_repo.AddSession(appId, session).Wait();

				if (session.Id == null)
				{
					error = BacksErrorCodes.SignUpError;
					return null;
				}
				var mappedUser = Mapper.Map<BacksUsers, UserDto>(user);
				mappedUser.SessionId = session.Id;

				return mappedUser;

			}
			catch (Exception e)
			{
				_log.Error("SignIn exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}

		public UserDto Login(string appId, string userName, string pwd, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{

				var user = _repo.Authenticate(appId, userName, pwd.CreateMD5Hash()).Result;
				if (user.Id == null)
				{
					error = BacksErrorCodes.AuthFailed;
					return null;
				}

				var session = new BacksSessions()
				{
					PUser = user.Id,
					CreatedAt = DateTime.UtcNow,
					ExpiresAt = DateTime.UtcNow.AddMinutes(30),
					AppId = appId,
				};
				_repo.AddSession(appId, session);

				if (session.Id == null)
				{
					error = BacksErrorCodes.SignUpError;
					return null;
				}
				var mappedUser = Mapper.Map<BacksUsers, UserDto>(user);
				mappedUser.SessionId = session.Id;

				return mappedUser;


			}
			catch (Exception e)
			{
				_log.Error("Login exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}
		//KillSession

		public void Logout(string appId, string userId, string sessionId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				if(ValidateSession(appId, userId, sessionId, out error) && error == BacksErrorCodes.Ok)
				{
					_repo.RemoveSession(appId, sessionId).Wait();
				}
					
			}
			catch (Exception e)
			{
				_log.Error("Logout exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
		}

		public void RemoveUser(string appId, string userId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				_repo.RemoveUser(appId, userId).Wait();
			
			}
			catch (Exception e)
			{
				_log.Error("RemoveUser exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
		}
	
		public bool ValidateSession(string appId, string userId, string sessionId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				BacksSessions session = _repo.GetSession(appId, sessionId).Result;
				return string.IsNullOrEmpty(userId) && session !=null ? session != null : session.PUser == userId;
			}
			catch (Exception e)
			{
				_log.Error("ValidateSession exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return false;
		}

		public UserDto GetUser(string appId, string userId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				BacksUsers user = _repo.GetUser(appId, userId).Result;
				if (user == null)
				{
					error = BacksErrorCodes.UserIsNotFound;
					return null;
				}

				var mappedUser = Mapper.Map<BacksUsers, UserDto>(user);

				return mappedUser;
			}
			catch (Exception e)
			{
				_log.Error("GetUser exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return null;
		}

		public void UpdateUser(string appId, string userId, string sessionToken, Dictionary<string, object> customFields, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				if (!ValidateSession(appId, userId, sessionToken, out error))
				{
					error = BacksErrorCodes.SessionIsNotFound;
					return;
				}

				//get User and  update
				BacksUsers user = _repo.GetUser(appId, userId).Result;
				if (user == null)
				{
					error = BacksErrorCodes.UserIsNotFound;
					return;
				}

				var updatedData = user.Data;
				foreach (var pair in customFields)
				{
					updatedData.CreateNewOrUpdateExisting(pair.Key, pair.Value);
				}


				_repo.UpdateUserData(appId, userId, updatedData).Wait();
			}
			catch (Exception e)
			{
				_log.Error("UpdateUser exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
		}
		public void QueryUserData()
		{

		}

		public void PasswrodReset(string appId, string userId, string pwd, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				_repo.UpdateUserPasswrod(appId, userId, pwd.CreateMD5Hash()).Wait();
				//send email
			}
			catch (Exception e)
			{
				_log.Error("PasswrodReset exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

		}

		public void UpdateSession(string appId, string sessionToken, Dictionary<string, object> customFields, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				BacksSessions session = _repo.GetSession(appId, sessionToken).Result;

				//if (ValidateSession(appId, null, sessionToken, out error))
				if(session == null)
				{
					error = BacksErrorCodes.SessionIsNotFound;
					return;
				}
				//Get Session and Update
				

				var updatedData = session.Data;
				foreach (var pair in customFields)
				{
					updatedData.CreateNewOrUpdateExisting(pair.Key, pair.Value);
				}

				_repo.UpdateSession(appId, sessionToken, updatedData).Wait();
			}
			catch (Exception e)
			{
				_log.Error("UpdateUser exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
		}
		public void RemoveSession(string appId, string sessionId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				_repo.RemoveSession(appId, sessionId).Wait();

			}
			catch (Exception e)
			{
				_log.Error("RemoveSession exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
		}
		
		public SessionDto GetSession(string appId, string sessionId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				var session = _repo.GetSession(appId, sessionId).Result;
				if (session == null)
				{
					error = BacksErrorCodes.SessionIsNotFound;
					return null;
				}


				var sessionDto = new SessionDto()
				{
					Id = session.Id,
					PUser = session.PUser,
					CreatedAt = session.CreatedAt,
					ExpiresAt = session.ExpiresAt,
					Data = session.Data
				};

				return sessionDto;
			}
			catch (Exception e)
			{
				_log.Error("GetSession exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}

		public List<SessionDto> GetUserSession(string appId, string userId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				var sessions = _repo.GetAllSessions(appId, userId).Result;
				if (sessions == null)
				{
					error = BacksErrorCodes.SessionIsNotFound;
					return null;
				}

				var sessionsDto = new List<SessionDto>();

				foreach (var item in sessions)
				{
					var sessionDto = new SessionDto()
					{
						Id = item.Id,
						PUser = item.PUser,
						CreatedAt = item.CreatedAt,
						ExpiresAt = item.ExpiresAt,
						Data = item.Data
					};

					sessionsDto.Add(sessionDto);
				}
				

				return sessionsDto;
			}
			catch (Exception e)
			{
				_log.Error("GetUserSession exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}
	}
}
