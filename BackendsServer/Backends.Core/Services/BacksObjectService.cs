using AutoMapper;
using Backends.Core.DataEngine;
using Backends.Core.Model;
using Backends.Core.Model.BackAdminData;
using BackendsCommon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backends.Core.Extension;
using BackendsCommon.Types;

namespace Backends.Core.Services
{
	public class BacksObjectService
	{
		private readonly SchemaHandler _handler;
		private readonly IRepositoryAsync _repo;
		private ILog _log = new Log(typeof(BacksUsersService));

		public BacksObjectService(IRepositoryAsync repository)
		{
			_repo = repository;
			_handler = new SchemaHandler(_repo);
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<BacksObject, ObjectsDto>();
			});
		}

		public ObjectsDto CreateEntity(string appId, string name, Dictionary<string,object> data, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{

				var entity = new BacksObject()
				{
					AppId = appId,
					Data = data,
					CreatedAt = DateTime.UtcNow,
					Name = name
					
				};
				_repo.AddEntity(appId, entity).Wait();

				if (entity.Id == null)
				{
					error = BacksErrorCodes.SystemError;
					return null;
				}

				return new ObjectsDto()
				{
					Id = entity.Id,
					CreatedAt = entity.CreatedAt.Value
				};
			}
			catch (Exception e)
			{
				_log.Error("CreateEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}

			return null;
		}

		public ObjectsDto GetEntity(string appId, string name, string entityId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				BacksObject entity = _repo.GetEntity(appId, name, entityId).Result;
				if (entity == null)
				{
					error = BacksErrorCodes.EntityNotFound;
					return null;
				}

				var mappedEntity = Mapper.Map<BacksObject, ObjectsDto>(entity);

				return mappedEntity;

			}
			catch (Exception e)
			{
				_log.Error("GetEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return null;
		}

		public ObjectsDto UpdateEntity(string appId, string entityName, string entityId, 
					Dictionary<string, object> data, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{

				BacksObject entity = _repo.GetEntity(appId, entityName, entityId).Result;
				if (entity == null)
				{
					error = BacksErrorCodes.EntityNotFound;
					return null;
				}

				var updatedData = entity.Data;
				foreach (var pair in data)
				{
					updatedData.CreateNewOrUpdateExisting(pair.Key, pair.Value);
				}

				_repo.UpdateEntity(appId, entityName, entityId, updatedData);

				return new ObjectsDto() {UpdatedAt = DateTime.UtcNow};
			}
			catch (Exception e)
			{
				_log.Error("UpdateEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return null;
		}

		public ObjectsDto QueryEntity(string appId, string entityId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{

			}
			catch (Exception e)
			{
				_log.Error("QueryEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return null;
		}

		public ObjectsDto RemoveEntity(string appId, string entityName, string entityId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{
				_repo.RemoveEntity(appId, entityName, entityId).Wait();
			}
			catch (Exception e)
			{
				_log.Error("RemoveEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return null;
		}
	}
}
