using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backends.Core.DataEngine;
using Backends.Core.Extension;
using Backends.Core.Model.BackAdminData;
using BackendsCommon.Logging;
using BackendsCommon.Types;

namespace Backends.Core.Services
{
	public class BacksObjectServiceAsync
	{
		private readonly SchemaHandler _handler;
		private readonly IRepositoryAsync _repo;
		private ILog _log = new Log(typeof(BacksUsersService));

		public BacksObjectServiceAsync(IRepositoryAsync repository)
		{
			_repo = repository;
			_handler = new SchemaHandler(_repo);
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<BacksObject, ObjectsDto>();
			});
		}

		public async Task<ObjectsDto> CreateEntity(string appId, string name, Dictionary<string, object> data/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			var obj = new ObjectsDto();
			try
			{

				var entity = new BacksObject()
				{
					AppId = appId,
					Data = data,
					CreatedAt = DateTime.UtcNow,
					Name = name

				};
				await _repo.AddEntity(appId, entity).ConfigureAwait(false);

				if (entity.Id == null)
				{
					obj.Error = BacksErrorCodes.SystemError;
					return obj;
				}

				return new ObjectsDto()
				{
					Error = BacksErrorCodes.Ok,
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

		public async Task<ObjectsDto> GetEntity(string appId, string name, string entityId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				BacksObject entity = await _repo.GetEntity(appId, name, entityId).ConfigureAwait(false);
				if (entity == null)
				{
					return new ObjectsDto() { Error = BacksErrorCodes.EntityNotFound }; ;
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

		public async Task<ObjectsDto> UpdateEntity(string appId, string entityName, string entityId,
					Dictionary<string, object> data/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{

				BacksObject entity = await _repo.GetEntity(appId, entityName, entityId).ConfigureAwait(false);
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

				await _repo.UpdateEntity(appId, entityName, entityId, updatedData).ConfigureAwait(false);

				return new ObjectsDto() { UpdatedAt = DateTime.UtcNow };
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

		public async Task<ObjectsDto> RemoveEntity(string appId, string entityName, string entityId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				await  _repo.RemoveEntity(appId, entityName, entityId).ConfigureAwait(false);
				return new ObjectsDto() { UpdatedAt = DateTime.UtcNow };
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
