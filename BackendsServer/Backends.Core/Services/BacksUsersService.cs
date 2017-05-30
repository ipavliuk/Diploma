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
		}


		public BacksUsers SignIn(string appId, string email, string userName, string pwd, out BacksErrorCodes error) // create user
		{
			error = BacksErrorCodes.Ok;
			try
			{
				var user = new BacksUsers()
				{
					AppId = appId,
					UserName = userName,
					Password = pwd,
					Email = email,
					CreatedAt = DateTime.UtcNow
				};

				if (_repo.FindDuplicates(userName).Result.Any())
				{
					error = BacksErrorCodes.DuplicateLogin;
					return null;
				}

				_repo.AddUser(appId, user);
				

			}
			catch (Exception e)
			{
				_log.Error("SignIn exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}

		public void Login()
		{

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
	}
}
