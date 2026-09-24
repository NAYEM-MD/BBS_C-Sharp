// (c) 2026 W2 Co.,Ltd.
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;

namespace w2.BBS.Manager.Form.Post
{
	/// <summary>
	/// “Šeˆê——‰æ–Ê
	/// </summary>
	public partial class PostList : ManagerPageBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			CheckLogin();

			if (IsPostBack == false)
			{
				BindPostList();
				BindReplyList();
			}
		}

		protected void lbSearch_Click(object sender, EventArgs e)
		{
			BindPostList();
			BindReplyList();
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
		/// “Šeˆê——•\¦
		/// </summary>
		private void BindPostList()
		{
			rPostList.DataSource = GetPostList(tbSearchWord.Text.Trim());
			rPostList.DataBind();
		}

		/// <summary>
		/// •ÔMˆê——•\¦
		/// </summary>
		private void BindReplyList()
		{
			rReplyList.DataSource = GetReplyList(tbSearchWord.Text.Trim());
			rReplyList.DataBind();
		}

		/// <summary>
		/// “Šeˆê——æ“¾
		/// </summary>
		/// <param name="searchWord">ŒŸõŒê</param>
		private PostListRow[] GetPostList(string searchWord)
		{
			var searchUserId = int.TryParse(searchWord, out var parsedUserId) ? parsedUserId : 0;

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				var query = queryFactory
					.Query($"{Constants.TABLE_FORUMPOST} as post")
					.Join($"{Constants.TABLE_USER} as u", "post.user_id", "u.user_id")
					.Select(
						"post.post_id as PostId",
						"post.user_id as UserId",
						"post.title as Title",
						"post.body as Body",
						"u.login_id as LoginId",
						"u.user_name as UserName")
					.Where("post.del_flg", Constants.FLG_DEL_FLG_OFF)
					.Where("u.del_flg", Constants.FLG_DEL_FLG_OFF);

				if (string.IsNullOrEmpty(searchWord) == false)
				{
					query.Where(condition =>
					{
						condition
							.WhereLike("post.title", $"%{searchWord}%")
							.OrWhereLike("post.body", $"%{searchWord}%")
							.OrWhereLike("u.user_name", $"%{searchWord}%");

						if (searchUserId > 0)
						{
							condition.OrWhere("post.user_id", searchUserId);
						}

						return condition;
					});
				}

				return query
					.OrderByDesc("post.post_id")
					.Get<PostListRow>()
					.ToArray();
			}
		}

		/// <summary>
		/// •ÔMˆê——æ“¾
		/// </summary>
		/// <param name="searchWord">ŒŸõŒê</param>
		private ReplyListRow[] GetReplyList(string searchWord)
		{
			var searchUserId = int.TryParse(searchWord, out var parsedUserId) ? parsedUserId : 0;

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				var query = queryFactory
					.Query($"{Constants.TABLE_FORUMREPLY} as reply")
					.Join($"{Constants.TABLE_USER} as u", "reply.user_id", "u.user_id")
					.Select(
						"reply.reply_id as ReplyId",
						"reply.post_id as PostId",
						"reply.user_id as UserId",
						"reply.body as Body",
						"u.login_id as LoginId",
						"u.user_name as UserName")
					.Where("reply.del_flg", Constants.FLG_DEL_FLG_OFF)
					.Where("u.del_flg", Constants.FLG_DEL_FLG_OFF);

				if (string.IsNullOrEmpty(searchWord) == false)
				{
					query.Where(condition =>
					{
						condition
							.WhereLike("reply.body", $"%{searchWord}%")
							.OrWhereLike("u.user_name", $"%{searchWord}%");

						if (searchUserId > 0)
						{
							condition.OrWhere("reply.user_id", searchUserId);
						}

						return condition;
					});
				}

				return query
					.OrderByDesc("reply.reply_id")
					.Get<ReplyListRow>()
					.ToArray();
			}
		}

		/// <summary>
		/// “Šeíœ
		/// </summary>
		/// <param name="postId">“ŠeID</param>
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
		/// •ÔMíœ
		/// </summary>
		/// <param name="replyId">•ÔMID</param>
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

		/// <summary>
		/// “Šeˆê——s
		/// </summary>
		private class PostListRow
		{
			public int PostId { get; set; }
			public int UserId { get; set; }
			public string Title { get; set; }
			public string Body { get; set; }
			public string LoginId { get; set; }
			public string UserName { get; set; }
		}

		/// <summary>
		/// •ÔMˆê——s
		/// </summary>
		private class ReplyListRow
		{
			public int ReplyId { get; set; }
			public int PostId { get; set; }
			public int UserId { get; set; }
			public string Body { get; set; }
			public string LoginId { get; set; }
			public string UserName { get; set; }
		}
	}
}
