// (c) 2026 W2 Co.,Ltd.
using SqlKata.Compilers;
using SqlKata.Execution;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using w2.BBS.Front.Controller.Shared;
using w2.BBS.Front.ViewModels;
using w2.Common;

namespace w2.BBS.Front.Controller
{
	/// <summary>
	/// åfé¶î¬ÉgÉbÉvÉyÅ[ÉWÉRÉìÉgÉçÅ[Éâ
	/// </summary>
	public class TopPageController : BaseController
	{
		private const int PAGE_SIZE = 20;

		/// <summary>
		/// åfé¶î¬àÍóóâÊñ 
		/// </summary>
		[Route("~/forum")]
		[HttpGet]
		public ActionResult Index()
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return Redirect("~/login");
			}

			return View(
				"TopPage.liquid",
				new TopPageViewModel());
		}

		/// <summary>
		/// ìäçeàÍóóéÊìæ
		/// </summary>
		/// <param name="page">ÉyÅ[ÉWî‘çÜ</param>
		[Route("~/forum/get-posts")]
		[HttpGet]
		public ActionResult GetPosts(int page = 1)
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return JsonForJs(
					new TopPageViewModel
					{
						Posts = new ForumPostViewModel[0],
					});
			}

			var currentPage = page < 1 ? 1 : page;

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var totalCount = queryFactory.Query("w2_ForumPost as post")
					.Join("w2_User as u", "post.user_id", "u.user_id")
					.Where("post.del_flg", 0)
					.Where("u.del_flg", 0)
					.Count<int>();

				var totalPages = (totalCount + PAGE_SIZE - 1) / PAGE_SIZE;
				if ((totalPages > 0) && (currentPage > totalPages))
				{
					currentPage = totalPages;
				}

				var posts = queryFactory.Query("w2_ForumPost as post")
					.Join("w2_User as u", "post.user_id", "u.user_id")
					.Select(
						"post.post_id as PostId",
						"post.user_id as UserId",
						"u.user_name as Username",
						"post.title as Title",
						"post.body as Body")
					.Where("post.del_flg", 0)
					.Where("u.del_flg", 0)
					.OrderByDesc("post.post_id")
					.Offset((currentPage - 1) * PAGE_SIZE)
					.Limit(PAGE_SIZE)
					.Get<ForumPostViewModel>()
					.ToArray();

				return JsonForJs(
					new TopPageViewModel
					{
						Posts = posts,
						Page = currentPage,
						PageSize = PAGE_SIZE,
						TotalCount = totalCount,
						TotalPages = totalPages,
						HasPrevious = currentPage > 1,
						HasNext = currentPage < totalPages,
					});
			}
		}

		/// <summary>
		/// êVãKìäçeâÊñ 
		/// </summary>
		[Route("~/forum/post")]
		[HttpGet]
		public ActionResult Post()
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return Redirect("~/login");
			}

			return View(
				"Post.liquid",
				new PostViewModel());
		}

		/// <summary>
		/// êVãKìäçe
		/// </summary>
		/// <param name="title">É^ÉCÉgÉã</param>
		/// <param name="body">ñ{ï∂</param>
		[Route("~/forum/post")]
		[HttpPost]
		public ActionResult Post(string title, string body)
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
				queryFactory.Query("w2_ForumPost")
					.Insert(new Dictionary<string, object>
					{
				{ "user_id", userId },
				{ "title", title },
				{ "body", body },
				{ "del_flg", 0 },
					});
			}

			return JsonForJs(
				new
				{
					Success = true,
					RedirectUrl = "/forum",
				});
		}

		/// <summary>
		/// ìäçeè⁄ç◊âÊñ 
		/// </summary>
		/// <param name="id">ìäçeID</param>
		[Route("~/forum/detail")]
		[HttpGet]
		public ActionResult Detail(int id)
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return Redirect("~/login");
			}

			var loginUserId = (int)this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID];

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var postDetail = queryFactory.Query("w2_ForumPost as post")
					.Join("w2_User as u", "post.user_id", "u.user_id")
					.Select(
						"post.post_id as PostId",
						"post.user_id as UserId",
						"u.user_name as Username",
						"post.title as Title",
						"post.body as Body")
					.Where("post.post_id", id)
					.Where("post.del_flg", 0)
					.Where("u.del_flg", 0)
					.FirstOrDefault<PostDetailViewModel>();

				if (postDetail is null)
				{
					return Redirect("~/forum");
				}

				postDetail.Replies = queryFactory.Query("w2_ForumReply as reply")
					.Join("w2_User as u", "reply.user_id", "u.user_id")
					.Select(
						"reply.reply_id as ReplyId",
						"reply.user_id as UserId",
						"u.user_name as Username",
						"reply.body as Body")
					.Where("reply.post_id", id)
					.Where("reply.del_flg", 0)
					.Where("u.del_flg", 0)
					.OrderByDesc("reply.reply_id")
					.Get<ForumReplyViewModel>()
					.ToArray();

				postDetail.IsMine = postDetail.UserId == loginUserId;

				return View(
					"PostDetail.liquid",
					postDetail);
			}
		}

		/// <summary>
		/// ï‘êMìoò^
		/// </summary>
		/// <param name="postId">ìäçeID</param>
		/// <param name="body">ï‘êMñ{ï∂</param>
		[Route("~/forum/reply")]
		[HttpPost]
		public ActionResult Reply(int postId, string body)
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
				queryFactory.Query("w2_ForumReply")
					.Insert(new Dictionary<string, object>
					{
				{ "post_id", postId },
				{ "user_id", userId },
				{ "body", body },
				{ "del_flg", 0 },
					});
			}

			return JsonForJs(
				new
				{
					Success = true,
					RedirectUrl = "/forum/detail?id=" + postId,
				});
		}

		/// <summary>
		/// ìäçeï“èWâÊñ 
		/// </summary>
		/// <param name="id">ìäçeID</param>
		[Route("~/forum/edit")]
		[HttpGet]
		public ActionResult Edit(int id)
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return Redirect("~/login");
			}

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var post = queryFactory.Query("w2_ForumPost as post")
					.Join("w2_User as u", "post.user_id", "u.user_id")
					.Select(
						"post.post_id as PostId",
						"post.user_id as UserId",
						"u.user_name as Username",
						"post.title as Title",
						"post.body as Body")
					.Where("post.post_id", id)
					.Where("post.del_flg", 0)
					.Where("u.del_flg", 0)
					.FirstOrDefault<PostDetailViewModel>();

				if (post is null)
				{
					return Redirect("~/forum");
				}

				return View(
					"EditPost.liquid",
					new EditPostViewModel
					{
						PostId = post.PostId,
						Title = post.Title,
						Body = post.Body,
					});
			}
		}

		/// <summary>
		/// ìäçeï“èW
		/// </summary>
		/// <param name="postId">ìäçeID</param>
		/// <param name="title">É^ÉCÉgÉã</param>
		/// <param name="body">ñ{ï∂</param>
		[Route("~/forum/edit")]
		[HttpPost]
		public ActionResult Edit(int postId, string title, string body)
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

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				queryFactory.Query("w2_ForumPost")
					.Where("post_id", postId)
					.Where("del_flg", 0)
					.Update(new Dictionary<string, object>
					{
				{ "title", title },
				{ "body", body },
					});
			}

			return JsonForJs(
				new
				{
					Success = true,
					RedirectUrl = "/forum/detail?id=" + postId,
				});
		}

		/// <summary>
		/// ìäçeçÌèú
		/// </summary>
		/// <param name="postId">ìäçeID</param>
		[Route("~/forum/delete")]
		[HttpPost]
		public ActionResult Delete(int postId)
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

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());
				queryFactory.Query("w2_ForumPost")
					.Where("post_id", postId)
					.Update(new Dictionary<string, object>
					{
				{ "del_flg", 1 },
					});
			}

			return JsonForJs(
				new
				{
					Success = true,
					RedirectUrl = "/forum",
				});
		}

		/// <summary>
		/// ìäçeè⁄ç◊éÊìæ
		/// </summary>
		/// <param name="postId">ìäçeID</param>
		[Route("~/forum/get-post-detail")]
		[HttpGet]
		public ActionResult GetPostDetail(int postId)
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return JsonForJs(
					new PostDetailViewModel
					{
						Replies = new ForumReplyViewModel[0],
					});
			}

			var loginUserId = (int)this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID];

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var post = queryFactory.Query("w2_ForumPost as post")
					.Join("w2_User as u", "post.user_id", "u.user_id")
					.Select(
						"post.post_id as PostId",
						"post.user_id as UserId",
						"u.user_name as Username",
						"post.title as Title",
						"post.body as Body")
					.Where("post.post_id", postId)
					.Where("post.del_flg", 0)
					.Where("u.del_flg", 0)
					.FirstOrDefault<PostDetailViewModel>();

				if (post is null)
				{
					return JsonForJs(
						new PostDetailViewModel
						{
							Replies = new ForumReplyViewModel[0],
						});
				}

				post.IsMine = post.UserId == loginUserId;
				post.Replies = queryFactory.Query("w2_ForumReply as reply")
					.Join("w2_User as u", "reply.user_id", "u.user_id")
					.Select(
						"reply.reply_id as ReplyId",
						"reply.user_id as UserId",
						"u.user_name as Username",
						"reply.body as Body")
					.Where("reply.post_id", postId)
					.Where("reply.del_flg", 0)
					.Where("u.del_flg", 0)
					.OrderByDesc("reply.reply_id")
					.Get<ForumReplyViewModel>()
					.ToArray();

				return JsonForJs(post);
			}
		}

		/// <summary>
		/// ï‘êMàÍóóéÊìæ
		/// </summary>
		/// <param name="postId">ìäçeID</param>
		[Route("~/forum/get-replies")]
		[HttpGet]
		public ActionResult GetReplyList(int postId)
		{
			if (this.Session[FrontSession.SESSION_KEY_LOGIN_USER_ID] is null)
			{
				return JsonForJs(
					new
					{
						Replies = new ForumReplyViewModel[0],
					});
			}

			using (var connection = new SqlConnection(Constants.STRING_SQL_CONNECTION))
			{
				var queryFactory = new QueryFactory(connection, new SqlServerCompiler());

				var replies = queryFactory.Query("w2_ForumReply as reply")
					.Join("w2_User as u", "reply.user_id", "u.user_id")
					.Select(
						"reply.reply_id as ReplyId",
						"reply.user_id as UserId",
						"u.user_name as Username",
						"reply.body as Body")
					.Where("reply.post_id", postId)
					.Where("reply.del_flg", 0)
					.Where("u.del_flg", 0)
					.OrderByDesc("reply.reply_id")
					.Get<ForumReplyViewModel>()
					.ToArray();

				return JsonForJs(
					new
					{
						Replies = replies,
					});
			}
		}
	}
}
