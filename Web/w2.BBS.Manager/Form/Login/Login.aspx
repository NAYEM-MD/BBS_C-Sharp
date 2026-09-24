<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="w2.BBS.Manager.Form.Login.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <link rel="stylesheet" href="<%: ResolveUrl("~/css/style.css") %>" />
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
  <title>管理者ログイン</title>
</head>
<body>
  <form id="form1" runat="server">
    <h2>管理者ログイン</h2>
    <p>
      ログインID<br />
      <asp:TextBox ID="tbLoginId" runat="server" />
    </p>
    <p>
      パスワード<br />
      <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" />
    </p>

    <asp:LinkButton ID="lbLogin" runat="server" Text="ログイン" OnClick="lbLogin_Click" />

    <p>
      <asp:Literal ID="lErrorMessage" runat="server" />
    </p>
  </form>
</body>
</html>
