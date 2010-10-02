<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RaffleLib.Domain.Entities.Registration>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Register
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Register</h2>

    <% if(Model == null) { %>
        <% using (Html.BeginForm()) { %>
            Welcome to the meeting, click register to continue.
            <input type="submit" value="Register" />
        <% } %>
    <% } else { %>

    <% } %>

</asp:Content>
