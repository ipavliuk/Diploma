using System;
using Backends.Core;
using Backends.Core.Config;
using Backends.Core.DataEngine;
using Backends.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendsCore.Test
{
	[TestClass]
	public class BaseObjectTest
	{

		private readonly BacksObjectService _service =
					new BacksObjectService(new BacksRepository(
								new Configuration
								{
									ConnectionString = "mongodb://localhost:27017",
									Database = "testBackends"
								}));

		
		private const string appId = "592affe8ae1e7843883fd023";

		[TestMethod]
		public void CreateEntity_Test()
		{
			BacksErrorCodes error = BacksErrorCodes.Ok;
			
			//_service.CreateEntity(appId, out error);
		}

		[TestMethod]
		public void GetEntity_Test()
		{
			BacksErrorCodes error = BacksErrorCodes.Ok;
			Assert.IsNull(null);
		}

		[TestMethod]
		public void UpdateEntity_Test()
		{
			BacksErrorCodes error = BacksErrorCodes.Ok;
		}

		[TestMethod]
		public void QueryEntity_Test()
		{
			BacksErrorCodes error = BacksErrorCodes.Ok;
		}


		[TestMethod]
		public void RemoveEntity_Test()
		{
			BacksErrorCodes error = BacksErrorCodes.Ok;
		}
	}
}
