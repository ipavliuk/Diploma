using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backends.Core.DataEngine;
using Backends.Core.Model;
using Backends.Core.Model.BackAdminData;
using BackendsCommon.Logging;
using AutoMapper;
using BackendsCommon.Types;
using BackendsCommon.Types.BacksModel;

namespace Backends.Core.Services
{
	public class BacksDashboardService
	{
		private readonly IRepositoryAsync _repo;
		private ILog _log = new Log(typeof(BacksDashboardService));

		public BacksDashboardService(IRepositoryAsync repo)
		{
			_repo = repo;
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

				_repo.AddAccount(acc).Wait();

				return !string.IsNullOrEmpty(acc.Id) ? acc.Id : null;
			}
			catch (Exception e)
			{
				_log.Error("SignIn exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}
		public AccountDto Login(string login,string pwd, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				Account acc = _repo.Login(login, pwd).Result;

				if (acc == null)
				{
					error = BacksErrorCodes.AuthFailed;
					return null;
				}

				var projects = _repo.GetAccountProjects(acc.Id).Result.ToList();

				var mappedAccount = Mapper.Map<Account, AccountDto>(acc);
				var mappedProjetcs = Mapper.Map<List<Project>, List<ProjectDto>>(projects);
				mappedAccount.Projects = mappedProjetcs;

				return mappedAccount;
			}
			catch (Exception e)
			{
				_log.Error("LogIn exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return null;
		}


		public ProjectDto AddNewProject(string name, string acc_id, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
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

				_repo.AddProject(project).Wait();
				if (project == null)
				{
					error = BacksErrorCodes.ProjectCreationFailed;
				}
				//Create basic entities schema: _User, _Sessions, 
				var schema = new BacksProjectSchema()
				{
					AppId = project.Id,
					EntityColumnTypeMapping = new Dictionary<string, EntitiesSchema>()
					{
						{
							"_User", new EntitiesSchema()
							{
								ColumnTypeMapping = new Dictionary<string, string>()
								{
									{ "_id", BacksDataType.BString},
									{ "userName", BacksDataType.BString},
									{ "paswword", BacksDataType.BString},
									{ "createdAt", BacksDataType.BTime},
									{ "updatedAt", BacksDataType.BTime},
									{ "userData", BacksDataType.BObject}
								}
							}
						},
						{
							"_Session", new EntitiesSchema()
							{
								ColumnTypeMapping = new Dictionary<string, string>()
								{
									{ "_id", BacksDataType.BString},
									{ "sessionToken", BacksDataType.BString},
									{ "createdAt", BacksDataType.BTime},
									{ "updatedAt", BacksDataType.BTime},
									{ "expiredAt", BacksDataType.BTime},
									{ "installationId", BacksDataType.BString},
									{ "sessionData", BacksDataType.BString},
									{ "previleges", BacksDataType.BBoolean},
									{ "_p_user", BacksDataType.BPointer}
								}
							}
						},
						{
							"_Roles", new EntitiesSchema()
							{
								ColumnTypeMapping = new Dictionary<string, string>()
								{
									{ "_id", BacksDataType.BString},
									{ "name", BacksDataType.BString},
									{ "paswword", BacksDataType.BString},
									{ "createdAt", BacksDataType.BTime},
									{ "updatedAt", BacksDataType.BTime},
									{ "userData", BacksDataType.BObject}
								}
							}
						}

					}

				};
				_repo.Add_Schema(schema).Wait();
				//_Schema, 


				var mappedProjetcs = Mapper.Map<Project, ProjectDto>(project);
				return mappedProjetcs;
			}
			catch (Exception e)
			{
				_log.Error("AddNewProject exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			
			
			return null;
		}
		public ProjectDto GeProject()
		{
			return null;
		}


		public List<ProjectDto> GetUserProjects()
		{
			return null;
		}

		
	}
}
