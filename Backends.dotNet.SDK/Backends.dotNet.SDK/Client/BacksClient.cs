using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.dotNet.SDK.Client

{
	public class BacksClient
	{
		#region Initialisation Singlenton
		private BacksClient() { }

		private static readonly Lazy<BacksClient> _instance =
			new Lazy<BacksClient>(() => new BacksClient(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

		public static BacksClient Instance
		{
			get
			{ return _instance.Value; }
		}

		#endregion

		public BacksClientConfig Configuration { get; set; }
		public void Initialize(BacksClientConfig config)
		{
			Configuration = config;
		}

		public void Initialize(string appId, string apiKey, string serverUri)
		{
			Initialize(new BacksClientConfig()
			{
				ApiKey = apiKey,
				ApplicationId = appId,
				ApiUri = serverUri
			});
		}


	}
}
