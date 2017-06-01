using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backends.Core.DataEngine;
using Backends.Core.Config;
using Backends.Core.Model;
using BackendsCommon.Types;
using BackendsCommon.Types.BacksModel;
using Backends.Core.Services;
using Backends.Core.Extension;

namespace BackendsCore.Test
{
	[TestClass]
	public class RepositoryTest
	{
		private IRepositoryAsync _repo = new BacksRepository(
								new Configuration
								{
									ConnectionString = "mongodb://localhost:27017",
									Database = "testBackends"
								});

		private Account _acc = new Account
		{
			FirstName = "Jack",
			LastName = "Genry",
			Email = "ig@mail.com",
			ScreenName = "dudu123",
			Pasword = "12345"
		};

		private Project _proj = new Project()
		{
			AppId = Guid.NewGuid().ToString("N"),
			ApiKeyAccess = Guid.NewGuid().ToString("N"),
			MasterKeyAccess = Guid.NewGuid().ToString("N"),
			CreatedAt =  DateTime.UtcNow
		};

		[TestInitialize]
		public void Initialize()
		{
			//_repo.DropDB("testBackends");
		}
		[TestCleanup]
		public void TestCleanup()
		{

		}

		// Init with drop all test data 
		[TestMethod]
		public void CreateAccount_Test()
		{
			_repo.AddAccount(_acc).Wait();

			Assert.AreNotEqual(_acc.Id, null, "CreateAccount_Test=> failed create Account");
		}

		
		[TestMethod]
		public void GetAccount_Test()
		{
			Account acc = _repo.GetAllAccounts().Result.FirstOrDefault();
			Assert.IsNotNull(acc, "GetAccounts failed to get object");

			Account accNew = _repo.GetAccount(acc.Id).Result;
			Assert.IsNotNull(accNew, "Get Account failed to get object");
			Assert.AreEqual(acc.Id, accNew.Id, "Get Account get different objetc");
		}

		[TestMethod]
		public void UpdateAccount_Test()
		{
			Account acc = _repo.GetAllAccounts().Result.FirstOrDefault();
			Assert.IsNotNull(acc, "UpdateAccount_Test=>GetAllAccounts failed to get object");

			string NewEmail = "newmail@gmail.com";

			_repo.UpdateAccount(acc.Id, acc.Pasword, acc.FirstName, acc.LastName, NewEmail).Wait();

			Account accNew = _repo.GetAccount(acc.Id).Result;
			Assert.IsNotNull(accNew, "UpdateAccount_Test=>Get Account failed to get object");
			Assert.AreEqual(NewEmail, accNew.Email, "UpdateAccount_Test=>Get Account get different object");
			Assert.AreEqual(acc.Id, accNew.Id, "UpdateAccount_Test=>Get Account get different objetc");
		}

		[TestMethod]
		public void RemoveAccount_Test()
		{
			var testAcc = new Account
			{
				FirstName = "test",
				LastName = "JaskTest",
				Email = "test@mail.com",
				ScreenName = "testAcc",
				Pasword = "12345"
			};

			_repo.AddAccount(testAcc).Wait();
			Assert.AreNotEqual(testAcc.Id, null, "RemoveAccount_Test=> failed create Account");

			_repo.RemoveAccount(testAcc.Id).Wait();

			Account accNew = _repo.GetAccount(testAcc.Id).Result;
			Assert.IsNull(accNew, "RemoveAccount_Test => Remove failed for object");

		}

		[TestMethod]
		public void FindDuplicates_Test()
		{
			var testAcc = new Account
			{
				FirstName = "test",
				LastName = "JaskTest",
				Email = "test@mail.com",
				ScreenName = "testAccDublicate",
				Pasword = "12345"
			};

			_repo.AddAccount(testAcc).Wait();
			Assert.AreNotEqual(testAcc.Id, null, "FindDuplicates_Test=> failed create Account");

			var testAcc1 = new Account
			{
				FirstName = "test",
				LastName = "JaskTest",
				Email = "test@mail.com",
				ScreenName = "testAccDublicate1",
				Pasword = "12345"
			};


			var items = _repo.FindDuplicates(testAcc1.ScreenName).Result;

			Assert.IsFalse(items.Any());
		}

		[TestMethod]
		public void Login_Test()
		{
			var testAcc = new Account
			{
				FirstName = "testlogin",
				LastName = "JaskTest",
				Email = "test@mail.com",
				ScreenName = "testAccDublicate" + Guid.NewGuid().ToString("N"),
				Pasword = Guid.NewGuid().ToString("N")
			};

			_repo.AddAccount(testAcc).Wait();
			Assert.AreNotEqual(testAcc.Id, null, "FindDuplicates_Test=> failed create Account");

			var account = _repo.Login(testAcc.ScreenName,testAcc.Pasword).Result;

			Assert.IsNotNull(account, "Login_Test failed to get object");
			//Assert.AreEqual(account.Id, testAcc.Id, "Login_Test get different objetc");
		}
		[TestMethod]
		public void AddProject_Test_Test()
		{
			var testAcc = new Account
			{
				FirstName = "testProjeAccount",
				LastName = "JaskTestProjetc",
				Email = "testProj@mail.com",
				ScreenName = "testAccProj",
				Pasword = "12345"
			};

			_repo.AddAccount(testAcc).Wait();
			Assert.AreNotEqual(testAcc.Id, null, "AddProject_Test_Test=> failed to create Account");

			_proj.P_AccountId = testAcc.Id;
			_repo.AddProject(_proj).Wait();

			Assert.AreNotEqual(_proj.Id, null, "AddProject_Test_Test=> failed to create Project");

		}
		[TestMethod]
		public void GetProject_Test()
		{
			var testAcc = new Account
			{
				FirstName = "testProjeAccount",
				LastName = "JaskTestProjetc",
				Email = "testProj@mail.com",
				ScreenName = "testAccProj",
				Pasword = "12345"
			};

			_repo.AddAccount(testAcc).Wait();
			Assert.AreNotEqual(testAcc.Id, null, "GetProject_Test=> failed to create Account");

			_proj.P_AccountId = testAcc.Id;
			_repo.AddProject(_proj).Wait();

			Assert.AreNotEqual(_proj.Id, null, "GetProject_Test=> failed to create Project");

			Project projNew = _repo.GetProject(_proj.Id).Result;
			Assert.IsNotNull(projNew, "GetProject_Test failed to get object");
			Assert.AreEqual(_proj.Id, projNew.Id, "GetProject_Test get different object");
		}
		[TestMethod]
		public void UpdateProject_Test()
		{
			Project proj = _repo.GetAllProject().Result.FirstOrDefault();
			Assert.IsNotNull(proj, "UpdateProject_Test=>GetAllAccounts failed to get object");

			//string NewEmail = "newmail@gmail.com";

			//_repo.UpdateAccount(acc.Id, acc.Pasword, acc.FirstName, acc.LastName, NewEmail).Wait();

			//Account accNew = _repo.GetAccount(acc.Id).Result;
			//Assert.IsNotNull(accNew, "UpdateAccount_Test=>Get Account failed to get object");
			//Assert.AreEqual(NewEmail, accNew.Email, "UpdateAccount_Test=>Get Account get different object");
			//Assert.AreEqual(acc.Id, accNew.Id, "UpdateAccount_Test=>Get Account get different objetc");
		}

		[TestMethod]
		public void RemoveProject_Test()
		{
			Project proj = _repo.GetAllProject().Result.FirstOrDefault();
			Assert.IsNotNull(proj, "RemoveProject_Test=>GetAllAccounts failed to get object");

			Assert.AreNotEqual(proj.Id, null, "RemoveProject_Test=> failed to create Project");

			_repo.RemoveProject(proj.Id).Wait();

			Project projNew = _repo.GetProject(proj.Id).Result;
			Assert.IsNull(projNew, "RemoveProject_Test => Remove failed for object");
		}
		[TestMethod]
		public void GetAccountProjects_Test()
		{
			var testAcc = new Account
			{
				FirstName = "testProjeAccount",
				LastName = "JaskTestProjetc",
				Email = "testProj@mail.com",
				ScreenName = "testAccProj",
				Pasword = "12345"
			};

			_repo.AddAccount(testAcc).Wait();
			Assert.AreNotEqual(testAcc.Id, null, "GetProject_Test=> failed to create Account");

			var proj1 = new Project()
			{
				AppId = Guid.NewGuid().ToString("N"),
				ApiKeyAccess = Guid.NewGuid().ToString("N"),
				MasterKeyAccess = Guid.NewGuid().ToString("N"),
				CreatedAt = DateTime.UtcNow,
				P_AccountId = testAcc.Id
			}; 

			_repo.AddProject(proj1).Wait();
			Assert.AreNotEqual(proj1.Id, null, "GetAccountProjects_Test=> failed to create Project1");

			var proj2 = new Project()
			{
				AppId = Guid.NewGuid().ToString("N"),
				ApiKeyAccess = Guid.NewGuid().ToString("N"),
				MasterKeyAccess = Guid.NewGuid().ToString("N"),
				CreatedAt = DateTime.UtcNow,
				P_AccountId = testAcc.Id
			};

			_repo.AddProject(proj2).Wait();
			Assert.AreNotEqual(proj2.Id, null, "GetAccountProjects_Test=> failed to create Project2");


			List<Project> projs = _repo.GetAccountProjects(testAcc.Id).Result.ToList();
			Assert.IsNotNull(projs, "GetAccountProjects_Test failed to get object");
			Assert.IsTrue(projs.Count==2, "GetAccountProjects_Test get different object");
			
		}
        #region _Schema test
        private readonly BacksProjectSchema _schema = new BacksProjectSchema()
        {
            EntityColumnTypeMapping = new Dictionary<string, EntitiesSchema>()
                    {
                        {
                            "_User", new EntitiesSchema()
                            {
                                ColumnTypeMapping = new Dictionary<string, string>()
                                {
                                    { "_id", BacksDataType.BString},
                                    { "userName", BacksDataType.BString},
                                    { "paswword", BacksDataType.BString},
                                    { "createdAt", BacksDataType.BTime},
                                    { "updatedAt", BacksDataType.BTime},
                                    { "userData", BacksDataType.BObject}
                                }
                            }
                        },
                        {
                            "_Session", new EntitiesSchema()
                            {
                                ColumnTypeMapping = new Dictionary<string, string>()
                                {
                                    { "_id", BacksDataType.BString},
                                    { "sessionToken", BacksDataType.BString},
                                    { "createdAt", BacksDataType.BTime},
                                    { "updatedAt", BacksDataType.BTime},
                                    { "expiredAt", BacksDataType.BTime},
                                    { "installationId", BacksDataType.BString},
                                    { "sessionData", BacksDataType.BString},
                                    { "previleges", BacksDataType.BBoolean},
                                    { "_p_user", BacksDataType.BPointer}
                                }
                            }
                        },
                        {
                            "_Roles", new EntitiesSchema()
                            {
                                ColumnTypeMapping = new Dictionary<string, string>()
                                {
                                    { "_id", BacksDataType.BString},
                                    { "name", BacksDataType.BString},
                                    { "paswword", BacksDataType.BString},
                                    { "createdAt", BacksDataType.BTime},
                                    { "updatedAt", BacksDataType.BTime},
                                    { "userData", BacksDataType.BObject}
                                }
                            }
                        }

                    }
        };

        [TestMethod]
        public void Add_Schema_Test()
        {
            Project proj = _repo.GetAllProject().Result.FirstOrDefault();
            Assert.IsNotNull(proj, "RemoveProject_Test=>Add_Schema_Test failed to get object");

            //create Schema
            _schema.AppId = proj.Id;

            _repo.Add_Schema(_schema).Wait();
            Assert.AreNotEqual(_schema.Id, null, "Add_Schema_Test=> failed to create porject's _schema");
        }

        [TestMethod]
        public void Get_Schema_Test()
        {
            _repo.DropCollection("_Schema");
            Project proj = _repo.GetAllProject().Result.FirstOrDefault();
            Assert.IsNotNull(proj, "RemoveProject_Test=>Add_Schema_Test failed to get object");

            //create Schema
            _schema.AppId = proj.Id;

            _repo.Add_Schema(_schema).Wait();
            Assert.AreNotEqual(_schema.Id, null, "Get_Schema_Test=> failed to create porject's _schema");

            BacksProjectSchema schema = _repo.GetSchema(proj.Id).Result;

            Assert.IsNotNull(schema, "Get_Schema_Test failed to get schema");
            Assert.AreEqual(schema.Id, _schema.Id, "Get_Schema_Test get different object");
        }


		/*[TestMethod]
		public void ValidateSchema_Test()
		{
			_repo.DropCollection("_Schema");
			Project proj = _repo.GetAllProject().Result.FirstOrDefault();
			Assert.IsNotNull(proj, "RemoveProject_Test=>Add_Schema_Test failed to get object");

			//create Schema
			_schema.AppId = proj.Id;

			_repo.Add_Schema(_schema).Wait();
			Assert.AreNotEqual(_schema.Id, null, "Get_Schema_Test=> failed to create porject's _schema");

			var handler = new SchemaHandler(null);

			var user = new BacksUsers()
			{
				Data = new 
			};
			handler.IsSchemaValid<>
		}*/

		private void CreateAccountProject(out string accId, out string projId)
		{
			var testAcc = new Account
			{
				FirstName = "testProjeAccount",
				LastName = "JaskTestProjetc",
				Email = "testProj@mail.com",
				ScreenName = "testAccProj",
				Pasword = "12345"
			};

			_repo.AddAccount(testAcc).Wait();
			Assert.AreNotEqual(testAcc.Id, null, "GetProject_Test=> failed to create Account");
			accId = testAcc.Id;

			var proj1 = new Project()
			{
				AppId = Guid.NewGuid().ToString("N"),
				ApiKeyAccess = Guid.NewGuid().ToString("N"),
				MasterKeyAccess = Guid.NewGuid().ToString("N"),
				CreatedAt = DateTime.UtcNow,
				P_AccountId = testAcc.Id
			};

			_repo.AddProject(proj1).Wait();
			Assert.AreNotEqual(proj1.Id, null, "GetAccountProjects_Test=> failed to create Project1");
			projId = proj1.Id;
		}

		[TestMethod]
		public void AddUser_Test()
		{
			string accId, projId;
			CreateAccountProject(out accId, out projId);
			BacksUsers user = new BacksUsers()
			{
				UserName = "mccparker",
				Password ="123456",
				Email= "ig@mail.com",
				AppId = projId,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			_repo.AddUser(projId, user).Wait();
			Assert.AreNotEqual(user.Id, null, "AddUser_Test=> failed create User");
		}

		[TestMethod]
		public void GetUser_Test()
		{
			string accId, projId;
			CreateAccountProject(out accId, out projId);
			BacksUsers user = new BacksUsers()
			{
				UserName = "mccparker",
				Password = "123456",
				Email = "ig@mail.com",
				AppId = projId,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			_repo.AddUser(projId, user).Wait();
			Assert.AreNotEqual(user.Id, null, "GetUser_Test=> failed create User");

			BacksUsers userGet = _repo.GetUser(projId, user.Id).Result;
			Assert.AreNotEqual(userGet.Id, null, "GetUser_Test=> failed get User");
			Assert.IsNotNull(userGet.Id, null, "GetUser_Test=> failed get User");
		}
		[TestMethod]
		public void UpdateUser_WithNewFields_Test()
		{
			string accId, projId;
			CreateAccountProject(out accId, out projId);
			BacksUsers user = new BacksUsers()
			{
				UserName = "mccparker",
				Password = "123456",
				Email = "ig@mail.com",
				AppId = projId,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			_repo.AddUser(projId, user).Wait();
			Assert.AreNotEqual(user.Id, null, "UpdateUser_Test=> failed create User");

			_repo.UpdateUserData(projId, user.Id, new Dictionary<string, object>()
			{
				{"phone", "555-666"}
			});

			BacksUsers userGet = _repo.GetUser(projId, user.Id).Result;
			Assert.IsNotNull(userGet.Data, "UpdateUser_Test=> data is not updated");
			Assert.AreEqual(userGet.Data["phone"], "555-666", "UpdateUser_Test=> phone data is not updated");
			var newDict = new Dictionary<string, object>()
			{
				{"location", "Ukraine"},
				{"phone", "666-555"},
			};

			var oldDict = userGet.Data;
			foreach (var pair in newDict)
			{
				oldDict.CreateNewOrUpdateExisting(pair.Key, pair.Value);
			}
			
			_repo.UpdateUserData(projId, user.Id, oldDict);

			userGet = _repo.GetUser(projId, user.Id).Result;
			Assert.AreEqual(userGet.Data["location"], "Ukraine", "UpdateUser_Test=> phone data is not updated");
			Assert.AreEqual(userGet.Data["phone"], "666-555", "UpdateUser_Test=> phone data is not updated");

			_repo.UpdateUserPasswrod(projId, user.Id, "654321");
			userGet = _repo.GetUser(projId, user.Id).Result;
			Assert.AreEqual(userGet.Password, "654321", "UpdateUser_Test=> password is not updated");

		}

		[TestMethod]
		public void RemoveUser_Test()
		{
			string accId, projId;
			CreateAccountProject(out accId, out projId);
			BacksUsers user = new BacksUsers()
			{
				UserName = "mccparker",
				Password = "123456",
				Email = "ig@mail.com",
				AppId = projId,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			_repo.AddUser(projId, user).Wait();
			Assert.AreNotEqual(user.Id, null, "UpdateUser_Test=> failed create User");

			_repo.RemoveUser(projId, user.Id).Wait();

			var userNew = _repo.GetUser(projId, user.Id).Result;
			Assert.IsNull(userNew, "RemoveUser_Test => Remove failed for object");
		}

		[TestMethod]
		public void CreateSession_Test()
		{
			
			BacksSessions sessionOut;
			BacksUsers userOut;
			GenerateUserSessions(out sessionOut, out userOut);
		}

		[TestMethod]
		public void GetSession_Test()
		{
			string accId, projId;
			CreateAccountProject(out accId, out projId);
			BacksUsers user = new BacksUsers()
			{
				UserName = "mccparker",
				Password = "123456",
				Email = "ig@mail.com",
				AppId = projId,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			_repo.AddUser(projId, user).Wait();
			Assert.AreNotEqual(user.Id, null, "GetSession_Test=> failed create User");

			var session = new BacksSessions()
			{
				PUser = user.Id,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddMinutes(30),
				AppId = projId
			};

			_repo.AddSession(projId, session).Wait();
			Assert.AreNotEqual(session.Id, null, "GetSession_Test=> failed create User session");

			BacksSessions sessionNew = _repo.GetSession(projId, session.Id).Result;
			Assert.IsNotNull(sessionNew, "GetSession_Test=> failed get User");
			Assert.AreEqual(sessionNew.Id, session.Id, "GetSession_Test=> failed get User");
			
		}
		private void GenerateUserSessions(out BacksSessions sessionOut, out BacksUsers userOut)
		{
			string accId, projId;
			CreateAccountProject(out accId, out projId);
			BacksUsers user = new BacksUsers()
			{
				UserName = "mccparker",
				Password = "123456",
				Email = "ig@mail.com",
				AppId = projId,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			_repo.AddUser(projId, user).Wait();
			Assert.AreNotEqual(user.Id, null, "CreateSession_Test=> failed create User");

			var session = new BacksSessions()
			{
				PUser = user.Id,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddMinutes(30),
				AppId = projId
			};

			_repo.AddSession(projId, session).Wait();
			Assert.AreNotEqual(session.Id, null, "CreateSession_Test=> failed create User session");
			sessionOut = session;
			userOut = user;
		}

		[TestMethod]
		public void UpdateSession_Test()
		{
			BacksSessions sessionOut;
			BacksUsers userOut;
			GenerateUserSessions(out sessionOut, out userOut);

			var newDict = new Dictionary<string, object>()
			{
				{"location", "Ukraine"},
				{"phone", "666-555"},
			};

			var oldDict = sessionOut.Data;
			foreach (var pair in newDict)
			{
				oldDict.CreateNewOrUpdateExisting(pair.Key, pair.Value);
			}

			_repo.UpdateSession(userOut.AppId, sessionOut.Id, oldDict).Wait();

			var session1 = _repo.GetSession(userOut.AppId, sessionOut.Id).Result;
			Assert.AreEqual(sessionOut.Data["location"], "Ukraine", "UpdateSession_Test=> phone data is not updated");
			Assert.AreEqual(sessionOut.Data["phone"], "666-555", "UpdateSession_Test=> phone data is not updated");
		}

		[TestMethod]
		public void GetAllUserSession_Test()
		{
			BacksSessions sessionOut; 
			BacksUsers userOut;
			GenerateUserSessions(out sessionOut, out userOut);
			var session1 = new BacksSessions()
			{
				PUser = userOut.Id,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddMinutes(30),
				AppId = userOut.AppId
			};

			_repo.AddSession(userOut.AppId, session1).Wait();
			Assert.AreNotEqual(session1.Id, null, "GetAllUserSession_Test=> failed create User session");

			var sessions = _repo.GetAllSessions(userOut.AppId, userOut.Id).Result;
			Assert.IsNotNull(sessions, "GetAllUserSession_Test=> failed create User session");
			

		}

		[TestMethod]
		public void RemoveSession_Test()
		{
			BacksSessions sessionOut;
			BacksUsers userOut;
			GenerateUserSessions(out sessionOut, out userOut);

			_repo.RemoveSession(userOut.AppId, sessionOut.Id).Wait();

			var session1 = _repo.GetSession(userOut.AppId, sessionOut.Id).Result;
			Assert.IsNull(session1, "RemoveSession_Test => Remove failed for object");
		}



		[TestMethod]
		public void CreateEnttity_Test()
		{
			string accId, projId;
			CreateAccountProject(out accId, out projId);

			var entity = new BacksObject()
			{
				AppId = projId,
				Name = "GameScore",
				CreatedAt = DateTime.Now,
				Data = new Dictionary<string, object>()
				{
					{"score", 1337},
					{"playerName","Jack wilkins"},
					{"gameMode", "play"}
				}
			};
			_repo.AddEntity(projId, entity).Wait();
			Assert.AreNotEqual(entity.Id, null, "CreateEnttity_Test=> failed create Account");

			BacksObject entity1 = _repo.GetEntity(projId, "GameScore", entity.Id).Result;
			Assert.AreNotEqual(entity1.Id, null, "CreateEnttity_Test=> failed get Entity");
			Assert.AreEqual(entity1.Id, entity.Id, "CreateEnttity_Test=> get wrong Entity");

		}

		[TestMethod]
		public void GetUnvalidEnttity_Test()
		{
			string accId, projId;
			CreateAccountProject(out accId, out projId);

			BacksObject entity = _repo.GetEntity(projId, "GameScore", "592efa1972403627583425d4").Result;
			Assert.IsNull(entity, "GetUnvalidEnttity_Test=> entity is prsetnt!!@!");
			
		}

		[TestMethod]
		public void UpdateAndRemoveEnttity_Test()
		{
			string accId, projId;
			CreateAccountProject(out accId, out projId);

			var entity = new BacksObject()
			{
				AppId = projId,
				Name = "GameScore",
				CreatedAt = DateTime.Now,
				Data = new Dictionary<string, object>()
				{
					{"score", 1337},
					{"playerName","Jack wilkins"},
					{"gameMode", "play"}
				}
			};

			_repo.AddEntity(projId, entity).Wait();
			Assert.AreNotEqual(entity.Id, null, "UpdateAndRemoveEnttity_Test=> failed add entity");


			var newDict = new Dictionary<string, object>()
			{
				{"score", 78888},
				{"hit", 15}
			};

			var oldDict = entity.Data;
			foreach (var pair in newDict)
			{
				oldDict.CreateNewOrUpdateExisting(pair.Key, pair.Value);
			}

			_repo.UpdateEntity(projId, "GameScore", entity.Id, oldDict);

			BacksObject entity1 = _repo.GetEntity(projId, "GameScore", entity.Id).Result;
			Assert.AreNotEqual(entity1.Id, null, "UpdateAndRemoveEnttity_Test=> failed get Entity");
			Assert.AreEqual(entity1.Id, entity.Id, "UpdateAndRemoveEnttity_Test=> get wrong Entity");

			_repo.RemoveEntity(projId, "GameScore", entity.Id).Wait();

			BacksObject entity2 = _repo.GetEntity(projId, "GameScore", entity.Id).Result;
			Assert.AreEqual(entity2, null, "UpdateAndRemoveEnttity_Test=> Entity wasn't deleted");

		}
		#endregion

	}
}
