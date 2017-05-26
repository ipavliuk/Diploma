using Backends.Core.Model;
using BackendsCommon.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.DataEngine
{
	public interface IRepositoryAsync
	{
		#region BacksAdminAccounr&Projects
		Task AddAccount(Account acc);
		Task UpdateAccount(string id, string pwd, string firstName, string lastName, string email);
		Task<Account> GetAccount(string accId);
		Task RemoveAccount(string accId);

		Task AddProject(Project proj);
		Task<Project> GetProject(string projId);
		Task UpdateProject(string projId);
		Task removeProject(string projId);
		Task<IList<Project>> GetAccountProjects(string accId);
		#endregion

	/*	Task<string> Add(string appId, BacksObject bObject);
		Task<BacksObject> GetObject(string appId, string entityClass, string id);
		Task UpdateObject(string appId, string entityClass, string id, BacksObject bObject);
		Task RemoveObject(string appId, string entityClass, string id);
		Task<IList<BacksObject>> GetObjects(string appId, string entityClass);

		//BacksUsers SignInUser(string appId, BacksUsers user);
		//BacksUsers Login(string appId, string userName, string pwd);
		Task<BacksUsers> AddUser(string appId, BacksUsers user);
		Task<BacksUsers> GetUser(string appId, string id);

		Task UpdateUser(string appId, string BacksUsers);
		Task<IList<BacksUsers>> GetUsers(string appId);
		Task RemoveUser(string appId, string id);*/
	}
}
