using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Model.BackAdminData
{
	[DataContract]
	public class UserDto : BaseData
	{
		public UserDto()
		{
			//Data = new Dictionary<string, object>();
		}

		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public Dictionary<string, object> Data { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public string Id { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public string UserName { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public string Email { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public DateTime CreatedAt { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public DateTime UpdatedAt { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public string SessionId { get; set; }

	}
}
