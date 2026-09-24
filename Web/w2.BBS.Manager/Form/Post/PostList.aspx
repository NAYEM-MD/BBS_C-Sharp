<%@ Page Language="C#" MasterPageFile="~/Form/Common/Default.master"
  AutoEventWireup="true" CodeBehind="PostList.aspx.cs"
  Inherits="w2.BBS.Manager.Form.Post.PostList" %>
<%@ Import Namespace="w2.BBS.Manager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <h2>投稿一覧</h2>

  <p>
    検索（タイトル / 本文 / ユーザー名）<br />
    <asp:TextBox ID="tbSearchWord" runat="server" />
    <asp:LinkButton ID="lbSearch" runat="server" Text="検索" OnClick="lbSearch_Click" />
  </p>

  <h3>投稿</h3>
  <asp:Repeater ID="rPostList" runat="server" OnItemCommand="rPostList_ItemCommand">
    <HeaderTemplate>
      <table>
        <tr>
          <th>ログインID</th>
          <th>タイトル</th>
          <th>本文</th>
          <th>投稿者</th>
          <th></th>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
      <tr>
        <td><%#: Eval("LoginId") %></td>
        <td><%#: Eval("Title") %></td>
        <td><%#: Eval("Body") %></td>
        <td><%#: Eval("UserName") %></td>
        <td>
          <asp:LinkButton runat="server" Text="削除"
            CommandName="DeletePost"
            CommandArgument='<%#: Eval("PostId") %>'
            OnClientClick="return confirm('削除しますか？');" />
        </td>
      </tr>
    </ItemTemplate>
    <FooterTemplate>
      </table>
    </FooterTemplate>
  </asp:Repeater>

  <h3>返信</h3>
  <asp:Repeater ID="rReplyList" runat="server" OnItemCommand="rReplyList_ItemCommand">
    <HeaderTemplate>
      <table>
        <tr>
          
          <th>本文</th>
          <th>ログインID</th>
          <th>投稿者</th>
          <th></th>
        </tr>
    </HeaderTemplate>
    <ItemTemplate>
      <tr>
        
        <td><%#: Eval("Body") %></td>
        <td><%#: Eval("LoginId") %></td>
        <td><%#: Eval("UserName") %></td>
        <td>
          <asp:LinkButton runat="server" Text="削除"
            CommandName="DeleteReply"
            CommandArgument='<%#: Eval("ReplyId") %>'
            OnClientClick="return confirm('削除しますか？');" />
        </td>
      </tr>
    </ItemTemplate>
    <FooterTemplate>
      </table>
    </FooterTemplate>
  </asp:Repeater>
</asp:Content>
