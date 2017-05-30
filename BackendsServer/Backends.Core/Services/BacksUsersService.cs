using Backends.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Services
{
	public class BacksUsersService
	{
		private readonly IRepository _repository;

		public BacksUsersService(IRepository repository)
		{
			_repository = repository;
		}




	}
}
