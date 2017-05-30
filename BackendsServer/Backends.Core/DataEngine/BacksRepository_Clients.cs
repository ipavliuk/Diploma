using BackendsCommon.Types;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public async Task UpdateUser(string appId, string userId, string password, Dictionary<string, object> data)
		{
			try
			{
				var filter = Builders<BacksUsers>.Filter.Eq(s => s.Id, userId);
				//var updater = Builders<Account>.Update;

				var update = Builders<BacksUsers>.Update
								.Set(s => s.Password, password)
								.Set(s => s.Data, data).
								.CurrentDate(s => s.UpdatedAt);
								
				//.CurrentDate(s => s.);

				await _context.Get_Users("_User_" + appId).UpdateOneAsync(filter, update);
			}
			catch (Exception e)
			{
				_log.Error("Exception in UpdateAccount", e);
				throw;
			}

		}
	}
}
