using System;
using Backends.Core;
using Backends.Core.Config;
using Backends.Core.DataEngine;
using Backends.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendsCore.Test
{
	[TestClass]
	public class DashboardServiceTest
	{
		private readonly BacksDashboardService _service = 
					new BacksDashboardService(new BacksRepository(
								new Configuration
								{
									ConnectionString = "mongodb://localhost:27017",
									Database = "testBackends"
								}));
		[TestInitialize]
		public void Initialize()
		{
			
		}

		[TestMethod]
		public void SignIn_Test_OK()
		{
			BacksErrorCodes error = BacksErrorCodes.Ok;
			string _id = _service.SignIn("TestUser", "Pav1", "Test@gmail.com", "duduz", Guid.NewGuid().ToString("N"), out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok, "SignIn_Test_OK => failed to get proper error code");
			Assert.IsNotNull(_id, "SignIn_Test_OK => failed to get id");
		}

		[TestMethod]
		public void SignIn_Test_Duplicates()
		{
			_service.RepoInstance.DropCollection("Accounts");
			BacksErrorCodes error = BacksErrorCodes.Ok;
			string _id1 = _service.SignIn("TestUser", "Pav1", "Test@gmail.com", "SignIn_Test_Duplicates", Guid.NewGuid().ToString("N"), out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok, "SignIn_Test_Duplicates => failed to get proper error code");
			Assert.IsNotNull(_id1, "SignIn_Test_Duplicates => failed to get id");


			string _id2 = _service.SignIn("TestUser", "Pav1", "Test@gmail.com", "SignIn_Test_Duplicates", Guid.NewGuid().ToString("N"), out error);

			Assert.AreEqual(error, BacksErrorCodes.DuplicateLogin, "SignIn_Test_Duplicates => failed to get DuplicateLogin error ");
			Assert.IsNull(_id2, "SignIn_Test_Duplicates=> failed to sign in");
		}
		[TestMethod]
		public void Login()
		{

		}

		[TestMethod]
		public void AddProject()
		{

		}

	}
}
