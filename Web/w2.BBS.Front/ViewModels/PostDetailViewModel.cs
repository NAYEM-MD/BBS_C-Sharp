// (c) 2026 W2 Co.,Ltd.
using System;
namespace w2.BBS.Front.ViewModels
{
	public class PostDetailViewModel : BaseViewModel
	{
		public int PostId { get; set; }
		public int UserId { get; set; }
		public string Username { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public ForumReplyViewModel[] Replies { get; set; }
		public string ErrorMessage { get; set; }
		public bool IsMine { get; set; }
	}
	public class ForumReplyViewModel
	{
		public int ReplyId { get; set; }
		public int UserId { get; set; }
		public string Username { get; set; }
		public string Body { get; set; }
	}
}
