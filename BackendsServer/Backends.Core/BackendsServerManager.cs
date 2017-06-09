using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backends.Core.Config;
using Backends.Core.DataEngine;
using Backends.Core.Services;

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

		private BacksUsersServiceAsync _userService;

		private BacksObjectServiceAsync _dataService;
		
		public BacksUsersServiceAsync UserService { get { return _userService; } }
		public BacksObjectServiceAsync DataService { get { return _dataService; } }

		private BacksDashboardServiceAsync _adminService;
		public BacksDashboardServiceAsync AdminService { get { return _adminService; } }

		public void Start(string connectionStr, string database)
		{
			var config = new Configuration
			{
				ConnectionString = string.IsNullOrEmpty(connectionStr) ? "mongodb://localhost:27017" : connectionStr,
				Database = string.IsNullOrEmpty(database) ? "BackendsDatabase" : database
			};
			_userService = new BacksUsersServiceAsync(new BacksRepository(config));

			_dataService = new BacksObjectServiceAsync(new BacksRepository(config));

			_adminService = new BacksDashboardServiceAsync(new BacksRepository(config));
		}

		#endregion


	}
}
