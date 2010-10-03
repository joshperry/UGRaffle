<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RaffleWeb.Models.IndexViewModel>" %>
<%@ Import Namespace="RaffleWeb.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
UGRaffle : Current Meeting
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Current Meeting</h2>
    <% var meeting = Model.Meeting;
        if (meeting != null) { %>

        <%: meeting.Date.ToShortDateString()%><br />
        <%: meeting.Description%>

        <ul id="raffle-items">
        <% foreach (var item in meeting.RaffleItems)
           { %>
            <li>
                <% Html.RenderPartial("RaffleItem", item); %>
            </li>
        <% } %>
        </ul>

    <% } else { %>
        No meeting has been scheduled for today.
    <% } %>
    
    <%
        if (Model.Member == null)
        {
            Html.RenderPartial("CompactLogin", new LoginViewModel());
        }
        else
        {
    %>
        <% using (Html.BeginForm("Register", "Registration")) { %>
            <input type="submit" value="Register" />
        <% } %>
    <% } %>
</asp:Content>
