// (c) 2026 W2 Co.,Ltd.
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace w2.BBS.Manager.Form.Login
{
	/// <summary>
	/// 管理者ログイン画面
	/// </summary>
	public partial class Login : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Session[ManagerSession.SESSION_KEY_LOGIN_OPERATOR_ID] is null == false)
			{
				Response.Redirect(Constants.PAGE_USER_LIST);
			}
		}

		protected void lbLogin_Click(object sender, EventArgs e)
		{
			var loginId = tbLoginId.Text.Trim();
			var password = tbPassword.Text.Trim();

			if (string.IsNullOrEmpty(loginId) || string.IsNullOrEmpty(password))
			{
				lErrorMessage.Text = ManagerMessages.MESSAGE_LOGIN_INPUT_REQUIRED;
				return;
			}

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var operatorRow = queryFactory
					.Query(Constants.TABLE_OPERATOR)
					.Select(
						$"{Constants.FIELD_OPERATOR_OPERATOR_ID} as OperatorId",
						$"{Constants.FIELD_OPERATOR_OPERATOR_NAME} as OperatorName")
					.Where(Constants.FIELD_OPERATOR_LOGIN_ID, loginId)
					.Where(Constants.FIELD_OPERATOR_PASSWORD, password)
					.Where(Constants.FIELD_OPERATOR_DEL_FLG, Constants.FLG_DEL_FLG_OFF)
					.FirstOrDefault<OperatorLoginRow>();

				if (operatorRow is null)
				{
					lErrorMessage.Text = ManagerMessages.MESSAGE_LOGIN_FAILED;
					return;
				}

				this.Session[ManagerSession.SESSION_KEY_LOGIN_OPERATOR_ID] = operatorRow.OperatorId;
				this.Session[ManagerSession.SESSION_KEY_LOGIN_OPERATOR_NAME] = operatorRow.OperatorName;
			}

			Response.Redirect(Constants.PAGE_USER_LIST);
		}

		/// <summary>
		/// オペレータログイン行
		/// </summary>
		private class OperatorLoginRow
		{
			public int OperatorId { get; set; }
			public string OperatorName { get; set; }
		}
	}
}
