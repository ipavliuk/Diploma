using System;
using System.Collections.Generic;
using Backends.Core;
using Backends.Core.Config;
using Backends.Core.DataEngine;
using Backends.Core.Model.BackAdminData;
using Backends.Core.Services;
using BackendsCommon.Types;
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

		private void SignUpWithLogin(string appId, out UserDto user, out SessionDto session)
		{
			BacksErrorCodes error = BacksErrorCodes.Ok;
			
			user = _service.SignUp(appId, "ig@pa.gmail.com", "testUser", "123456", out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok,
					String.Format("SignInUser => failed to signin user with error => {0}", error));
			Assert.IsNotNull(user, "SignInUser => failed to add signin user project");
			Assert.IsNotNull(user.Id, "AddNewProject_Test => failed to signin project id");

			session = _service.GetSession(appId, user.SessionId, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
					String.Format("SignInUser => failed to get user session with error => {0}", error));
			Assert.IsNotNull(session, "SignInUser => failed to add signin user project");
		}

		private void RemoveUser(string appId, string userId, out BacksErrorCodes error)
		{
			_service.RemoveUser(appId, userId, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				"ValidateSession_Test => Get user error Code failed");

			UserDto user1 = _service.GetUser(appId, userId, out error);
			Assert.AreEqual(error, BacksErrorCodes.UserIsNotFound,
				"ValidateSession_Test => Get user error Code failed");
			Assert.IsNull(user1, "ValidateSession_Test => Get user after delete");
		}

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
		public void Login_Test()
		{
			UserDto user = null;
			string appId = "592affe8ae1e7843883fd023";

			BacksErrorCodes error = BacksErrorCodes.Ok;

			user = _service.SignUp(appId, "ig@pa.gmail.com", "testUser", "123456", out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok,
					String.Format("Login_Test => failed to signin user with error => {0}", error));
			Assert.IsNotNull(user, "Login_Test => failed to add signin user project");
			Assert.IsNotNull(user.Id, "Login_Test => failed to signin project id");

			UserDto userLog =_service.Login(appId, "testUser", "123456", out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
					String.Format("Login_Test => failed to signin user with error => {0}", error));
			Assert.IsNotNull(userLog, "Login_Test => failed to add signin user project");
			Assert.IsNotNull(userLog.Id, "Login_Test => failed to signin project id");


			RemoveUser(appId, user.Id, out error);
		}


		[TestMethod]
		public void LogoutWithUserObject_Test()
		{
			UserDto user = null;
			SessionDto session = null;
			string appId = "592affe8ae1e7843883fd023";
			SignUpWithLogin(appId, out user, out session);
			BacksErrorCodes error = BacksErrorCodes.Ok;

			_service.Logout(appId, user.Id, session.Token, out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok,
					String.Format("Login_Test => failed to logout user with error => {0}", error));
		}


		[TestMethod]
		public void Logout_Test()
		{
			UserDto user = null;
			string appId = "592affe8ae1e7843883fd023";
			BacksErrorCodes error = BacksErrorCodes.Ok;
			user = _service.SignUp(appId, "ig@pa.gmail.com", "testUser", "123456", out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
			String.Format("Login_Test => failed to signin user with error => {0}", error));
			Assert.IsNotNull(user, "Login_Test => failed to add signin user project");
			Assert.IsNotNull(user.Id, "Login_Test => failed to signin project id");

			_service.Logout(appId, null, user.SessionId,out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
					String.Format("Logout_Test => failed to logiut user with error => {0}", error));
		}

		[TestMethod]
		public void RemoveSession_Test()
		{
			UserDto user = null;
			SessionDto session = null;
			string appId = "592affe8ae1e7843883fd023";
			SignUpWithLogin(appId, out user, out session);

			BacksErrorCodes error = BacksErrorCodes.Ok;
			_service.RemoveSession(appId, session.Token, out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("Login_Test => failed to logout user with error => {0}", error));
			
		}
		[TestMethod]
		public void PasswordReset_Test()
		{
			UserDto user = null;
			string appId = "592affe8ae1e7843883fd023";
			BacksErrorCodes error = BacksErrorCodes.Ok;
			user = _service.SignUp(appId, "ig@pa.gmail.com", "testUser", "123456", out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("PasswordReset_Test => failed to signin user with error => {0}", error));
			Assert.IsNotNull(user, "PasswordReset_Test => failed to add signin user project");
			Assert.IsNotNull(user.Id, "PasswordReset_Test => failed to signin project id");

			_service.PasswrodReset(appId, user.Id, "6542135", out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("PasswordReset_Test => failed to reset pwdwith error => {0}", error));

			UserDto userLog = _service.Login(appId, "testUser", "6542135", out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("PasswordReset_Test => failed to signin user with error => {0}", error));
			Assert.IsNotNull(userLog, "PasswordReset_Test => failed to add signin");
			Assert.IsNotNull(userLog.Id, "PasswordReset_Test => failed to signin ");

		}
		[TestMethod]
		public void QuesrySession_Test()
		{
			_service.QueryUserData();
		}

		[TestMethod]
		public void UpdateSession_Test()
		{
			UserDto user = null;
			SessionDto session = null;
			string appId = "592affe8ae1e7843883fd023";
			SignUpWithLogin(appId, out user, out session);

			BacksErrorCodes error = BacksErrorCodes.Ok;

			var fields = new Dictionary<string, object>()
			{
				{"Role", "Admin"},
				{"IsExtandable", "true" }

			};
			_service.UpdateSession(appId, session.Token,fields, out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("UpdateSession_Test => failed to update user without errors => {0}", error));

			SessionDto session1 = _service.GetSession(appId, session.Token, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				"UpdateSession_Test => Get user error Code failed");
			Assert.IsNotNull(session1, "UpdateSession_Test => Get user after delete");
			Assert.IsNotNull(session1.Data, "UpdateSession_Test => Get user after delete");
			Assert.AreEqual(session1.Data.Count, 2, "UpdateSession_Test => Get user after delete");
		}

		[TestMethod]
		public void GetUserSession_Test()
		{
			UserDto user = null;
			SessionDto session = null;
			string appId = "592affe8ae1e7843883fd023";
			SignUpWithLogin(appId, out user, out session);

			BacksErrorCodes error = BacksErrorCodes.Ok;


			List<SessionDto> sessions = _service.GetUserSession(appId, user.Id, out error);

			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("UpdateSession_Test => failed to update user without errors => {0}", error));
			
			
			Assert.IsNotNull(sessions, "GetUserSession_Test => Get user after delete");
			
		}


		[TestMethod]
		public void UpdateUser_Test()
		{
			UserDto user = null;
			string appId = "592affe8ae1e7843883fd023";
			BacksErrorCodes error = BacksErrorCodes.Ok;
			user = _service.SignUp(appId, "ig@pa.gmail.com", "testUser", "123456", out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("UpdateUser_Test => failed to signin user with error => {0}", error));
			Assert.IsNotNull(user, "UpdateUser_Test => failed to add signin user project");
			Assert.IsNotNull(user.Id, "UpdateUser_Test => failed to signin project id");

			var fields = new Dictionary<string, object>()
			{
				{"Phone", "555-555"},
				{"ZIP", "20110" }

			};
			_service.UpdateUser(appId, user.Id, user.SessionId, fields, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("UpdateUser_Test => failed to update user without errors => {0}", error));

			UserDto user1 = _service.GetUser(appId, user.Id, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				"UpdateUser_Test => Get user error Code failed");
			Assert.IsNotNull(user1, "UpdateUser_Test => Get user after delete");
			Assert.IsNotNull(user1.Data, "UpdateUser_Test => Get user after delete");
			Assert.AreEqual(user1.Data.Count, 2,"UpdateUser_Test => Get user after delete");
		}

		[TestMethod]
		public void ValidateSession_Test()
		{
			UserDto user = null;
			SessionDto session = null;
			string appId = "592affe8ae1e7843883fd023";
			SignUpWithLogin(appId, out user, out session);
			BacksErrorCodes error = BacksErrorCodes.Ok;
			Assert.IsTrue(_service.ValidateSession(appId, user.Id, session.Token, out error),
				"ValidateSession_Test => failed to validate signin user and session ");

			Assert.IsTrue(_service.ValidateSession(appId, null, session.Token, out error),
				"ValidateSession_Test => failed to validate signin session without user");

			RemoveUser(appId, user.Id, out error);

		}

	}
}
