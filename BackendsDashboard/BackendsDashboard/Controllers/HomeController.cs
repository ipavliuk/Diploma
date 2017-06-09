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
	public class HomeController : Controller
	{
	
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			//ViewBag.Message = "Дане застосування є адмінінстративною частиної BaaS платформи виконане в рамках бакалаврської кваліфікаційної роботи";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Адреса:";

			return View();
		}

		[HttpGet]
		public ActionResult Register()
		{
			ViewBag.IsAuthenticated = false;
			return View();
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			ViewBag.IsAuthenticated = false;
			SessionBag.Current.IsAuthenticated = false;
			if (ModelState.IsValid)
			{
				var service = BackendsServerManager.Instance.AdminService;
				var accountDto = await service.SignIn(model.FirstName, model.LastName, model.Email, model.ScreenName, model.Password);

				string result = "";
				
				
				if (accountDto != null)
				{
					switch (accountDto.Error)
					{
						case BacksErrorCodes.Ok:
							SessionBag.Current.account = accountDto;
							SessionBag.Current.AccountName = accountDto.FirstName + " " +accountDto.LastName;
							SessionBag.Current.AccountId = accountDto.Id;


								return RedirectToAction("Index", "Home");
						case BacksErrorCodes.DuplicateLogin:
							result = "Даний логін вже присутній в системі";
							break;
						case BacksErrorCodes.SignUpError:
						default:
							ModelState.AddModelError("", "Invalid login attempt.");
							return View(model);
					}
					// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
						// Send an email with this link
						// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
						// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
						// await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
					
				}

				AddErrors(result);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}


		[HttpGet]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var service = BackendsServerManager.Instance.AdminService;
			var result = await service.Login(model.Login, model.Password);

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, change to shouldLockout: true
			//var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
			if (result != null)
			{
				SessionBag.Current.account = result;
				SessionBag.Current.AccountName = result.FirstName + " " + result.LastName;
				SessionBag.Current.AccountId = result.Id;

				//return RedirectToLocal(returnUrl);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("", "Invalid login attempt.");
				return View(model);
			}
			
		}
		
		public ActionResult AddProject()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> AddProject(ProjectViewModel model)
		{
			if (ModelState.IsValid)
			{
				var service = BackendsServerManager.Instance.AdminService;
				ProjectDto accountDto = await service.AddNewProject(model.Name, SessionBag.Current.AccountId);
			
				if (accountDto != null)
				{
					var modelNewProject = new NewProjectViewModel()
					{
						Name = accountDto.Name,
						AppId = accountDto.Id,
						ApiKeyAccess = accountDto.ApiKeyAccess,
						MasterKeyAccess = accountDto.MasterKeyAccess,
						CreatedAt = accountDto.CreatedAt
					};
					return View("AddProjectResult", modelNewProject);
					//return RedirectToAction("Index", "Home");
				}

				AddErrors("Проект не був створений");
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		public async Task<ActionResult> GetAllProjects()
		{
			if (SessionBag.Current.account != null)
			{
				AccountDto account = SessionBag.Current.account;
				var service = BackendsServerManager.Instance.AdminService;
				List<ProjectDto> projects = await service.GetUserProjects(account.Id);

				if (projects != null)
				{
					var projectsModel = new AccountProjectsViewModel();
					foreach (var item in projects)
					{
						var modelNewProject = new NewProjectViewModel()
						{
							Id = item.Id,
							Name = item.Name,
							AppId = item.AppId,
							ApiKeyAccess = item.ApiKeyAccess,
							MasterKeyAccess = item.MasterKeyAccess,
							CreatedAt = item.CreatedAt
						};

						projectsModel.Projects.Add(modelNewProject);
					}
					return View("GetAllProjects", projectsModel);
					//return View("AddProjectResult", modelNewProject);
					//return RedirectToAction("Index", "Home");
				}
			}
			
			

			

			return View();
		}

		[HttpPost]
		public ActionResult LogOff()
		{
			SessionBag.Current.account = null;
			//SignOut
			return RedirectToAction("Index", "Home");
		}

		public static bool IsValid()
		{
			return SessionBag.Current.account != null;
		} 

		private void AddErrors(string result)
		{
			ModelState.AddModelError("", result);
		}
		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}
	}
}