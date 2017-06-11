using BackendsCommon.Types;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backends.Core.Model;

namespace Backends.Core.DataEngine
{
	public partial class BacksRepository
	{
		public async Task AddUser(string appId, BacksUsers user)
		{
			try
			{
				await _context.Get_Users("_User_" + appId).InsertOneAsync(user);
			}
			catch (Exception e)
			{
				_log.Error("Exception in AddUser", e);
				throw;
			}

		}
		public async Task<BacksUsers> Authenticate(string appId, string username, string pwd)
		{
			try
			{
				var filter = Builders<BacksUsers>.Filter.Eq(s => s.AppId, appId) &
				             Builders<BacksUsers>.Filter.Eq(s => s.UserName, username) &
				             Builders<BacksUsers>.Filter.Eq(s => s.Password, pwd);

				return await _context.Get_Users("_User_" + appId)
					.Find(filter)
					.FirstOrDefaultAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in Authenticate", e);
				throw;
			}
		}
		public async Task<BacksUsers> GetUser(string appId, string userId)
		{
			try
			{
				var filter = Builders<BacksUsers>.Filter.Eq("Id", userId);
				return await _context.Get_Users("_User_" + appId)
								.Find(filter)
								.FirstOrDefaultAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetUser", e);
				throw;
			}
		}

		public async Task<List<BacksUsers>> GetUsers(string appId, string query)
		{
			try
			{
				return await _context.Get_Users("_User_" + appId).Find(_ => true).ToListAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetUser", e);
				throw;
			}
		}

		public async Task UpdateUserPasswrod(string appId, string userId, string password)
		{
			try
			{
				var filter = Builders<BacksUsers>.Filter.Eq(s => s.Id, userId);
				var update = Builders<BacksUsers>.Update
								.Set(s => s.Password, password)
								.CurrentDate(s => s.UpdatedAt);
								
				await _context.Get_Users("_User_" + appId).UpdateOneAsync(filter, update);
			}
			catch (Exception e)
			{
				_log.Error("Exception in UpdateUser", e);
				throw;
			}
		}

		public async Task UpdateUserData(string appId, string userId, Dictionary<string, object> data)
		{
			try
			{
				var filter = Builders<BacksUsers>.Filter.Eq(s => s.Id, userId);

				var update = Builders<BacksUsers>.Update
								.Set(s => s.Data, data)
								.CurrentDate(s => s.UpdatedAt);
				await _context.Get_Users("_User_" + appId).UpdateOneAsync(filter, update);
			}
			catch (Exception e)
			{
				_log.Error("Exception in UpdateUser", e);
				throw;
			}
		}

		public async Task RemoveUser(string appId, string userId)
		{
			try
			{
				await _context.Get_Users("_User_" + appId).DeleteOneAsync(Builders<BacksUsers>.Filter.Eq("Id", userId));
			}
			catch (Exception e)
			{
				_log.Error("Exception in RemoveUser", e);
				throw;
			}
		}


		//session
		public async Task AddSession(string appId, BacksSessions session)
		{
			try
			{
				await _context.Get_Sessions("_Session_" + appId).InsertOneAsync(session);
			}
			catch (Exception e)
			{
				_log.Error("Exception in AddSession", e);
				throw;
			}

		}
		public async Task<BacksSessions> GetSession(string appId, string sessionId)
		{
			try
			{
				var filter = Builders<BacksSessions>.Filter.Eq("Id", sessionId);
				return await _context.Get_Sessions("_Session_" + appId)
								.Find(filter)
								.FirstOrDefaultAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetSession", e);
				throw;
			}
		}

		public async Task UpdateSession(string appId, string sessionId, Dictionary<string, object> data)
		{
			try
			{
				var filter = Builders<BacksSessions>.Filter.Eq(s => s.Id, sessionId);

				var update = Builders<BacksSessions>.Update
								.Set(s => s.Data, data)
								.CurrentDate(s => s.UpdatedAt);


				await _context.Get_Sessions("_Session_" + appId).UpdateOneAsync(filter, update);
			}
			catch (Exception e)
			{
				_log.Error("Exception in UpdateSession", e);
				throw;
			}
		}

		public async Task<List<BacksSessions>> GetAllSessions(string appId, string userId)
		{
			try
			{
				var filter = Builders<BacksSessions>.Filter.Eq(s => s.AppId, appId) &
					Builders<BacksSessions>.Filter.Eq(s => s.PUser, userId);

				return await _context.Get_Sessions("_Session_" + appId).Find(filter).ToListAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetAllSessions", e);
				throw;
			}
		}

		public async Task RemoveSession(string appId, string sessionId)
		{
			try
			{
				await _context.Get_Sessions("_Session_" + appId).DeleteOneAsync(Builders<BacksSessions>.Filter.Eq("Id", sessionId));
			}
			catch (Exception e)
			{
				_log.Error("Exception in RemoveSession", e);
				throw;
			}
		}

		public async Task AddEntity(string appId, BacksObject entity)
		{
			try
			{
				await _context.Get_Objects(entity.Name + "_" + appId).InsertOneAsync(entity);
			}
			catch (Exception e)
			{
				_log.Error("Exception in AddEntity", e);
				throw;
			}
		}
		public async Task<BacksObject> GetEntity(string appId, string entityName, string entityId)
		{
			try
			{
				var filter = Builders<BacksObject>.Filter.Eq("Id", entityId);
				return await _context.Get_Objects(entityName + "_" + appId)
								.Find(filter)
								.FirstOrDefaultAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetEntity", e);
				throw;
			}
		}

		public async Task<List<BacksObject>> GetAllEntity(string appId, string entityName)
		{
			try
			{
				var filter = Builders<BacksObject>.Filter.Eq("AppId", appId);
				return await _context.Get_Objects(entityName + "_" + appId).Find(filter).ToListAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetAllEntity", e);
				throw;
			}
		}

		public async Task UpdateEntity(string appId, string entityName, string entityId, Dictionary<string, object> data)
		{
			try
			{
				var filter = Builders<BacksObject>.Filter.Eq(s => s.Id, entityId);

				var update = Builders<BacksObject>.Update
								.Set(s => s.Data, data)
								.CurrentDate(s => s.UpdatedAt);


				await _context.Get_Objects(entityName + "_" + appId).UpdateOneAsync(filter, update);
			}
			catch (Exception e)
			{
				_log.Error("Exception in UpdateEntity", e);
				throw;
			}
		}

		public async Task RemoveEntity(string appId, string entityName, string entityId)
		{
			try
			{
				await _context.Get_Objects(entityName + "_" + appId).DeleteOneAsync(Builders<BacksObject>.Filter.Eq("Id", entityId));
			}
			catch (Exception e)
			{
				_log.Error("Exception in RemoveSession", e);
				throw;
			}
		}
	}
}
