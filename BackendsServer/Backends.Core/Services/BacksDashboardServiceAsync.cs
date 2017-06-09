using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backends.Core.DataEngine;
using Backends.Core.Model;
using Backends.Core.Model.BackAdminData;
using BackendsCommon.Logging;

namespace Backends.Core.Services
{
	public class BacksDashboardServiceAsync
	{
		private readonly IRepositoryAsync _repo;
		private readonly SchemaHandler _handler;
		private ILog _log = new Log(typeof(BacksDashboardService));

		public BacksDashboardServiceAsync(IRepositoryAsync repo)
		{
			_repo = repo;
			_handler = new SchemaHandler(_repo);

			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Account, AccountDto>();
				cfg.CreateMap<Project, ProjectDto>();
			});
		}

		public IRepositoryAsync RepoInstance
		{
			get { return _repo; }
		}

		public async Task<AccountDto> SignIn(string firstName, string lastName, string email, string screen, string pwd/*, out BacksErrorCodes error*/)
		{

			var error = BacksErrorCodes.Ok;
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
				var accs = await _repo.FindDuplicates(screen).ConfigureAwait(false);
				if (accs.Any())
				{
					error = BacksErrorCodes.DuplicateLogin;
					return new AccountDto() {Error = BacksErrorCodes.DuplicateLogin};
				}

				await _repo.AddAccount(acc).ConfigureAwait(false);

				return new AccountDto()
				{
					Id = !string.IsNullOrEmpty(acc.Id) ? acc.Id : null,
					Error = string.IsNullOrEmpty(acc.Id) ? BacksErrorCodes.SignUpError : BacksErrorCodes.Ok
				};
				//!string.IsNullOrEmpty(acc.Id) ? acc.Id : null;
			}
			catch (Exception e)
			{
				_log.Error("SignIn exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}
		public async Task<AccountDto> Login(string login, string pwd/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				Account acc = await _repo.Login(login, pwd).ConfigureAwait(false);

				if (acc == null)
				{
					error = BacksErrorCodes.AuthFailed;
					return null;
				}

				var projects = (await _repo.GetAccountProjects(acc.Id).ConfigureAwait(false)).ToList();

				var mappedAccount = Mapper.Map<Account, AccountDto>(acc);
				mappedAccount.Projects = await GetUserProjects(acc.Id/*, out error*/).ConfigureAwait(false);
				//var mappedProjetcs = Mapper.Map<List<Project>, List<ProjectDto>>(projects);
				//mappedAccount.Projects = mappedProjetcs;

				return mappedAccount;
			}
			catch (Exception e)
			{
				_log.Error("LogIn exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return null;
		}


		public async Task<ProjectDto> AddNewProject(string name, string acc_id/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				//add new project 
				var project = new Project()
				{
					Name = name,
					AppId = Guid.NewGuid().ToString("N"),
					ApiKeyAccess = Guid.NewGuid().ToString("N"),
					MasterKeyAccess = Guid.NewGuid().ToString("N"),
					CreatedAt = DateTime.UtcNow,
					P_AccountId = acc_id
				};

				await _repo.AddProject(project).ConfigureAwait(false);
				if (project == null)
				{
					error = BacksErrorCodes.ProjectCreationFailed;
				}
				//Create basic entities schema: _User, _Sessions, 
				var schema = SchemaHandler.CreateDefaultSchema(project.Id);

				await _repo.Add_Schema(schema).ConfigureAwait(false);
				//_Schema, 
				if (schema.Id == null)
				{
					_log.Error("Error creating schema");
					error = BacksErrorCodes.SystemError;
					return null;
				}

				var mappedProjetcs = Mapper.Map<Project, ProjectDto>(project);
				mappedProjetcs.Schema = schema;
				return mappedProjetcs;
			}
			catch (Exception e)
			{
				_log.Error("AddNewProject exception : ", e);
				error = BacksErrorCodes.SystemError;
			}


			return null;
		}
		public ProjectDto GetProject(string projId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				var project = _repo.GetProject(projId).Result;
				if (project == null)
				{
					error = BacksErrorCodes.SystemError;
					return null;
				}
				var mappedProjetcs = Mapper.Map<Project, ProjectDto>(project);

				//GetSchema
			}
			catch (Exception e)
			{
				_log.Error("GetProject exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return null;
		}


		public async Task<List<ProjectDto>> GetUserProjects(string acc_id/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				var projects = await _repo.GetAccountProjects(acc_id).ConfigureAwait(false);

				if (projects == null)
				{
					error = BacksErrorCodes.SystemError;
					return null;
				}

				var mappedProjetcs = Mapper.Map<List<Project>, List<ProjectDto>>(projects.ToList());

				//GetSchema
				foreach (var item in mappedProjetcs)
				{
					var schema = await _repo.GetSchema(item.Id).ConfigureAwait(false);
					if (schema != null)
					{
						item.Schema = schema;
					}
				}

				return mappedProjetcs;
			}
			catch (Exception e)
			{
				_log.Error("GetUserProjects exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}


	}
}
