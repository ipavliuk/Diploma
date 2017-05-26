using Backends.Core.Config;
using Backends.Core.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.DataEngine
{
	public class BacksRepository : IRepositoryAsync
	{
		private readonly BackendsContext _context = null;
		public BacksRepository(Configuration config)
		{
			_context = new BackendsContext(config.ConnectionString, config.Database);
		}

		public async Task AddAccount(Account acc)
		{//Validate screenname
			await _context.Accounts.InsertOneAsync(acc);
		}
		public async Task UpdateAccount(string id, string pwd, string firstName, string lastName, string email)
		{
			var filter = Builders<Account>.Filter.Eq(s => s.Id, id);
			var updater = Builders<Account>.Update;

			var update = Builders<Account>.Update
							.Set(s => s.FirstName, firstName)
							.Set(s => s.LastName, lastName)
							.Set(s => s.Email, email);
							//.CurrentDate(s => s.);

			await _context.Accounts.UpdateOneAsync(filter, update);
		}
		public async Task<Account> GetAccount(string accId)
		{
			var filter = Builders<Account>.Filter.Eq("Id", accId);
			return await _context.Accounts
							.Find(filter)
							.FirstOrDefaultAsync();
		}
		public async Task RemoveAccount(string accId)
		{
			 await _context.Accounts.DeleteOneAsync(
				 Builders<Account>.Filter.Eq("Id", accId));
		}

		public async Task AddProject(Project proj)
		{
			await _context.Projects.InsertOneAsync(proj);
		}
		public async Task<Project> GetProject(string projId)
		{
			var filter = Builders<Project>.Filter.Eq("Id", projId);
			return await _context.Projects
							.Find(filter)
							.FirstOrDefaultAsync();
		}
		public async Task UpdateProject(string projId)
		{

		}
		public async Task removeProject(string projId)
		{
			await _context.Projects.DeleteOneAsync(
				 Builders<Project>.Filter.Eq("Id", projId));
		}
		public async Task<IList<Project>> GetAccountProjects(string accId)
		{
			var filter = Builders<Project>.Filter.Eq("P_AccountId", accId);
			return await _context.Projects.Find(filter).ToListAsync();
		}
	}
}
