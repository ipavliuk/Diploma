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
        #endregion

    }
}
