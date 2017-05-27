using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Backends.Core.Model
{
	public class Project
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string AppId { get; set; }
		public string Name { get; set; }
		public string P_AccountId { get; set; }
		public string ApiKeyAccess { get; set; }
		public string MasterKeyAccess { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
