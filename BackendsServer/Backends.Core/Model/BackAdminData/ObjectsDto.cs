using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Model.BackAdminData
{
	[DataContract]
	public class ObjectsDto
	{
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public string Id { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public string Name { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public DateTime? CreatedAt { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public DateTime? UpdatedAt { get; set; }

		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public Dictionary<string, object> Data { get; set; }
		
		public ObjectsDto()
		{
			Data = new Dictionary<string, object>();
		}
	}
}
