using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendsCommon.Types;
using BackendsCommon.Logging;

namespace Backends.Core.Model
{
	public class InMemoryRepository : IRepository
	{
		private readonly ICollection<BacksUsers> _users;
		private readonly ICollection<BacksObject> _objects;
		private readonly ICollection<BacksSessions> _sessions;
		private readonly ICollection<Account> _accounts;
		private readonly ICollection<Project> _projects;

		private ILog _log = new Log(typeof(InMemoryRepository));
		public InMemoryRepository()
		{
			_users = new SafeCollection<BacksUsers>();
			_objects = new SafeCollection<BacksObject>();
			_sessions = new SafeCollection<BacksSessions>();
			_accounts = new SafeCollection<Account>();
			_projects = new SafeCollection<Project>();
		}
		public void Add(string appId, BacksSessions session)
		{
			if (session == null)
			{
				return ;
			}
			session.Id = Guid.NewGuid().ToString("N");
			session.Token = Guid.NewGuid().ToString("N");


			_sessions.Add(session);
		}

		public string Add(string appId, BacksObject bObject)
		{
			if(bObject == null)
			{
				return null;
			}
			bObject.Id = Guid.NewGuid().ToString("N");
			_objects.Add(bObject);
			return bObject.Id;
		}

		public void AddAccount(Account acc)
		{
			if (acc == null)
			{
				return ;
			}
			acc.Id = Guid.NewGuid().ToString("N");
			_accounts.Add(acc);
			
		}

		public void AddProject(Project proj)
		{
			if (proj == null)
			{
				return;
			}
			proj.Id = Guid.NewGuid().ToString("N");
			_projects.Add(proj);

		}

		public void AddUser(string appId, BacksUsers user)
		{
			if (user == null)
			{
				return;
			}
			user.Id = Guid.NewGuid().ToString("N");

			//Create session
			var session = new BacksSessions()
			{
				Id = Guid.NewGuid().ToString("N"),
				AppId = appId,
				PUser = user.Id,
				CreatedAt = user.CreatedAt,
				UpdatedAt = user.UpdatedAt,
				Token = Guid.NewGuid().ToString("N")
			};
			_sessions.Add(session);
			_users.Add(user);
			
		}

		public Account GetAccount(string accId)
		{
			return _accounts.FirstOrDefault(acc =>acc.Id != null &&  acc.Id.Equals(accId, StringComparison.OrdinalIgnoreCase));
		}

		public List<Project> GetAccountProjects(string accId)
		{
			return _projects.Where(pr => pr.Id!=null && pr.P_AccountId != null && pr.P_AccountId.Equals(accId, StringComparison.OrdinalIgnoreCase)).ToList();

		}

		public BacksObject GetObject(string appId, string entityClass, string id)
		{
			return _objects.FirstOrDefault(o => o.Id != null && o.Id.Equals(id, StringComparison.OrdinalIgnoreCase) &&
													o.AppId.Equals(appId, StringComparison.OrdinalIgnoreCase)&&
													o.Name.Equals(entityClass, StringComparison.OrdinalIgnoreCase));
		}

		public IList<BacksObject> GetObjects(string appId, string entityClass)
		{
			return _objects.Where(o => o.Id != null && o.AppId != null && o.AppId.Equals(appId, StringComparison.OrdinalIgnoreCase)
								&& o.Name.Equals(entityClass, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public Project GetProject(string projId)
		{
			return _projects.FirstOrDefault(pr => pr.Id != null && pr.Id.Equals(projId, StringComparison.OrdinalIgnoreCase));
		}

		public BacksSessions GetSession(string appId, string id, string token)
		{
			return _sessions.FirstOrDefault(s => s.Id != null && s.AppId != null && s.AppId.Equals(appId, StringComparison.OrdinalIgnoreCase)
								&& s.Token.Equals(token, StringComparison.OrdinalIgnoreCase));
		}

		public List<BacksSessions> GetSessions(string appId, string token)
		{
			return _sessions.Where(s => s.Id != null && s.AppId != null && s.AppId.Equals(appId, StringComparison.OrdinalIgnoreCase)
								&& s.Token.Equals(token, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public BacksUsers GetUser(string appId, string id)
		{
			return _users.FirstOrDefault(u => u.Id != null && u.AppId != null && u.AppId.Equals(appId, StringComparison.OrdinalIgnoreCase)
								&& u.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
		}
	

		public List<BacksUsers> GetUsers(string appId)
		{
			return _users.Where(u => u.Id != null && u.AppId != null && u.AppId.Equals(appId, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public BacksUsers Login(string appId, string userName, string pwd)
		{

			return _users.FirstOrDefault(u => u.AppId != null && u.AppId.Equals(appId, StringComparison.OrdinalIgnoreCase) &&
											u.UserName.Equals(appId, StringComparison.OrdinalIgnoreCase) &&
											u.Password.Equals(pwd, StringComparison.OrdinalIgnoreCase));
		}

		public void PasswordReset(string appId, string id)
		{
			throw new NotImplementedException();
		}

		public void RemoveAccount(string accId)
		{
			var acc = GetAccount(accId);
			if(acc != null)
			{
				_accounts.Remove(acc);
			}
			else
			{
				_log.ErrorFormat("Remove account : {0}", accId);
			}
			
		}

		public void RemoveObject(string appId, string entityClass, string id)
		{
			var obj = GetObject(appId, entityClass, id);
			if (obj != null)
			{
				_objects.Remove(obj);
			}
			else
			{
				_log.ErrorFormat("Remove Object entityClass: {0}; _id :{1}, appId:{2} ", entityClass, id, appId);
			}

		}

		public void removeProject(string projId)
		{
			var proj = GetProject(projId);
			if (proj != null)
			{
				_projects.Remove(proj);
			}
			else
			{
				_log.ErrorFormat("Remove project : {0}", projId);
			}
		}

		public void RemoveSessions(string appId, string id, string token)
		{
			var session = GetSession(appId, id, token);
			if (session != null)
			{
				_sessions.Remove(session);
			}
			else
			{
				_log.ErrorFormat("Remove Sessions token: {0}; _id :{1}, appId:{2} ", token, id, appId);
			}
		}

		public void RemoveUser(string appId, string id)
		{
			var user = GetUser(appId, id);
			if (user != null)
			{
				_users.Remove(user);
			}
			else
			{
				_log.ErrorFormat("Remove Users :{0}, appId:{1} ", id, appId);
			}
		}

		public BacksUsers SignInUser(string appId, BacksUsers user)
		{
			if (user == null)
			{
				return null;
			}
			user.Id = Guid.NewGuid().ToString("N");
			
			_users.Add(user);

			return user; 
		}

		public void UpdateAccount(Account acc)
		{
			throw new NotImplementedException();
		}

		public void UpdateObject(string appId, string entityClass, string id)
		{
			throw new NotImplementedException();
		}

		public void UpdateProject(string projId)
		{
			throw new NotImplementedException();
		}

		public void UpdateSession(string appId, string id, string token)
		{
			throw new NotImplementedException();
		}

		public void UpdateUser(string appId, string id)
		{
			throw new NotImplementedException();
		}

		
	}
}
