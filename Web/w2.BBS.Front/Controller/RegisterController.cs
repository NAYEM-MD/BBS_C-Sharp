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
	/// 会員登録コントローラ
	/// </summary>
	public class RegisterController : BaseController
	{
		/// <summary>
		/// 会員登録画面
		/// </summary>
		[Route("~/register")]
		[HttpGet]
		public ActionResult Register()
		{
			return View(
				"Register.liquid",
				new RegisterViewModel());
		}

		/// <summary>
		/// 会員登録
		/// </summary>
		/// <param name="loginId">ログインID</param>
		/// <param name="password">パスワード</param>
		/// <param name="userName">ユーザー名</param>
		[Route("~/register")]
		[HttpPost]
		public ActionResult Register(string loginId, string password, string userName)
		{
			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var activeExists = queryFactory.Query("w2_User")
					.Where("login_id", loginId)
					.Where("del_flg", 0)
					.Count<int>() > 0;

				if (activeExists)
				{
					return JsonForJs(
						new
						{
							Success = false,
							Message = FrontMessages.MESSAGE_LOGIN_ID_ALREADY_USED,
						});
				}

				var deletedExists = queryFactory.Query("w2_User")
					.Where("login_id", loginId)
					.Where("del_flg", 1)
					.Count<int>() > 0;

				if (deletedExists == false)
				{
					queryFactory.Query("w2_User")
						.Insert(new Dictionary<string, object>
						{
							{ "login_id", loginId },
							{ "password", password },
							{ "user_name", userName },
							{ "del_flg", 0 },
						});
				}
				else
				{
					queryFactory.Query("w2_User")
						.Where("login_id", loginId)
						.Where("del_flg", 1)
						.Update(new Dictionary<string, object>
						{
							{ "password", password },
							{ "user_name", userName },
							{ "del_flg", 0 },
						});
				}
			}

			return JsonForJs(
				new
				{
					Success = true,
					RedirectUrl = "/login",
				});
		}
	}
}
