// (c) 2026 W2 Co.,Ltd.
namespace w2.BBS.Front.ViewModels
{
	public class EditPostViewModel : BaseViewModel
	{
		public int PostId { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public string ErrorMessage { get; set; }
	}
}
