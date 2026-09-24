<%@ Page Language="C#" MasterPageFile="~/Form/Common/Default.master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="w2.BBS.Manager.Form.User.UserList" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<h2>ユーザー一覧</h2>
	<p>
		検索（ログインID / ユーザー名 / ユーザーID）<br />
		<asp:TextBox ID="tbSearchWord" runat="server" />
		<asp:LinkButton ID="lbSearch" runat="server" Text="検索" OnClick="lbSearch_Click" />
	</p>
	<asp:Repeater ID="rUserList" runat="server">
		<HeaderTemplate>
			<table>
				<tr>
					<th>ユーザーID</th>
					<th>ログインID</th>
					<th>ユーザー名</th>
					<th></th>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td><%#: Eval("UserId") %></td>
				<td><%#: Eval("LoginId") %></td>
				<td><%#: Eval("UserName") %></td>
				<td><a href='<%#: GetUserDetailUrl(Eval("UserId")) %>'>詳細</a></td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
</asp:Content>
