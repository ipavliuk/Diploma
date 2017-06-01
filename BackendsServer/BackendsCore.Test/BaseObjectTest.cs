using System;
using System.Collections.Generic;
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

			var data = new Dictionary<string, object>()
			{
				{"score", 1337},
				{"playerName", "Sean Plott"},
				{"cheatMode", "False"}

			};
			var entityDto = _service.CreateEntity(appId, "GameResults", data,out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("CreateEntity_Test => failed to create entity; error => {0}", error));
			Assert.IsNotNull(entityDto, "CreateEntity_Test => failed to create entity");
			Assert.IsNotNull(entityDto.Id, "CreateEntity_Test => failed to to create entity id is null");
		}

		[TestMethod]
		public void GetEntity_Test()
		{
			BacksErrorCodes error = BacksErrorCodes.Ok;
			var data = new Dictionary<string, object>()
			{
				{"score", 1337},
				{"playerName", "Sean Plott"},
				{"cheatMode", "False"}

			};
			var entityDto = _service.CreateEntity(appId, "GameResults", data, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("GetEntity_Test => failed to create entity; error => {0}", error));
			Assert.IsNotNull(entityDto, "GetEntity_Test => failed to create entity");
			Assert.IsNotNull(entityDto.Id, "GetEntity_Test => failed to to create entity id is null");

			var entityDto1 = _service.GetEntity(appId, "GameResults", entityDto.Id, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("GetEntity_Test => failed to create entity; error => {0}", error));
			Assert.IsNotNull(entityDto1, "GetEntity_Test => failed to create entity");
			Assert.IsNotNull(entityDto1.Id, "GetEntity_Test => failed to to create entity id is null");

		}

		[TestMethod]
		public void UpdateEntity_Test()
		{
			BacksErrorCodes error = BacksErrorCodes.Ok;
			var data = new Dictionary<string, object>()
			{
				{"score", 1337},
				{"playerName", "Sean Plott"},
				{"cheatMode", "False"}

			};
			var entityDto = _service.CreateEntity(appId, "GameResults", data, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("UpdateEntity_Test => failed to create entity; error => {0}", error));
			Assert.IsNotNull(entityDto, "UpdateEntity_Test => failed to create entity");
			Assert.IsNotNull(entityDto.Id, "UpdateEntity_Test => failed to to create entity id is null");

			var dataNew = new Dictionary<string, object>()
			{
				{"score", 5000},
				{"playerName", "John"},
				{"cheatMode", "False"},
				{ "Loyalty", "VIP"}

			};

			var updatedDto = _service.UpdateEntity(appId, "GameResults", entityDto.Id, dataNew, out error);

			var entityDto1 = _service.GetEntity(appId, "GameResults", entityDto.Id, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("UpdateEntity_Test => failed to update entity; error => {0}", error));
			Assert.IsNotNull(entityDto1, "UpdateEntity_Test => failed to update entity");
			Assert.AreEqual(entityDto1.Data.Count, 4, "UpdateEntity_Test => failed to to update entity id is null");
			Assert.AreEqual(entityDto1.Data["score"], 5000,"UpdateEntity_Test => failed to to update entity id is null");
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
			var data = new Dictionary<string, object>()
			{
				{"score", 1337},
				{"playerName", "Sean Plott"},
				{"cheatMode", "False"}

			};
			var entityDto = _service.CreateEntity(appId, "GameResults", data, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("RemoveEntity_Test => failed to create entity; error => {0}", error));
			Assert.IsNotNull(entityDto, "RemoveEntity_Test => failed to create entity");
			Assert.IsNotNull(entityDto.Id, "RemoveEntity_Test => failed to to create entity id is null");

			var updatedDto = _service.RemoveEntity(appId, "GameResults", entityDto.Id, out error);

			var entityDto1 = _service.GetEntity(appId, "GameResults", entityDto.Id, out error);
			Assert.AreEqual(error, BacksErrorCodes.EntityNotFound,
				String.Format("RemoveEntity_Test => failed to remove entity; error => {0}", error));
			Assert.IsNull(entityDto1, "RemoveEntity_Test => failed to remove entity");
		}

		[TestMethod]
		public void QuesrySession_Test()
		{

			BacksErrorCodes error = BacksErrorCodes.Ok;
			var data = new Dictionary<string, object>()
			{
				{"score", 1337},
				{"playerName", "Sean Plott"},
				{"cheatMode", "False"}

			};
			var entityDto = _service.CreateEntity(appId, "GameResults", data, out error);
			Assert.AreEqual(error, BacksErrorCodes.Ok,
				String.Format("RemoveEntity_Test => failed to create entity; error => {0}", error));
			Assert.IsNotNull(entityDto, "RemoveEntity_Test => failed to create entity");
			Assert.IsNotNull(entityDto.Id, "RemoveEntity_Test => failed to to create entity id is null");

			_service.QueryEntity(appId, "GameResults", out error);
		}
	}
}
