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
        void CreateCollection(string name, BsonDocument validator);
		#endregion

		void DropDB(string name);
		void DropCollection(string name);

		#region BacksAPIObjects
		//users
		Task AddUser(string appId, BacksUsers user);
		Task<BacksUsers> GetUser(string appId, string userId);
		Task<IList<BacksUsers>> GetUsers(string appId, string query);
		Task UpdateUser(string appId, string userId, string password, Dictionary<string, object> data);
		Task RemoveUser(string appId, string userId);

		//Task<BacksUsers> GetUser(string appId, string userId);
		Task AddSession(string appId, BacksSessions session);
		Task<BacksUsers> GetSession(string appId, string sessionId);
		Task UpdateSession(string appId, string sessionId, Dictionary<string, object> data);

		#endregion

		/*	Task<string> Add(string appId, BacksObject bObject);
					Task<BacksObject> GetObject(string appId, string entityClass, string id);
					Task UpdateObject(string appId, string entityClass, string id, BacksObject bObject);
					Task RemoveObject(string appId, string entityClass, string id);
					Task<IList<BacksObject>> GetObjects(string appId, string entityClass);
		
					//BacksUsers SignInUser(string appId, BacksUsers user);
					//BacksUsers Login(string appId, string userName, string pwd);
					Task<BacksUsers> AddUser(string appId, BacksUsers user);
					
		
					Task UpdateUser(string appId, string BacksUsers);
					Task<IList<BacksUsers>> GetUsers(string appId);
					Task RemoveUser(string appId, string id);*/
	}
}
