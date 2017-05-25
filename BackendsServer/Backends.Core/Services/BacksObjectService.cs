using Backends.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backends.Core.Services
{
	public class BacksObjectService
	{
		private readonly IRepository _repository;
		public BacksObjectService(IRepository repository)
		{
			_repository = repository;
		}

		//
	}
}
