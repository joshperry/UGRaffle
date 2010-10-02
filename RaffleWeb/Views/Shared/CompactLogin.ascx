<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<RaffleWeb.Models.LoginViewModel>" %>
<% using (Html.BeginForm("Login", "Member")) { %>
<table cellspacing="0">
    <tr>
        <td>
            <label for="Email">Email</label>
        </td>
        <td>
            <label for="Password">Password</label>
        </td>
    </tr>
    <tr>
        <td>
            <%= Html.EditorFor(x => x.Email) %>
        </td>
        <td>
            <%= Html.EditorFor(x => x.Password) %>
        </td>
        <td>
            <input value="Login" tabindex="4" type="submit" />
        </td>
    </tr>
</table>
<% } %>