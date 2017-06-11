using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BackendsCommon.Types.BacksModel;

namespace BackendsDashboard.Models
{
	public class ProjectViewModel
	{
		[Required]
		[DataType(DataType.Text)]
		public string Name { get; set; }
	}

	public class LoginViewModel
	{

		//[Required]
		//[Display(Name = "Email")]
		//[EmailAddress]
		//public string Email { get; set; }
		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Login")]
		public string Login { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		//[Display(Name = "Remember me?")]
		//public bool RememberMe { get; set; }
	}

	public class RegisterViewModel
	{
		[Required]
		[Display(Name = "Login")]
		public string ScreenName { get; set; }

		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }

		[Required]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class IndexViewModel
	{
		public string PhoneNumber { get; set; }
		public bool BrowserRemembered { get; set; }
	}

	public class NewProjectViewModel
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string AppId { get; set; }

		public string ApiKeyAccess { get; set; }

		public string MasterKeyAccess { get; set; }

		public int EntitiesCount { get; set; }

		public long UserLogedIn { get; set; }

		//public string Settings { get; set; }

		//public long UserCount { get; set; }

		//public long InstallationCount { get; set; }

		public DateTime CreatedAt { get; set; }

		public BacksProjectSchema Schema { get; set; }
	}

	public class AccountProjectsViewModel
	{
		public AccountProjectsViewModel()
		{
			Projects = new List<NewProjectViewModel>();
		}
		public List<NewProjectViewModel> Projects { get; set; }
	}


	public class EntityDataViewModel
	{
		public List<string> Keys { get; set; }

		public List<Dictionary<string, object>> Data { get; set; }

		public EntityDataViewModel()
		{
			Keys = new List<string>();
			Data = new List<Dictionary<string, object>>();
		}
	}

	public class MainDashboardProjectModel
	{
		public MainDashboardProjectModel()
		{
			SelecteEntity = new EntityDataViewModel();
			CurrentProject = new NewProjectViewModel();
		}
		public EntityDataViewModel SelecteEntity { get; set; }
		public NewProjectViewModel CurrentProject { get; set; }
	}
		
}