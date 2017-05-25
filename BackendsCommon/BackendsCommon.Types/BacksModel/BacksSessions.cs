using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendsCommon.Types
{
	public class BacksSessions : BacksObject
	{
		public BacksSessions()
		{
			this.Name = "_Session";
		}
		public string Token { get; set; }
		public DateTime ExpiresAt { get; set; }	
		public string PUser { get; set; }
	}
}
