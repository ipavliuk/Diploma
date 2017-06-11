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

		public async Task<Tuple<BacksErrorCodes, ObjectsDto>> CreateEntity(string appId, string name, Dictionary<string, object> data/*, out BacksErrorCodes error*/)
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
					error = BacksErrorCodes.SystemError;
					return new Tuple<BacksErrorCodes, ObjectsDto>(error, null);
				}

				//Update schema
				var schema = await _repo.GetSchema(appId).ConfigureAwait(false);
				//_Schema, 
				if (schema.Id == null)
				{
					_log.Error("Error updating schema schema");
					error = BacksErrorCodes.SystemError;
					return new Tuple<BacksErrorCodes, ObjectsDto>(error, null);
				}

				var updatedData = schema.EntityColumnTypeMapping;
				updatedData[name] = _handler.GetSchema(data);

				await _repo.Update_Schema(appId,updatedData).ConfigureAwait(false); 

				obj = new ObjectsDto()
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

			return new Tuple<BacksErrorCodes, ObjectsDto>(error, obj);
		}

		public async Task<Tuple<BacksErrorCodes, ObjectsDto>> GetEntity(string appId, string name, string entityId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			var obj = new ObjectsDto();
			try
			{
				BacksObject entity = await _repo.GetEntity(appId, name, entityId).ConfigureAwait(false);
				if (entity == null)
				{
					error = BacksErrorCodes.EntityNotFound;
					return new Tuple<BacksErrorCodes, ObjectsDto>(error, null);
				}

				obj = new ObjectsDto()
				{
					Id = entity.Id,
					Name = entity.Name,
					Data = entity.Data,
					CreatedAt = entity.CreatedAt,
					UpdatedAt = entity.UpdatedAt
				}; //Mapper.Map<BacksObject, ObjectsDto>(entity);
			}
			catch (Exception e)
			{
				_log.Error("GetEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return new Tuple<BacksErrorCodes, ObjectsDto>(error, obj);
		}

		public async Task<Tuple<BacksErrorCodes, ObjectsDto>> UpdateEntity(string appId, string entityName, string entityId,
					Dictionary<string, object> data/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				BacksObject entity = await _repo.GetEntity(appId, entityName, entityId).ConfigureAwait(false);
				if (entity == null)
				{
					error = BacksErrorCodes.EntityNotFound;
					return new Tuple<BacksErrorCodes, ObjectsDto>(error, null);
				}


				var updatedData = entity.Data;
				foreach (var pair in data)
				{
					updatedData.CreateNewOrUpdateExisting(pair.Key, pair.Value);
				}

				await _repo.UpdateEntity(appId, entityName, entityId, updatedData).ConfigureAwait(false);
				
			}
			catch (Exception e)
			{
				_log.Error("UpdateEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return new Tuple<BacksErrorCodes, ObjectsDto>(error, new ObjectsDto() { UpdatedAt = DateTime.UtcNow });
		}

		public async Task<Tuple<BacksErrorCodes, List<ObjectsDto>>> QueryEntity(string appId, string entityId)
		{
			var error = BacksErrorCodes.Ok;
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

		public async Task<Tuple<BacksErrorCodes, List<ObjectsDto>>> GetEntities(string appId, string entityName)
		{
			var error = BacksErrorCodes.Ok;
			var objects = new List<ObjectsDto>();
			try
			{
				List<BacksObject> entities = await _repo.GetAllEntity(appId, entityName).ConfigureAwait(false);
				if (entities == null)
				{
					error = BacksErrorCodes.EntityNotFound;
					return new Tuple<BacksErrorCodes, List<ObjectsDto>>(error, objects);
				}

				
				foreach (var item in entities)
				{
					objects.Add(new ObjectsDto()
					{
						Id = item.Id,
						Name = item.Name,
						Data = item.Data,
						CreatedAt = item.CreatedAt,
						UpdatedAt = item.UpdatedAt
					});
				}

				//return new Tuple<BacksErrorCodes, List<ObjectsDto>>(error, objects);
			}
			catch (Exception e)
			{
				_log.Error("GetEntities exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return new Tuple<BacksErrorCodes, List<ObjectsDto>>(error, objects);
		}


		public async Task<Tuple<BacksErrorCodes, ObjectsDto>> RemoveEntity(string appId, string entityName, string entityId/*, out BacksErrorCodes error*/)
		{
			var error = BacksErrorCodes.Ok;
			try
			{
				await _repo.RemoveEntity(appId, entityName, entityId).ConfigureAwait(false);
				return new Tuple<BacksErrorCodes, ObjectsDto>(error, new ObjectsDto() { UpdatedAt = DateTime.UtcNow });
			}
			catch (Exception e)
			{
				_log.Error("RemoveEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return new Tuple<BacksErrorCodes, ObjectsDto>(error, null);
		}
	}

}
