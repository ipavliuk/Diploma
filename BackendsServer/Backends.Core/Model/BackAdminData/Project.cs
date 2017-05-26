using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Model
{
	public class Project
	{
		[BsonId]
		public string Id { get; set; }
		public string AppId { get; set; }
		public string P_AccountId { get; set; }
		public string ApiKeyAccess { get; set; }
		public string MasterKeyAccess { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
