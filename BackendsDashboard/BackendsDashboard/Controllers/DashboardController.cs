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
						CreatedAt = project.CreatedAt,
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
				ProjectDto project = SessionBag.Current.LoadedProjectModel;
				var modelEntityProject = new EntityDataViewModel();
				if (project != null)
				{

					var headers = project.Schema.EntityColumnTypeMapping[name].Select(it=> it.Key).ToList();

					if (name != "_User" || name != "_Sessions")
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
										dictionary[header] = firstValue;
									}
									else
									{
										if (objectsDto.Data.ContainsKey(header))
										{
											dictionary[header] = objectsDto.Data[header];
										}
									}
								}
								modelEntityProject.Data.Add(dictionary);
							}

							modelEntityProject.Keys = headers;
						}
					}
					else
					{
						var service =  BackendsServerManager.Instance.UserService;
						var tuple = await service.GetUsers(project.Id, null);
						if (tuple.Item1 != BacksErrorCodes.Ok)
						{
							
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