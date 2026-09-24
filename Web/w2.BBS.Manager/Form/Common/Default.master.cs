// (c) 2026 W2 Co.,Ltd.
using System;
using System.Web.UI;

namespace w2.BBS.Manager.Form.Common
{
	public partial class Default :MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			divMenu.Visible = this.Session[ManagerSession.SESSION_KEY_LOGIN_OPERATOR_ID] is not null;
		}

		protected void lbLogout_Click(object sender, EventArgs e)
		{
			this.Session.Clear();
			Response.Redirect(Constants.PAGE_LOGIN);
		}
	}
}
