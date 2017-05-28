using System;
using System.Linq;
using Backends.Core;
using Backends.Core.Config;
using Backends.Core.DataEngine;
using Backends.Core.Model;
using Backends.Core.Model.BackAdminData;
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

			Assert.AreEqual(error, BacksErrorCodes.DuplicateLogin, string.Format("SignIn_Test_Duplicates => failed to get DuplicateLogin error ; errorCode = {0}", error));
			Assert.IsNull(_id2, "SignIn_Test_Duplicates=> failed to sign in");
		}
		[TestMethod]
		public void Login_Test_Ok()
		{
			_service.RepoInstance.DropCollection("Accounts");
			BacksErrorCodes error = BacksErrorCodes.Ok;
			string login = "LoginTestUser";
			string pwd = Guid.NewGuid().ToString("N");
			string _id1 = _service.SignIn("TestUser", "Pav1", "Test@gmail.com", login, pwd, out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok, "Login_Test_Ok => failed to get proper error code");
			Assert.IsNotNull(_id1, "Login => failed to create accoutn id");

			error = BacksErrorCodes.Ok;
			AccountDto account = _service.Login("LoginTestUser", pwd, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok, "Login => failed to get proper error code");
			Assert.AreEqual(account.Id, _id1, string.Format("Login => failed to login user errorCode = {0}", error));
		}

		[TestMethod]
		public void Login_Test_Failed()
		{
			_service.RepoInstance.DropCollection("Accounts");
			BacksErrorCodes error = BacksErrorCodes.Ok;
			string login = "LoginTestUser";
			string pwd = Guid.NewGuid().ToString("N");
			string _id1 = _service.SignIn("TestUser", "Pav1", "Test@gmail.com", login, pwd, out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok, "Login_Test_Failed => failed to get proper error code");
			Assert.IsNotNull(_id1, "Login => failed to create accoutn id");

			error = BacksErrorCodes.Ok;
			AccountDto account = _service.Login("LoginTestUser", "somepwd", out error);
			Assert.AreEqual(error, BacksErrorCodes.AuthFailed, "Login => failed to get proper error code");
			Assert.IsNull(account, string.Format("Login_Test_Failed => get wrong account  errorCode = {0}", error));
		}

		[TestMethod]
		public void AddNewProject_Test()
		{
			_service.RepoInstance.DropCollection("Accounts");
			BacksErrorCodes error = BacksErrorCodes.Ok;
			string login = "LoginTestUser";
			string pwd = Guid.NewGuid().ToString("N");
			string _id1 = _service.SignIn("TestUser", "Pav1", "Test@gmail.com", login, pwd, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok, "Login_Test_With_Projects => failed to get proper error code");
			Assert.IsNotNull(_id1, "GetUserProject => failed to create accoutn id");

			//addProjects
			
			error = BacksErrorCodes.Ok;
			string projName = "TestProject1";
			ProjectDto proj = _service.AddNewProject(projName, _id1, out error);

			Assert.IsNotNull(proj, "AddNewProject_Test => failed to create project");
			Assert.IsNotNull(proj.Id, "AddNewProject_Test => failed to create project id");
		}


		[TestMethod]
		public void GetUserProject()
		{
			_service.RepoInstance.DropCollection("Accounts");
			BacksErrorCodes error = BacksErrorCodes.Ok;
			string login = "LoginTestUser";
			string pwd = Guid.NewGuid().ToString("N");
			string _id1 = _service.SignIn("TestUser", "Pav1", "Test@gmail.com", login, pwd, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok, "Login_Test_With_Projects => failed to get proper error code");
			Assert.IsNotNull(_id1, "GetUserProject => failed to create accoutn id");

			//addProjects
			var proj1 = new Project()
			{
				Name = "Test1",
				AppId = Guid.NewGuid().ToString("N"),
				ApiKeyAccess = Guid.NewGuid().ToString("N"),
				MasterKeyAccess = Guid.NewGuid().ToString("N"),
				CreatedAt = DateTime.UtcNow,
				P_AccountId = _id1

			};
			_service.RepoInstance.AddProject(proj1).Wait();

			Assert.AreNotEqual(proj1.Id, null, "GetUserProject=> failed to create Project");

			var proj2 = new Project()
			{
				Name = "Test2",
				AppId = Guid.NewGuid().ToString("N"),
				ApiKeyAccess = Guid.NewGuid().ToString("N"),
				MasterKeyAccess = Guid.NewGuid().ToString("N"),
				CreatedAt = DateTime.UtcNow,
				P_AccountId = _id1
			};
			_service.RepoInstance.AddProject(proj2).Wait();

			Assert.AreNotEqual(proj2.Id, null, "GetUserProject=> failed to create Project");
			//
			error = BacksErrorCodes.Ok;
			AccountDto account = _service.Login("LoginTestUser", pwd, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok, "GetUserProject => failed to get proper error code");
			Assert.AreEqual(account.Id, _id1, string.Format("GetUserProject => failed to login user errorCode = {0}", error));
			Assert.IsTrue(account.Projects.Any(), "GetUserProject => failed to get projects");
		}
	}
}
