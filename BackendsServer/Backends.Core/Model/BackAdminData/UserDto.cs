using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Model.BackAdminData
{
	public class UserDto
	{
		public UserDto()
		{
			Data = new Dictionary<string, object>();
		}

		public Dictionary<string, object> Data { get; set; }
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public DateTime CreatedAt { get; set; }
		public string SessionId { get; set; }

	}
}
