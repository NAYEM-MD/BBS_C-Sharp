// (c) 2026 W2 Co.,Ltd.
namespace w2.BBS.Front.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		public string LoginId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string ErrorMessage { get; set; }
		public string SuccessMessage { get; set; }
	}
}
