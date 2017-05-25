using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core
{
	public partial class BackendsServerManager
	{
		#region Initialisation Singlenton
		private BackendsServerManager() { }

		private static readonly Lazy<BackendsServerManager> _instance =
			new Lazy<BackendsServerManager>(() => new BackendsServerManager(), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

		public static BackendsServerManager Instance
		{
			get
			{ return _instance.Value; }
		}

		#endregion


	}
}
