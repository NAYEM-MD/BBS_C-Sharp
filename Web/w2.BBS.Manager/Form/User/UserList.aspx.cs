// (c) 2026 W2 Co.,Ltd.
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace w2.BBS.Manager.Form.User
{
	/// <summary>
	/// ユーザー一覧画面
	/// </summary>
	public partial class UserList : ManagerPageBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			CheckLogin();

			if (!IsPostBack)
			{
				BindUserList();
			}
		}

		protected void lbSearch_Click(object sender, EventArgs e)
		{
			BindUserList();
		}

		private void BindUserList()
		{
			rUserList.DataSource = GetUserList(tbSearchWord.Text.Trim());
			rUserList.DataBind();
		}

		/// <summary>
		/// ユーザー一覧取得
		/// </summary
		private UserListRow[] GetUserList(string searchWord)
		{
			var searchUserId = int.TryParse(searchWord, out var parsedUserId) ? parsedUserId : 0;

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				var query = queryFactory
					.Query(Constants.TABLE_USER)
					.Select(
						$"{Constants.FIELD_USER_USER_ID} as UserId",
						$"{Constants.FIELD_USER_LOGIN_ID} as LoginId",
						$"{Constants.FIELD_USER_USER_NAME} as UserName")
					.Where(Constants.FIELD_USER_DEL_FLG, Constants.FLG_DEL_FLG_OFF);

				if (string.IsNullOrEmpty(searchWord) == false)
				{
					query.Where(condition =>
					{
						condition
							.WhereLike(Constants.FIELD_USER_LOGIN_ID, $"%{searchWord}%")
							.OrWhereLike(Constants.FIELD_USER_USER_NAME, $"%{searchWord}%");

						if (searchUserId > 0)
						{
							condition.OrWhere(Constants.FIELD_USER_USER_ID, searchUserId);
						}

						return condition;
					});
				}

				return query
					.OrderBy(Constants.FIELD_USER_USER_ID)
					.Get<UserListRow>()
					.ToArray();
			}
		}
		protected string GetUserDetailUrl(object userId)
		{
			var pageUrl = ResolveUrl(Constants.PAGE_USER_DETAIL);
			var encodedUserId = HttpUtility.UrlEncode(userId.ToString());
			return $"{pageUrl}?{Constants.REQUEST_KEY_USER_ID}={encodedUserId}";
		}

		/// <summary>
		/// ユーザー一覧行
		/// </summary>
		private class UserListRow
		{
			public int UserId { get; set; }
			public string LoginId { get; set; }
			public string UserName { get; set; }
		}
	}
}
