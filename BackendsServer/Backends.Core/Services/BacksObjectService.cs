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
				cfg.CreateMap<BacksObjectService, ObjectsDto>();
			});
		}

		public void CreateEntity(string appId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{

			}
			catch (Exception e)
			{
				_log.Error("CreateEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
		}

		public ObjectsDto GetEntity(string appId, string entityId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{

			}
			catch (Exception e)
			{
				_log.Error("CreateEntity exception : ", e);
				error = BacksErrorCodes.SystemError;
			}
			return null;
		}

		public ObjectsDto UpdateEntity(string appId, string entityId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{

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

		public ObjectsDto RemoveEntity(string appId, string entityId, out BacksErrorCodes error)
		{
			error = BacksErrorCodes.Ok;
			try
			{

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
