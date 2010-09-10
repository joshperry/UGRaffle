<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RaffleLib.Domain.Entities.Meeting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm()) { %>
        Date: <%= Html.EditorFor(x => x.Date) %><br />
        Description: <%= Html.EditorFor(x => x.Description) %><br />
        Tickets: <%= Html.EditorFor(x => x.TicketsForRegistering) %><br />

        <input type="submit" value="Add" />
    <% } %>

</asp:Content>
