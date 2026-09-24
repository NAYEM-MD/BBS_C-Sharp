// (c) 2026 W2 Co.,Ltd.
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace w2.BBS.Manager.Form.User
{
	/// <summary>
	/// ユーザー詳細画面
	/// </summary>
	public partial class UserDetail : ManagerPageBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			CheckLogin();

			if (this.UserId == 0)
			{
				Response.Redirect(Constants.PAGE_USER_LIST);
			}

			BindUser();

			if (!IsPostBack)
			{
				BindPostList();
				BindReplyList();
			}
		}
		protected void lbDeleteUser_Click(object sender, EventArgs e)
		{
			DeleteUser();
			Response.Redirect(Constants.PAGE_USER_LIST);
		}
		protected void rPostList_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == Constants.COMMAND_DELETE_POST)
			{
				DeletePost(int.Parse(e.CommandArgument.ToString()));
				BindPostList();
			}
		}
		protected void rReplyList_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == Constants.COMMAND_DELETE_REPLY)
			{
				DeleteReply(int.Parse(e.CommandArgument.ToString()));
				BindReplyList();
			}
		}

		/// <summary>
		/// ユーザー情報表示
		/// </summary>
		private void BindUser()
		{
			var user = GetUser();
			if (user is null)
			{
				Response.Redirect(Constants.PAGE_USER_LIST);
				return;
			}

			this.LoginId = user.LoginId;
			this.UserName = user.UserName;
		}
		private void BindPostList()
		{
			rPostList.DataSource = GetPostList();
			rPostList.DataBind();
		}

		/// <summary>
		/// 返信一覧表示
		/// </summary>
		private void BindReplyList()
		{
			rReplyList.DataSource = GetReplyList();
			rReplyList.DataBind();
		}

		/// <summary>
		/// ユーザー取得
		/// </summary>
		private UserDetailRow GetUser()
		{
			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				return queryFactory
					.Query(Constants.TABLE_USER)
					.Select(
						$"{Constants.FIELD_USER_LOGIN_ID} as LoginId",
						$"{Constants.FIELD_USER_USER_NAME} as UserName")
					.Where(Constants.FIELD_USER_USER_ID, this.UserId)
					.Where(Constants.FIELD_USER_DEL_FLG, Constants.FLG_DEL_FLG_OFF)
					.FirstOrDefault<UserDetailRow>();
			}
		}

		/// <summary>
		/// 投稿一覧取得
		/// </summary>
		private PostRow[] GetPostList()
		{
			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				return queryFactory
					.Query(Constants.TABLE_FORUMPOST)
					.Select(
						$"{Constants.FIELD_FORUMPOST_POST_ID} as PostId",
						$"{Constants.FIELD_FORUMPOST_TITLE} as Title")
					.Where(Constants.FIELD_FORUMPOST_USER_ID, this.UserId)
					.Where(Constants.FIELD_FORUMPOST_DEL_FLG, Constants.FLG_DEL_FLG_OFF)
					.OrderByDesc(Constants.FIELD_FORUMPOST_POST_ID)
					.Get<PostRow>()
					.ToArray();
			}
		}

		/// <summary>
		/// 返信一覧取得
		/// </summary>
		private ReplyRow[] GetReplyList()
		{
			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				return queryFactory
					.Query(Constants.TABLE_FORUMREPLY)
					.Select(
						$"{Constants.FIELD_FORUMREPLY_REPLY_ID} as ReplyId",
						$"{Constants.FIELD_FORUMREPLY_BODY} as Body")
					.Where(Constants.FIELD_FORUMREPLY_USER_ID, this.UserId)
					.Where(Constants.FIELD_FORUMREPLY_DEL_FLG, Constants.FLG_DEL_FLG_OFF)
					.OrderByDesc(Constants.FIELD_FORUMREPLY_REPLY_ID)
					.Get<ReplyRow>()
					.ToArray();
			}
		}

		/// <summary>
		/// ユーザー削除
		/// </summary>
		private void DeleteUser()
		{
			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				queryFactory
					.Query(Constants.TABLE_USER)
					.Where(Constants.FIELD_USER_USER_ID, this.UserId)
					.Update(new Dictionary<string, object>
					{
						{ Constants.FIELD_USER_DEL_FLG, Constants.FLG_DEL_FLG_ON },
					});
			}
		}
		private void DeletePost(int postId)
		{
			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				queryFactory
					.Query(Constants.TABLE_FORUMPOST)
					.Where(Constants.FIELD_FORUMPOST_POST_ID, postId)
					.Update(new Dictionary<string, object>
					{
						{ Constants.FIELD_FORUMPOST_DEL_FLG, Constants.FLG_DEL_FLG_ON },
					});
			}
		}
		/// <summary>
		/// 返信削除
		/// </summary>
		private void DeleteReply(int replyId)
		{
			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				queryFactory
					.Query(Constants.TABLE_FORUMREPLY)
					.Where(Constants.FIELD_FORUMREPLY_REPLY_ID, replyId)
					.Update(new Dictionary<string, object>
					{
						{ Constants.FIELD_FORUMREPLY_DEL_FLG, Constants.FLG_DEL_FLG_ON },
					});
			}
		}
		protected string GetUserEditUrl()
		{
			var pageUrl = ResolveUrl(Constants.PAGE_USER_EDIT);
			var encodedUserId = HttpUtility.UrlEncode(this.UserId.ToString());
			return $"{pageUrl}?{Constants.REQUEST_KEY_USER_ID}={encodedUserId}";
		}

		protected int UserId
		{
			get { return int.TryParse(Request.QueryString[Constants.REQUEST_KEY_USER_ID], out var userId) ? userId : 0; }
		}
		protected string LoginId { get; private set; } = string.Empty;
		protected string UserName { get; private set; } = string.Empty;

		private class UserDetailRow
		{
			public string LoginId { get; set; }
			public string UserName { get; set; }
		}

		/// <summary>
		/// 投稿行
		/// </summary>
		private class PostRow
		{
			public int PostId { get; set; }
			public string Title { get; set; }
		}

		/// <summary>
		/// 返信行
		/// </summary>
		private class ReplyRow
		{
			public int ReplyId { get; set; }
			public string Body { get; set; }
		}
	}
}
