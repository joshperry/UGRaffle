<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<RaffleLib.Domain.Entities.Meeting>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	List
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>List</h2>

    <table>
    <% foreach (var meeting in Model) { %>
        <tr>
            <td><%: meeting.Date.ToShortDateString() %></td>
            <td><%: meeting.Description %></td>
            <td>
                <% using (Html.BeginForm("Delete", "Meeting")) { %>
                    <%: Html.Hidden("Id", meeting.Id) %>
                    <input type="submit" value="Delete" />
                <% } %>
            </td>
        </tr>
    <% } %>
    </table>

</asp:Content>
