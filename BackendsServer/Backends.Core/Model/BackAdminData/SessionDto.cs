using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Model.BackAdminData
{
	[DataContract]
	public class SessionDto //: BaseData
	{
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public string Token { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public DateTime? CreatedAt { get; set; }
		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public DateTime ExpiresAt { get; set; }

		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public DateTime? UpdatedAt { get; set; }

		[DataMember(IsRequired = false, EmitDefaultValue = false)]
		public string PUser { get; set; }

		public Dictionary<string, object> Data { get; set; }
	}
}
