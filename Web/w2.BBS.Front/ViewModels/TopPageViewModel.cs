// (c) 2026 W2 Co.,Ltd.
using System.Collections.Generic;

namespace w2.BBS.Front.ViewModels
{
	public class TopPageViewModel : BaseViewModel
	{
		public IEnumerable<ForumPostViewModel> Posts { get; set; }
	
		public int Page { get; set; }	
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public int TotalPages { get; set; }
		public bool HasPrevious { get; set; }
		public bool HasNext { get; set; }


	}

	public class ForumPostViewModel
	{
		public int PostId { get; set; }
		
		public int UserId { get; set; }
		public string Username {get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
	}

}
