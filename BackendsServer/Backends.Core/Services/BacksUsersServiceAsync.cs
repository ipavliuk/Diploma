using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backends.Core.DataEngine;
using Backends.Core.Extension;
using Backends.Core.Model.BackAdminData;
using Backends.Core.Utils;
using BackendsCommon.Logging;
using BackendsCommon.Types;

namespace Backends.Core.Services
{
	public class BacksUsersServiceAsync
	{
		private readonly SchemaHandler _handler;

		private readonly IRepositoryAsync _repo;

		private ILog _log = new Log(typeof(BacksUsersService));

		public BacksUsersServiceAsync(IRepositoryAsync repository)
		{
			_repo = repository;
			_handler = new SchemaHandler(_repo);
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<BacksUsers, UserDto>();
			});
		}


		public async Task<Tuple<BacksErrorCodes, UserDto>> SignUp(string appId, string email, string userName, string pwd/*, out BacksErrorCodes error*/) // create user
		{
			var error = BacksErrorCodes.Ok;
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

				var users = await _repo.FindDuplicates(userName).ConfigureAwait(false);
				if (users.Any())
				{
					return new Tuple<BacksErrorCodes, UserDto>(BacksErrorCodes.DuplicateLogin, null);
				}

				await  _repo.AddUser(appId, user).ConfigureAwait(false);
				if (user.Id == null)
				{
					return new Tuple<BacksErrorCodes, UserDto>(BacksErrorCodes.SignUpError, null);
				}

				//create session
				var session = new BacksSessions()
				{
					PUser = user.Id,
					CreatedAt = DateTime.UtcNow,
					ExpiresAt = DateTime.UtcNow.AddMinutes(30),
					AppId = appId,
				};

				await _repo.AddSession(appId, session).ConfigureAwait(false);

				if (session.Id == null)
				{
					return new Tuple<BacksErrorCodes, UserDto>(BacksErrorCodes.SignUpError, null);
				}
				//var mappedUser = Mapper.Map<BacksUsers, UserDto>(user);
				var mappedUser = new UserDto()
				{
					Id = user.Id,
					SessionId = session.Id,
					CreatedAt = user.CreatedAt.Value,
				};

				//mappedUser.SessionId = session.Id;

				return new Tuple<BacksErrorCodes, UserDto>(error, mappedUser);

			}
			catch (Exception e)
			{
				_log.Error("SignIn exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return new Tuple<BacksErrorCodes, UserDto>(error, null);
		}

		public async Task<Tuple<BacksErrorCodes, UserDto>> Login(string appId, string userName, string pwd/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				var user = await _repo.Authenticate(appId, userName, pwd.CreateMD5Hash()).ConfigureAwait(false);
				if (user.Id == null)
				{
					new Tuple<BacksErrorCodes, UserDto>(BacksErrorCodes.AuthFailed, null);
				}

				var session = new BacksSessions()
				{
					PUser = user.Id,
					CreatedAt = DateTime.UtcNow,
					ExpiresAt = DateTime.UtcNow.AddMinutes(30),
					AppId = appId,
				};
				await _repo.AddSession(appId, session).ConfigureAwait(false);

				if (session.Id == null)
				{
					new Tuple<BacksErrorCodes, UserDto>(BacksErrorCodes.SignUpError, null);

				}
				/*var mappedUser = Mapper.Map<BacksUsers, UserDto>(user);
				mappedUser.SessionId = session.Id;*/
				var mappedUser = new UserDto()
				{
					Id = user.Id,
					UserName = user.UserName,
					SessionId = session.Id,
					CreatedAt = user.CreatedAt.Value,
					Email = user.Email
				};

				return new Tuple<BacksErrorCodes, UserDto>(error, mappedUser);

			}
			catch (Exception e)
			{
				_log.Error("Login exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return new Tuple<BacksErrorCodes, UserDto>(error, null);
		}
		//KillSession

		public async Task<BacksErrorCodes> Logout(string appId, string userId, string sessionId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				bool result = await ValidateSession(appId, userId, sessionId);

				if (!result)
				{
					return BacksErrorCodes.SessionIsNotFound;
				}

				await _repo.RemoveSession(appId, sessionId).ConfigureAwait(false);

			}
			catch (Exception e)
			{
				_log.Error("Logout exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return error;
		}

		public async Task<BacksErrorCodes> RemoveUser(string appId, string userId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				await _repo.RemoveUser(appId, userId).ConfigureAwait(false);

			}
			catch (Exception e)
			{
				_log.Error("RemoveUser exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return error;
		}

		public async Task<bool> ValidateSession(string appId, string userId, string sessionId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				BacksSessions session = await _repo.GetSession(appId, sessionId).ConfigureAwait(false);
				return string.IsNullOrEmpty(userId) && session != null ? session != null : session.PUser == userId;
			}
			catch (Exception e)
			{
				_log.Error("ValidateSession exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return false;
		}

		public async Task<Tuple<BacksErrorCodes, UserDto>> GetUser(string appId, string userId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				BacksUsers user = await  _repo.GetUser(appId, userId).ConfigureAwait(false);
				if (user == null)
				{
					return new Tuple<BacksErrorCodes, UserDto>(BacksErrorCodes.UserIsNotFound, null);
				}

				var mappedUser = Mapper.Map<BacksUsers, UserDto>(user);
				/*var mappedUser = new UserDto()
				{
					Id = user.Id,
					UserName = user.UserName,
					Data =  user.Data,
					Email = user.Email,
					CreatedAt = user.CreatedAt.Value,
					UpdatedAt = user.UpdatedAt.Value
				};*/


				return new Tuple<BacksErrorCodes, UserDto>(error, mappedUser);
			}
			catch (Exception e)
			{
				_log.Error("GetUser exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return new Tuple<BacksErrorCodes, UserDto>(error, null);
		}

		public async Task<Tuple<BacksErrorCodes, UserDto>> UpdateUser(string appId, string userId, string sessionToken, Dictionary<string, object> customFields/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				bool result = await ValidateSession(appId, userId, sessionToken);
				if (!result)
				{
					return new Tuple<BacksErrorCodes, UserDto>(BacksErrorCodes.SessionIsNotFound, null);
				}

				//get User and  update
				BacksUsers user = await _repo.GetUser(appId, userId).ConfigureAwait(false);
				if (user == null)
				{
					return new Tuple<BacksErrorCodes, UserDto>(BacksErrorCodes.UserIsNotFound, null);
				}

				var updatedData = user.Data;
				foreach (var pair in customFields)
				{
					updatedData.CreateNewOrUpdateExisting(pair.Key, pair.Value);
				}


				await _repo.UpdateUserData(appId, userId, updatedData).ConfigureAwait(false);
				var userDto = new UserDto() { UpdatedAt = DateTime.UtcNow };

				return new Tuple<BacksErrorCodes, UserDto>(error, userDto);
			}
			catch (Exception e)
			{
				_log.Error("UpdateUser exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return new Tuple<BacksErrorCodes, UserDto>(error, null);
		}
		public async Task<Tuple<BacksErrorCodes, List<UserDto>>> GetUsers(string appId, string query)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				//get User and  update
				List<BacksUsers> users = await _repo.GetUsers(appId, query).ConfigureAwait(false);
				if (users == null)
				{
					return new Tuple<BacksErrorCodes, List<UserDto>>(BacksErrorCodes.UserIsNotFound, null);
				}

				//var mappedUsers = Mapper.Map<List<BacksUsers>, List<UserDto>>(users);

				var mappedUsers = new List<UserDto>();

				foreach (var user in users)
				{
					//var mappedUser = Mapper.Map<BacksUsers, UserDto>(user);
					var mappedUser = new UserDto()
					{
						Id = user.Id,
						UserName = user.UserName,
						Data = user.Data,
						Email = user.Email,
						CreatedAt = user.CreatedAt.Value,
						UpdatedAt = user.UpdatedAt.HasValue ? user.UpdatedAt : null
					};

					mappedUsers.Add(mappedUser);
				}

				return new Tuple<BacksErrorCodes, List<UserDto>>(error, mappedUsers);
			}
			catch (Exception e)
			{
				_log.Error("UpdateUser exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return new Tuple<BacksErrorCodes, List<UserDto>>(error, null);
		}

		public async Task<BacksErrorCodes> PasswrodReset(string appId, string userId, string pwd/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				await _repo.UpdateUserPasswrod(appId, userId, pwd.CreateMD5Hash()).ConfigureAwait(false);
				//send email
			}
			catch (Exception e)
			{
				_log.Error("PasswrodReset exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return error;
		}

		public async Task<Tuple<BacksErrorCodes, SessionDto>> UpdateSession(string appId, string sessionToken, Dictionary<string, object> customFields/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				BacksSessions session = await _repo.GetSession(appId, sessionToken).ConfigureAwait(false);

				//if (ValidateSession(appId, null, sessionToken, out error))
				if (session == null)
				{
					return new Tuple<BacksErrorCodes, SessionDto>(BacksErrorCodes.SessionIsNotFound, null);
				}
				//Get Session and Update


				var updatedData = session.Data;
				foreach (var pair in customFields)
				{
					updatedData.CreateNewOrUpdateExisting(pair.Key, pair.Value);
				}

				await _repo.UpdateSession(appId, sessionToken, updatedData).ConfigureAwait(false);

				var sessionDto = new SessionDto() { UpdatedAt = DateTime.UtcNow };

				return new Tuple<BacksErrorCodes, SessionDto>(error, sessionDto);
			}
			catch (Exception e)
			{
				_log.Error("UpdateUser exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return new Tuple<BacksErrorCodes, SessionDto>(error, null);
		}
		public async Task<BacksErrorCodes> RemoveSession(string appId, string sessionId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				await _repo.RemoveSession(appId, sessionId).ConfigureAwait(false);

			}
			catch (Exception e)
			{
				_log.Error("RemoveSession exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return error;
		}

		public async Task<Tuple<BacksErrorCodes, SessionDto>> GetSession(string appId, string sessionId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				var session = await _repo.GetSession(appId, sessionId).ConfigureAwait(false);
				if (session == null)
				{
					return new Tuple<BacksErrorCodes, SessionDto>(BacksErrorCodes.SessionIsNotFound, null);
				}
				
				var sessionDto = new SessionDto()
				{
					Id = session.Id,
					PUser = session.PUser,
					CreatedAt = session.CreatedAt,
					ExpiresAt = session.ExpiresAt,
					Data = session.Data
				};

				return new Tuple<BacksErrorCodes, SessionDto>(error, sessionDto);

			}
			catch (Exception e)
			{
				_log.Error("GetSession exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return new Tuple<BacksErrorCodes, SessionDto>(error, null);
		}


		public async  Task<Tuple<BacksErrorCodes, List<SessionDto>>> GetUserSessions(string appId, string sessionId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				var session = await _repo.GetSession(appId, sessionId).ConfigureAwait(false);
				if (session == null)
				{
					return new Tuple<BacksErrorCodes, List<SessionDto>> (BacksErrorCodes.SessionIsNotFound, null);
				}


				var user = await _repo.GetUser(appId, session.PUser);
				if (user == null)
				{
					return new Tuple<BacksErrorCodes, List<SessionDto>>(BacksErrorCodes.EntityNotFound, null);
				}

				var sessions = await _repo.GetAllSessions(appId, user.Id).ConfigureAwait(false);
				if (sessions == null)
				{
					return new Tuple<BacksErrorCodes, List<SessionDto>> (BacksErrorCodes.SessionIsNotFound, null);
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


				return new Tuple<BacksErrorCodes, List<SessionDto>>(error, sessionsDto);
			}
			catch (Exception e)
			{
				_log.Error("GetUserSession exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}

		public async Task<Tuple<BacksErrorCodes, List<SessionDto>>> GetSessions(string appId)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				var sessions = await _repo.GetSessions(appId).ConfigureAwait(false);
				if (sessions == null)
				{
					return new Tuple<BacksErrorCodes, List<SessionDto>>(BacksErrorCodes.SessionIsNotFound, null);
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


				return new Tuple<BacksErrorCodes, List<SessionDto>>(error, sessionsDto);
			}
			catch (Exception e)
			{
				_log.Error("GetSessions exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}
	}


}

