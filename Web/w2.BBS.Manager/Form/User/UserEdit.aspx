<%@ Page Language="C#" MasterPageFile="~/Form/Common/Default.master"
  AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs"
  Inherits="w2.BBS.Manager.Form.User.UserEdit" %>
<%@ Import Namespace="w2.BBS.Manager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <h2>ユーザー編集</h2>

  <p>ログインID<br /><asp:TextBox ID="tbLoginId" runat="server" /></p>
  <p>名前<br /><asp:TextBox ID="tbUserName" runat="server" /></p>
  <p>新しいパスワード<br /><asp:TextBox ID="tbPassword" runat="server" TextMode="Password" /></p>

  <asp:LinkButton ID="lbSave" runat="server" Text="保存" OnClick="lbSave_Click" />
  <a href="<%: ResolveUrl(Constants.PAGE_USER_LIST) %>">戻る</a>
</asp:Content>
