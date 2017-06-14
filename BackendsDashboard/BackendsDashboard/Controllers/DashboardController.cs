using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Backends.Core;
using Backends.Core.Model;
using Backends.Core.Model.BackAdminData;
using BackendsDashboard.Models;
using Newtonsoft.Json.Serialization;

namespace BackendsDashboard.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public async Task<ActionResult> Index(string id)
        {
	        if (SessionBag.Current.account != null)
	        {
		        AccountDto account = SessionBag.Current.account;
		        var service = BackendsServerManager.Instance.AdminService;
		        var tuple = await service.GetProject(id);
		        ProjectDto project = tuple.Item2;

				if (project != null)
		        {
					var modelNewProject = new NewProjectViewModel()
					{
						Id = project.Id,
						Name = project.Name,
						AppId = project.AppId,
						ApiKeyAccess = project.ApiKeyAccess,
						MasterKeyAccess = project.MasterKeyAccess,
						CreatedAt = project.CreatedAt.ToString(),
						EntitiesCount = project.Schema.EntityColumnTypeMapping.Count,
						UserLogedIn = project.UserCount,
						Schema = project.Schema
					};

			        SessionBag.Current.LoadedProjectModel = project;
			        SessionBag.Current.LoadedProjectViewModel = modelNewProject;
					return View("Index", new MainDashboardProjectModel() { CurrentProject = modelNewProject});
			        //return View("AddProjectResult", modelNewProject);
			        //return RedirectToAction("Index", "Home");
		        }
	        }

			return View();
        }

	    public async Task<ActionResult> EntitySelected(string name)
	    {
			ViewBag.ClassName = name;

			if (SessionBag.Current.account != null)
			{
				ViewBag.selectedEntity = name;
				ProjectDto project = SessionBag.Current.LoadedProjectModel;
				var modelEntityProject = new EntityDataViewModel();
				if (project != null)
				{

					var headers = project.Schema.EntityColumnTypeMapping[name].Select(it=> it.Key).ToList();

					if (name == "_Session")
					{
						var service = BackendsServerManager.Instance.UserService;
						var tuple = await service.GetSessions(project.Id);
						if (tuple.Item1 == BacksErrorCodes.Ok)
						{
							foreach (var sessionDto in tuple.Item2)
							{
								Type objectType = sessionDto.GetType();

								var properties = objectType.GetProperties();
								var dictionary = new Dictionary<string, object>();
								foreach (var header in headers)
								{
									var propertyInfo = properties.FirstOrDefault(pr => pr.Name == header);
									if (propertyInfo != null && propertyInfo.CanRead)
									{
										object firstValue = propertyInfo.GetValue(sessionDto);
										dictionary[header] = firstValue ?? "<null>";
									}
									else
									{
										if (sessionDto.Data.ContainsKey(header))
										{
											dictionary[header] = sessionDto.Data[header];
										}
										else
										{
											dictionary[header] = "<null>";
										}
									}
								}
								modelEntityProject.Data.Add(dictionary);
							}

							modelEntityProject.Keys = headers;
						}
					}
					else if (name == "_User" )
					{
						var service =  BackendsServerManager.Instance.UserService;
						var tuple = await service.GetUsers(project.Id, null);
						if (tuple.Item1 == BacksErrorCodes.Ok)
						{
							foreach (var userDto in tuple.Item2)
							{
								Type objectType = userDto.GetType();

								var properties = objectType.GetProperties();
								var dictionary = new Dictionary<string, object>();
								foreach (var header in headers)
								{
									var propertyInfo = properties.FirstOrDefault(pr => pr.Name == header);
									if (propertyInfo != null && propertyInfo.CanRead)
									{
										object firstValue = propertyInfo.GetValue(userDto);
										dictionary[header] = firstValue ?? "<null>";
									}
									else
									{
										if (userDto.Data.ContainsKey(header))
										{
											dictionary[header] = userDto.Data[header];
										}
										else
										{
											dictionary[header] = "<null>";
										}
									}
									
								}
								modelEntityProject.Data.Add(dictionary);
							}

							modelEntityProject.Keys = headers;
						}
					}else
					{
						var service = BackendsServerManager.Instance.DataService;
						var tuple = await service.GetEntities(project.Id, name);
						if (tuple.Item1 == BacksErrorCodes.Ok)
						{
							foreach (var objectsDto in tuple.Item2)
							{
								Type objectType = objectsDto.GetType();

								var properties = objectType.GetProperties();
								var dictionary = new Dictionary<string, object>();
								foreach (var header in headers)
								{
									var propertyInfo = properties.FirstOrDefault(pr => pr.Name == header);
									if (propertyInfo != null && propertyInfo.CanRead)
									{
										object firstValue = propertyInfo.GetValue(objectsDto);
										dictionary[header] = firstValue ?? "<null>";
									}
									else
									{
										if (objectsDto.Data.ContainsKey(header))
										{
											dictionary[header] = objectsDto.Data[header];
										}
										else
										{
											dictionary[header] = "<null>";
										}
									}
								}
								modelEntityProject.Data.Add(dictionary);
							}

							modelEntityProject.Keys = headers;
						}

					}




					//var modelNewProject = new NewProjectViewModel()
					//{
					//	Id = project.Id,
					//	Name = project.Name,
					//	AppId = project.AppId,
					//	ApiKeyAccess = project.ApiKeyAccess,
					//	MasterKeyAccess = project.MasterKeyAccess,
					//	CreatedAt = project.CreatedAt,
					//	EntitiesCount = project.Schema.EntityColumnTypeMapping.Count,
					//	UserLogedIn = project.UserCount,
					//	Schema = project.Schema
					//};


					return View("Index", new MainDashboardProjectModel() {SelecteEntity = modelEntityProject, CurrentProject = SessionBag.Current.LoadedProjectViewModel });
					//return View("AddProjectResult", modelNewProject);
					//return RedirectToAction("Index", "Home");
				}
			}

			return View();
		}
	}
}