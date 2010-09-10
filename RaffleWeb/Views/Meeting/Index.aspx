<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RaffleLib.Domain.Entities.Meeting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
UGRaffle : Current Meeting
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Current Meeting</h2>

    <%: Model.Date.ToShortDateString() %><br />
    <%: Model.Description %>

    <ul id="raffle-items">
    <% foreach (var item in Model.RaffleItems) { %>
        <li>
            <% Html.RenderPartial("RaffleItem", item); %>
        </li>
    <% } %>
    </ul>
</asp:Content>
