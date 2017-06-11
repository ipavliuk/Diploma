using Backends.Core.Model;
using BackendsCommon.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendsCommon.Types.BacksModel;
using MongoDB.Bson;

namespace Backends.Core.DataEngine
{
	public interface IRepositoryAsync
	{
		#region BacksAdminAccounr&Projects
		Task AddAccount(Account acc);
		Task<IList<Account>> FindDuplicates(string login);
		Task UpdateAccount(string id, string pwd, string firstName, string lastName, string email);
		Task<Account> GetAccount(string accId);
		Task RemoveAccount(string accId);
		Task<Account> Login(string login, string pwd);
		Task<IList<Account>> GetAllAccounts();

		Task AddProject(Project proj);
		Task<Project> GetProject(string projId);
		Task UpdateProject(string projId);
		Task RemoveProject(string projId);
		Task<IList<Project>> GetAccountProjects(string accId);
		Task<IList<Project>> GetAllProject();


		#endregion

		#region Backends Projects

		Task Add_Schema(BacksProjectSchema schema);
		Task<BacksProjectSchema> GetSchema(string appId);
		Task Update_Schema(string appId, Dictionary<string, EntitiesSchema> data);

		void CreateCollection(string name, BsonDocument validator);
		#endregion

		void DropDB(string name);
		void DropCollection(string name);

		#region BacksAPIObjects
		//users
		Task AddUser(string appId, BacksUsers user);

		Task<BacksUsers> Authenticate(string appId, string username, string pwd);
		Task<BacksUsers> GetUser(string appId, string userId);
		Task<List<BacksUsers>> GetUsers(string appId, string query);
		Task UpdateUserPasswrod(string appId, string userId, string password);
		Task UpdateUserData(string appId, string userId, Dictionary<string, object> data);

		Task RemoveUser(string appId, string userId);

		//Task<BacksUsers> GetUser(string appId, string userId);
		Task AddSession(string appId, BacksSessions session);
		Task<BacksSessions> GetSession(string appId, string sessionId);
		Task UpdateSession(string appId, string sessionId, Dictionary<string, object> data);
		Task<List<BacksSessions>> GetAllSessions(string appId, string userId);
		Task RemoveSession(string appId, string sessionId);

		Task AddEntity(string appId, BacksObject entity);
		Task<BacksObject> GetEntity(string appId, string entityName, string entityId);
		Task<List<BacksObject>> GetAllEntity(string appId, string entityName);
		Task UpdateEntity(string appId, string entityName, string entityId, Dictionary<string, object> data);
		Task RemoveEntity(string appId, string entityName, string entityId);


		#endregion

	}
}
