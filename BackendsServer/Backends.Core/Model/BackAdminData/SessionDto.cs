using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Model.BackAdminData
{
	public class SessionDto
	{
		public string Token { get; set; }

		public DateTime? CreatedAt { get; set; }
		
		public DateTime ExpiresAt { get; set; }
		public string PUser { get; set; }

		public Dictionary<string, object> Data { get; set; }
	}
}
