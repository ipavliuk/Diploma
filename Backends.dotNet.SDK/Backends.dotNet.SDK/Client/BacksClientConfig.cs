using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.dotNet.SDK.Client
{
	public class BacksClientConfig
	{
		public string ApplicationId { get; set; }
		public string ApiKey { get; set; }
		public string ApiUri { get; set; }
		public IDictionary<string, string> AdditionalHTTPHeaders { get; set; }
	}
}
