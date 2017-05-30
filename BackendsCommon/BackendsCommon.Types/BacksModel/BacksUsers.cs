using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendsCommon.Types
{
	public class BacksUsers : BacksObject
	{
		public BacksUsers()
		{
			this.Name = "_User";
		}
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		//public string SessionId { get; set; } 
	}
}
