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
		public AccountDto()
		{
			Projects = new List<ProjectDto>();
		}
		public string Id { get; set; }
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string email { get; set; }

		public List<ProjectDto> Projects{ get; set; }

		public BacksErrorCodes Error { get; set; }

	}
}
