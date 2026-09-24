// (c) 2026 W2 Co.,Ltd.
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using w2.BBS.Front.Controller.Shared;
using w2.BBS.Front.ViewModels;
using w2.Common;

namespace w2.BBS.Front.Controller
{
	/// <summary>
	/// 設定コントローラ
	/// </summary>
	public class SettingsController : BaseController
	{
		/// <summary>
		/// 設定画面
		/// </summary>
		[Route("~/settings")]
		[HttpGet]
		public ActionResult Index()
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return Redirect("~/login");
			}

			var userId = (int)this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID];

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var user = queryFactory.Query("w2_User")
					.Select(
						"login_id as LoginId",
						"user_name as UserName")
					.Where("user_id", userId)
					.Where("del_flg", 0)
					.FirstOrDefault<SettingsViewModel>();

				if (user is null)
				{
					return Redirect("~/login");
				}

				return View(
					"Settings.liquid",
					user);
			}
		}

		/// <summary>
		/// 会員情報更新
		/// </summary>
		/// <param name="userName">ユーザー名</param>
		/// <param name="password">パスワード</param>
		[Route("~/settings")]
		[HttpPost]
		public ActionResult Index(string userName, string password)
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return JsonForJs(
					new
					{
						Success = false,
						Message = FrontMessages.MESSAGE_LOGIN_REQUIRED,
						RedirectUrl = "/login",
					});
			}

			var name = (userName ?? string.Empty).Trim();
			var newPassword = (password ?? string.Empty).Trim();
			var userId = (int)this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID];

			if (string.IsNullOrEmpty(name))
			{
				return JsonForJs(
					new
					{
						Success = false,
						Message = FrontMessages.MESSAGE_USER_NAME_REQUIRED,
					});
			}

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var values = new Dictionary<string, object>
				{
					{ "user_name", name },
				};

				if (string.IsNullOrEmpty(newPassword) == false)
				{
					values.Add("password", newPassword);
				}

				queryFactory.Query("w2_User")
					.Where("user_id", userId)
					.Where("del_flg", 0)
					.Update(values);
			}

			this.Session[FrontSession.SESSION_KEY_LOGIN_USER_NAME] = name;

			return JsonForJs(
				new
				{
					Success = true,
					Message = FrontMessages.MESSAGE_SETTINGS_UPDATED,
					UserName = name,
				});
		}

		/// <summary>
		/// 退会
		/// </summary>
		[Route("~/settings/withdraw")]
		[HttpPost]
		public ActionResult Withdraw()
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return JsonForJs(
					new
					{
						Success = false,
						Message = FrontMessages.MESSAGE_LOGIN_REQUIRED,
						RedirectUrl = "/login",
					});
			}

			var userId = (int)this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID];

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				queryFactory.Query("w2_User")
					.Where("user_id", userId)
					.Update(new Dictionary<string, object>
					{
						{ "del_flg", 1 },
					});
			}

			this.Session.Remove(FrontSession.SESSION_KEY_LOGIN_USER_ID);
			this.Session.Remove(FrontSession.SESSION_KEY_LOGIN_USER_NAME);

			return JsonForJs(
				new
				{
					Success = true,
					RedirectUrl = "/login",
				});
		}
	}
}
