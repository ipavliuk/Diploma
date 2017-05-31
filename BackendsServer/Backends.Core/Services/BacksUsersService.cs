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

		public void Logout()
		{

		}

		public void GetUser()
		{
			
		}

		public void UpdateUser()
		{
			
		}

		public void QueryUserData()
		{
			
		}

		public void PasswrodReset()
		{
			
		}

		public SessionDto GetSession(string appId, string sessionId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{

				var session = _repo.GetSession(appId, sessionId).Result;
				if (session.Id == null)
				{
					error = BacksErrorCodes.AuthFailed;
					return null;
				}


				var sessionDto = new SessionDto()
				{
					Token = session.Id,
					PUser = session.PUser,
					CreatedAt = session.CreatedAt,
					ExpiresAt = session.ExpiresAt
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
	}
}
