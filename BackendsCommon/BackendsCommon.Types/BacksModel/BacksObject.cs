using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendsCommon.Types
{
	public class BacksObject //: IEnumerable<KeyValuePair<string, object>>//: IBaseObject
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string Name { get; set; }
		public DateTime? CreatedAt { get; set; }
	
		public DateTime? UpdatedAt { get; set; }
		public string AppId { get; set; }
		public object this[string key]
		{
			get
			{
				return data[key];
			}
		}

		public bool ContainsKey(string key)
		{
			return Data.ContainsKey(key);
		}

		private Dictionary<string, object> data = new Dictionary<string, object>();
		public Dictionary<string, object> Data
		{
			get { return data; }
			set { data = value; }
		}

		public BacksObject()
		{
			
		}

	/*	public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return Data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}*/
	}
}
