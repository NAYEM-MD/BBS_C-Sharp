<%@ Page Language="C#" MasterPageFile="~/Form/Common/Default.master" AutoEventWireup="true" CodeBehind="UserDetail.aspx.cs" Inherits="w2.BBS.Manager.Form.User.UserDetail" %>
<%@ Import Namespace="w2.BBS.Manager" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<h2>ユーザー詳細</h2>

	<p>ユーザーID: <%: this.UserId %></p>
	<p>ログインID: <%: this.LoginId %></p>
	<p>ユーザー名: <%: this.UserName %></p>

	<p>
		<a href="<%: GetUserEditUrl() %>">編集</a>
		<asp:LinkButton ID="lbDeleteUser" runat="server" Text="削除" OnClick="lbDeleteUser_Click"
			OnClientClick="return confirm('削除しますか？');" />
	</p>

	<h3>投稿一覧</h3>
	<asp:Repeater ID="rPostList" runat="server" OnItemCommand="rPostList_ItemCommand">
		<HeaderTemplate>
			<table>
				<tr>
					<th>投稿ID</th>
					<th>タイトル</th>
					<th></th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td><%#: Eval("PostId") %></td>
				<td><%#: Eval("Title") %></td>
				<td>
					<asp:LinkButton runat="server" Text="削除" CommandName="DeletePost"
						CommandArgument='<%#: Eval("PostId") %>'
						OnClientClick="return confirm('削除しますか？');" />
				</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>

	<h3>返信一覧</h3>
	<asp:Repeater ID="rReplyList" runat="server" OnItemCommand="rReplyList_ItemCommand">
		<HeaderTemplate>
			<table>
				<tr>
					<th>返信ID</th>
					<th>本文</th>
					<th></th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td><%#: Eval("ReplyId") %></td>
				<td><%#: Eval("Body") %></td>
				<td>
					<asp:LinkButton runat="server" Text="削除" CommandName="DeleteReply"
						CommandArgument='<%#: Eval("ReplyId") %>'
						OnClientClick="return confirm('削除しますか？');" />
				</td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>

	<p><a href="<%: ResolveUrl(Constants.PAGE_USER_LIST) %>">一覧へ戻る</a></p>
</asp:Content>
