using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backends.Core.Model.BackAdminData;

namespace Backends.Core.Model
{
	public class AccountDto
	{
		public string Id { get; set; }

		public List<ProjectDto> Projects{ get; set; }
	}
}
