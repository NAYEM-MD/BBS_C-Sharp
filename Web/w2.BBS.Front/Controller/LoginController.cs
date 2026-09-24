// (c) 2026 W2 Co.,Ltd.
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data.SqlClient;
using System.Web.Mvc;
using w2.BBS.Front.Controller.Shared;
using w2.BBS.Front.ViewModels;
using w2.Common;

namespace w2.BBS.Front.Controller
{
	/// <summary>
	/// ログインコントローラ
	/// </summary>
	public class LoginController : BaseController
	{
		/// <summary>
		/// ログイン画面
		/// </summary>
		[Route("~/login")]
		[HttpGet]
		public ActionResult Login()
		{
			return View(
				"Login.liquid",
				new LoginViewModel());
		}

		/// <summary>
		/// ログイン
		/// </summary>
		/// <param name="loginId">ログインID</param>
		/// <param name="password">パスワード</param>
		[Route("~/login")]
		[HttpPost]
		public ActionResult Login(string loginId, string password)
		{
			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var user = queryFactory.Query("w2_User")
					.Select("user_id", "user_name")
					.Where("login_id", loginId)
					.Where("password", password)
					.Where("del_flg", 0)
					.FirstOrDefault();

				if (user is null)
				{
					return JsonForJs(
						new
						{
							Success = false,
							Message = FrontMessages.MESSAGE_LOGIN_FAILED,
						});
				}

				this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] = (int)user.user_id;
				this.Session[FrontSession.SESSION_KEY_LOGIN_USER_NAME] = (string)user.user_name;
			}

			return JsonForJs(
				new
				{
					Success = true,
					RedirectUrl = "/forum",
				});
		}
	}
}
