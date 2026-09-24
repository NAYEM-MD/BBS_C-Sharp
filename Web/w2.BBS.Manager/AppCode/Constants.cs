// (c) 2026 W2 Co.,Ltd.
namespace w2.BBS.Manager
{
	public class Constants : w2.Common.Constants
	{
		public const string TABLE_USER = "w2_User";
		public const string TABLE_FORUMPOST = "w2_ForumPost";
		public const string TABLE_FORUMREPLY = "w2_ForumReply";
		public const string TABLE_OPERATOR = "w2_Operator";

		public const string FIELD_USER_USER_ID = "user_id";
		public const string FIELD_USER_LOGIN_ID = "login_id";
		public const string FIELD_USER_PASSWORD = "password";
		public const string FIELD_USER_USER_NAME = "user_name";
		public const string FIELD_USER_DEL_FLG = "del_flg";

		public const string FIELD_FORUMPOST_POST_ID = "post_id";
		public const string FIELD_FORUMPOST_USER_ID = "user_id";
		public const string FIELD_FORUMPOST_TITLE = "title";
		public const string FIELD_FORUMPOST_BODY = "body";
		public const string FIELD_FORUMPOST_DEL_FLG = "del_flg";

		public const string FIELD_FORUMREPLY_REPLY_ID = "reply_id";
		public const string FIELD_FORUMREPLY_POST_ID = "post_id";
		public const string FIELD_FORUMREPLY_USER_ID = "user_id";
		public const string FIELD_FORUMREPLY_BODY = "body";
		public const string FIELD_FORUMREPLY_DEL_FLG = "del_flg";

		public const string FIELD_OPERATOR_OPERATOR_ID = "operator_id";
		public const string FIELD_OPERATOR_LOGIN_ID = "login_id";
		public const string FIELD_OPERATOR_PASSWORD = "password";
		public const string FIELD_OPERATOR_OPERATOR_NAME = "operator_name";
		public const string FIELD_OPERATOR_DEL_FLG = "del_flg";

		public const int FLG_DEL_FLG_OFF = 0;
		public const int FLG_DEL_FLG_ON = 1;

		public const string PAGE_LOGIN = "~/Form/Login/Login.aspx";
		public const string PAGE_USER_LIST = "~/Form/User/UserList.aspx";
		public const string PAGE_USER_DETAIL = "~/Form/User/UserDetail.aspx";
		public const string PAGE_USER_EDIT = "~/Form/User/UserEdit.aspx";
		public const string PAGE_POST_LIST = "~/Form/Post/PostList.aspx";

		public const string REQUEST_KEY_USER_ID = "userId";

		public const string COMMAND_DELETE_POST = "DeletePost";
		public const string COMMAND_DELETE_REPLY = "DeleteReply";
	}
}
