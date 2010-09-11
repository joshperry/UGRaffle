<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RaffleWeb.Models.LoginViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Login</h2>
    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true) %>
        <%: Html.EditorForModel() %>
        <input type="submit" value="Login" />
    <% } %>
    <% Html.EnableClientValidation(); %>
</asp:Content>
