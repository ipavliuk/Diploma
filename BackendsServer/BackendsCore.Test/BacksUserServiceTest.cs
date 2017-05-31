using System;
using Backends.Core;
using Backends.Core.Config;
using Backends.Core.DataEngine;
using Backends.Core.Model.BackAdminData;
using Backends.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendsCore.Test
{
	[TestClass]
	public class BacksUserServiceTest
	{
		private readonly BacksUsersService _service =
					new BacksUsersService(new BacksRepository(
								new Configuration
								{
									ConnectionString = "mongodb://localhost:27017",
									Database = "testBackends"
								}));
		[TestMethod]
		public void SignInUser()
		{

			BacksErrorCodes error = BacksErrorCodes.Ok;
			string appId = "592affe8ae1e7843883fd023";
			UserDto  dto = _service.SignUp(appId, "ig@pa.gmail.com", "testUser", "123456", out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok, 
					String.Format("SignInUser => failed to signin user with error => {0}", error));
			Assert.IsNotNull(dto, "SignInUser => failed to add signin user project");
			Assert.IsNotNull(dto.Id, "AddNewProject_Test => failed to signin project id");

			SessionDto sessionDto = _service.GetSession(appId, dto.SessionId, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
					String.Format("SignInUser => failed to get user session with error => {0}", error));
			Assert.IsNotNull(sessionDto, "SignInUser => failed to add signin user project");


		}

		[TestMethod]
		private void SessionExpiration_Test()
		{

		}

	}
}
