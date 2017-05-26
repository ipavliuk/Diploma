using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backends.Core.DataEngine;
using Backends.Core.Config;
using Backends.Core.Model;

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
        [TestInitialize]
        public void Initialize()
        {
            _repo.DropDB("testBackends");
        }
        [TestCleanup]
        public void TestCleanup()
        {

        }

        // Init with drop all test data 
        [TestMethod]
		public void CreateAccount_Test()
		{
			_repo.AddAccount(new Account
								{
									FirstName = "Jack",
									LastName = "Genry",
									Email = "ig@mail.com",
									ScreenName = "dudu123",
									Pasword ="12345"
								}).Wait();
		}

	}
}
