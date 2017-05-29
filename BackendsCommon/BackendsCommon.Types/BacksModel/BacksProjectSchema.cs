using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackendsCommon.Types.BacksModel
{
	
	public class BacksProjectSchema //: IEnumerable<KeyValuePair<string, EntitiesSchema>>
	{
        public BacksProjectSchema()
        {
            //EntityColumnTypeMapping = new Dictionary<string, EntitiesSchema>();
        }

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string AppId { get; set; }

		public Dictionary<string, EntitiesSchema> EntityColumnTypeMapping { get; set; }

		/*IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IEnumerator<KeyValuePair<string, EntitiesSchema>> GetEnumerator()
		{
			return EntityColumnTypeMapping.GetEnumerator();
		}*/
	}
}
