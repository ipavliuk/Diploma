using Backends.Core.Config;
using Backends.Core.Model;
using BackendsCommon.Types;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.DataEngine
{
	//Init of work implemantaion
	public class BackendsContext
	{
		private readonly IMongoDatabase _db = null;

		public BackendsContext(string connectionString, string database)
		{
			var client = new MongoClient(connectionString);
			if (client != null)
				_db = client.GetDatabase(database);
		}

		
		public IMongoCollection<BsonDocument>  GetCollection(string collection)
		{
			return _db.GetCollection<BsonDocument>(collection);
		}

		public IMongoCollection<BacksUsers> Get_Users(string collection)
		{
			return _db.GetCollection<BacksUsers>(collection);
		}

		public IMongoCollection<Account> Accounts
		{
			get
			{
                return _db.GetCollection<Account>("Accounts");
			}
		}

		public IMongoCollection<Project> Projects
		{
			get
			{
                return _db.GetCollection<Project>("Projects");
			}
		}

        internal void DropDB(string name)
        {
            _db.Client.DropDatabase(name);
                
        }
    }
}
