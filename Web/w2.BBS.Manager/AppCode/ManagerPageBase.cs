// (c) 2026 W2 Co.,Ltd.

using System.Web.UI;

namespace w2.BBS.Manager
{
	/// <summary>
	/// ログイン必須ページの基底クラス
	/// </summary>
	public class ManagerPageBase : Page
	{
		/// <summary>
		/// ログインチェック
		/// </summary>
		protected void CheckLogin()
		{
			if (this.Session[ManagerSession.SESSION_KEY_LOGIN_OPERATOR_ID] is null)
			{
				Response.Redirect(Constants.PAGE_LOGIN);
			}
		}
	}
}
