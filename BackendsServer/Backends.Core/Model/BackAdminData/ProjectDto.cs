using BackendsCommon.Types.BacksModel;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Model.BackAdminData
{
	public class ProjectDto
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string AppId { get; set; }

		public string ApiKeyAccess { get; set; }

		public string MasterKeyAccess { get; set; }

		public string Settings { get; set; }

		public long UserCount { get; set; }

		public long InstallationCount { get; set; }

		public DateTime CreatedAt { get; set; }

        public BacksProjectSchema Schema { get; set; }

    }
}
