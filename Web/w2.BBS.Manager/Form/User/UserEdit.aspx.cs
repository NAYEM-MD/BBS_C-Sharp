// (c) 2026 W2 Co.,Ltd.
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace w2.BBS.Manager.Form.User
{
	/// <summary>
	/// ユーザー編集画面
	/// </summary>
	public partial class UserEdit : ManagerPageBase
	{
		/// <summary>
		/// ユーザーID
		/// </summary>
		private int UserId
		{
			get
			{
				return int.TryParse(
					Request.QueryString[Constants.REQUEST_KEY_USER_ID],
					out var userId)
					? userId
					: 0;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			CheckLogin();

			if (UserId <= 0)
			{
				Response.Redirect(Constants.PAGE_USER_LIST);
				return;
			}

			if (IsPostBack == false)
			{
				BindUser();
			}
		}

		protected void lbSave_Click(object sender, EventArgs e)
		{
			var loginId = tbLoginId.Text.Trim();
			var userName = tbUserName.Text.Trim();
			var password = tbPassword.Text.Trim();

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var values = new Dictionary<string, object>
				{
					{ Constants.FIELD_USER_LOGIN_ID, loginId },
					{ Constants.FIELD_USER_USER_NAME, userName },
				};

				if (string.IsNullOrEmpty(password) == false)
				{
					values.Add(Constants.FIELD_USER_PASSWORD, password);
				}

				queryFactory
					.Query(Constants.TABLE_USER)
					.Where(Constants.FIELD_USER_USER_ID, UserId)
					.Where(Constants.FIELD_USER_DEL_FLG, Constants.FLG_DEL_FLG_OFF)
					.Update(values);
			}

			Response.Redirect(
				$"{Constants.PAGE_USER_DETAIL}?{Constants.REQUEST_KEY_USER_ID}={UserId}");
		}

		/// <summary>
		/// ユーザー情報表示
		/// </summary>
		private void BindUser()
		{
			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var user = queryFactory
					.Query(Constants.TABLE_USER)
					.Select(
						$"{Constants.FIELD_USER_LOGIN_ID} as LoginId",
						$"{Constants.FIELD_USER_USER_NAME} as UserName")
					.Where(Constants.FIELD_USER_USER_ID, UserId)
					.Where(Constants.FIELD_USER_DEL_FLG, Constants.FLG_DEL_FLG_OFF)
					.FirstOrDefault<UserEditRow>();

				if (user is null)
				{
					Response.Redirect(Constants.PAGE_USER_LIST);
					return;
				}

				tbLoginId.Text = user.LoginId;
				tbUserName.Text = user.UserName;
			}
		}

		/// <summary>
		/// ユーザー編集行
		/// </summary>
		private class UserEditRow
		{
			public string LoginId { get; set; }
			public string UserName { get; set; }
		}
	}
}
