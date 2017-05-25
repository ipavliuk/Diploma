using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendsCommon.Types;


namespace Backends.Core.Model
{
    public interface IRepository
    {
		string Add(string appId, BacksObject bObject);
		BacksObject GetObject(string appId, string entityClass, string id);
		void UpdateObject(string appId, string entityClass, string id);
		void RemoveObject(string appId, string entityClass, string id);
		IList<BacksObject> GetObjects(string appId, string entityClass);


		BacksUsers SignInUser(string appId, BacksUsers user);
		BacksUsers Login(string appId, string userName, string pwd);
		void AddUser(string appId, BacksUsers user);
		BacksUsers GetUser(string appId, string id);
		List<BacksUsers> GetUsers(string appId);
		void RemoveUser(string appId, string id);

		void UpdateUser(string appId, string id);
		void PasswordReset(string appId, string id);


		// restrict creation session for objects
		void Add(string appId, BacksSessions session);
		BacksSessions GetSession(string appId, string id, string token);
		void UpdateSession(string appId, string id, string token);
		List<BacksSessions> GetSessions(string appId, string token);
		void RemoveSessions(string appId, string id, string token);

		#region BacksAdminAccounr&Projects
		void AddAccount(Account acc);
		void UpdateAccount(Account acc);
		Account GetAccount(string accId);
		void RemoveAccount(string accId);
		
		void AddProject(Project proj);
		Project GetProject(string projId);
		void UpdateProject(string projId);
		void removeProject(string projId);
		List<Project> GetAccountProjects(string accId);
		#endregion

	}
}
