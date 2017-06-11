using System;
using System.Collections.Generic;
using System.Linq;
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


					return View("Index", modelNewProject);
			        //return View("AddProjectResult", modelNewProject);
			        //return RedirectToAction("Index", "Home");
		        }
	        }

			return View();
        }
    }
}