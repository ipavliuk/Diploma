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

		public async Task<IList<BacksUsers>> GetUsers(string appId, string query)
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

		public async Task UpdateUser(string appId, string userId, string password, Dictionary<string, object> data)
		{
			try
			{
				var filter = Builders<BacksUsers>.Filter.Eq(s => s.Id, userId);
				//var updater = Builders<Account>.Update;

				var update = Builders<BacksUsers>.Update
								.Set(s => s.Password, password)
								.Set(s => s.Data, data)
								.CurrentDate(s => s.UpdatedAt);
								
				//.CurrentDate(s => s.);

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
		public async Task<BacksUsers> GetSession(string appId, string sessionId)
		{
			try
			{
				var filter = Builders<BacksUsers>.Filter.Eq("Id", sessionId);
				return await _context.Get_Users("_Session_" + appId)
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

		public async Task GetAllSessions(string appId, string userId)
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

		public async Task RemoveSession(string appId, string userId)
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

	}
}
