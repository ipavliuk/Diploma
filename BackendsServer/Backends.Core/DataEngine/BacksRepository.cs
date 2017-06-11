using Backends.Core.Config;
using Backends.Core.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendsCommon.Logging;
using BackendsCommon.Types;
using BackendsCommon.Types.BacksModel;
using MongoDB.Bson;

namespace Backends.Core.DataEngine
{
	public partial class BacksRepository : IRepositoryAsync
	{
		private readonly BackendsContext _context = null;
		private ILog _log = new Log(typeof(BacksRepository));
		public BacksRepository(Configuration config)
		{
			_context = new BackendsContext(config.ConnectionString, config.Database);
		}
		public async Task<IList<Account>> GetAllAccounts()
		{
			try
			{
				return await _context.Accounts.Find(_ => true).ToListAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetAllAccounts", e);
				throw;
			}
		}
		public async Task AddAccount(Account acc)
		{//Validate screenname
			try
			{
				await _context.Accounts.InsertOneAsync(acc);
			}
			catch (Exception e)
			{
				_log.Error("Exception in AddAccount" , e);
				throw;
			}
		}
		public async Task UpdateAccount(string id, string pwd, string firstName, string lastName, string email)
		{
			try
			{
				var filter = Builders<Account>.Filter.Eq(s => s.Id, id);
				var updater = Builders<Account>.Update;
				var update = Builders<Account>.Update
								.Set(s => s.FirstName, firstName)
								.Set(s => s.LastName, lastName)
								.Set(s => s.Email, email);
				await _context.Accounts.UpdateOneAsync(filter, update);
			}
			catch (Exception e)
			{
				_log.Error("Exception in UpdateAccount", e);
				throw;
			}
			
		}
		public async Task<Account> GetAccount(string accId)
		{
			try
			{
				var filter = Builders<Account>.Filter.Eq("Id", accId);
				return await _context.Accounts
								.Find(filter)
								.FirstOrDefaultAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetAccount", e);
				throw;
			}
		}
		public async Task<Account> Login(string login, string pwd)
		{
			try
			{
				var filter = Builders<Account>.Filter.Eq(s => s.ScreenName, login) &
					Builders<Account>.Filter.Eq(s => s.Pasword, pwd);
				return await _context.Accounts
								.Find(filter)
								.FirstOrDefaultAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in Login", e);
				throw;
			}
		}
		public async Task RemoveAccount(string accId)
		{
			try
			{
				await _context.Accounts.DeleteOneAsync(Builders<Account>.Filter.Eq("Id", accId));
			}
			catch (Exception e)
			{
				_log.Error("Exception in RemoveAccount", e);
				throw;
			}
			
		}
		public async Task<IList<Account>> FindDuplicates(string login)
		{
			try
			{
				var filter = Builders<Account>.Filter.Eq("ScreenName", login);
				return await _context.Accounts.Find(filter).ToListAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in FindDuplicates", e);
				throw;
			}
		}
		public async Task AddProject(Project proj)
		{
			try
			{
				await _context.Projects.InsertOneAsync(proj);
			}
			catch (Exception e)
			{
				_log.Error("Exception in AddProject", e);
				throw;
			}
			
		}
		public async Task<Project> GetProject(string projId)
		{
			try
			{
				var filter = Builders<Project>.Filter.Eq("Id", projId);
				return await _context.Projects
								.Find(filter)
								.FirstOrDefaultAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetProject", e);
				throw;
			}
		}
		public async Task UpdateProject(string projId)
		{

		}
		public async Task RemoveProject(string projId)
		{
			try
			{
				await _context.Projects.DeleteOneAsync(Builders<Project>.Filter.Eq("Id", projId));
			}
			catch (Exception e)
			{
				_log.Error("Exception in RemoveProject", e);
				throw;
			}
		}
		public async Task<IList<Project>> GetAccountProjects(string accId)
		{
			try
			{
				var filter = Builders<Project>.Filter.Eq("P_AccountId", accId);
				return await _context.Projects.Find(filter).ToListAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetAccountProjects", e);
				throw;
			}
		}
		public async Task<IList<Project>> GetAllProject()
		{
			try
			{
				return await _context.Projects.Find(_ => true).ToListAsync();
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetAllProject", e);
				throw;
			}
		}
		public async Task Add_Schema(BacksProjectSchema schema)
		{
			try
			{
				await _context._Schema.InsertOneAsync(schema);
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetAllProject", e);
				throw;
			}
		}
		public async Task<BacksProjectSchema> GetSchema(string appId)
		{
			try
			{
				var filter = Builders<BacksProjectSchema>.Filter.Eq(s=>s.AppId, appId);
				return await _context._Schema.Find(filter).FirstOrDefaultAsync(); 
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetSchema", e);
				throw;
			}
		}

		public async Task Update_Schema(string appId, Dictionary<string, EntitiesSchema> data)
		{
			try
			{
				var filter = Builders<BacksProjectSchema>.Filter.Eq(s => s.AppId, appId);

				var update = Builders<BacksProjectSchema>.Update.Set(s => s.EntityColumnTypeMapping, data);
					//.CurrentDate(s => s.UpdatedAt);


				await _context._Schema.UpdateOneAsync(filter, update);
				
			}
			catch (Exception e)
			{
				_log.Error("Exception in GetSchema", e);
				throw;
			}
		}

		public void CreateCollection(string name, BsonDocument validator)
		{
			try
			{
				_context.CreateCollection(name, validator);
			}
			catch (Exception e)
			{
				_log.Error("Exception in CreateCollection", e);
				throw;
			}
		}
		public void DropDB(string name)
		{
			_context.DropDB(name);
		}

		public void DropCollection(string name)
		{
			_context.DropCollection(name);
		}
	}
}
