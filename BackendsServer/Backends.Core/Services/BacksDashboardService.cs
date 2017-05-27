using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backends.Core.DataEngine;
using Backends.Core.Model;
using Backends.Core.Model.BackAdminData;
using BackendsCommon.Logging;

namespace Backends.Core.Services
{
	public class BacksDashboardService
	{
		private readonly IRepositoryAsync _repo;
		private ILog _log = new Log(typeof(BacksDashboardService));

		public BacksDashboardService(IRepositoryAsync repo)
		{
			_repo = repo;
		}

		public IRepositoryAsync RepoInstance
		{
			get { return _repo; }
		}

		public string SignIn(string firstName, string lastName, string email, string screen, string pwd, out BacksErrorCodes error)
		{

			error = BacksErrorCodes.Ok;
			try
			{
				var acc = new Account()
				{
					FirstName = firstName,
					LastName = lastName,
					ScreenName = screen,
					Pasword = pwd,
					Email = email
				};

				if (_repo.FindDuplicates(screen).Result.Any())
				{
					error = BacksErrorCodes.DuplicateLogin;
					return null;
				}

				_repo.AddAccount(acc);

				return !string.IsNullOrEmpty(acc.Id) ? acc.Id : null;
			}
			catch (Exception e)
			{
				_log.Error("SignIn exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}
		public AccountDto Login(string login,string pwd)
		{
			//_repo.
			return null;
		}


		public ProjectDto AddNewProject()
		{
			//add new project 
			//add basic entities
			return null;
		}

		public List<ProjectDto> GetUserProjects()
		{
			return null;
		}

		
	}
}
