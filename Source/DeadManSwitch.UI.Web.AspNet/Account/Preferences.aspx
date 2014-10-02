<%@ Page Title="Preferences" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Preferences.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet.Account.Preferences" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <nav>
        <ul id="menu">
            <li><a runat="server" href="~/Account/Preferences">Preferences</a></li>
            <li><a runat="server" href="~/Account/Profile">Profile</a></li>
            <li><a runat="server" href="~/Account/ChangePassword">Password</a></li>
        </ul>
    </nav>

    <hgroup class="title">
        <h1><asp:Label ID="UserName" runat="server" />'s <%: Title %></h1>
    </hgroup>

    <p class="validation-summary-errors">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <fieldset>
        <legend>Preferences Form</legend>
        <ol>
            <li>
                <asp:Label runat="server" AssociatedControlID="TimeZone">Time zone</asp:Label>
                <asp:DropDownList runat="server" ID="TimeZone" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="TimeZone"
                    CssClass="field-validation-error" ErrorMessage="The time zone field is required." />
            </li>
            <li>
                <asp:Label runat="server" AssociatedControlID="EarlyCheckInMinutes">Early check in minutes</asp:Label>
                <asp:DropDownList runat="server" ID="EarlyCheckInMinutes" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="EarlyCheckInMinutes"
                    CssClass="field-validation-error" ErrorMessage="The early check in minutes field is required." />
            </li>
        </ol>
    </fieldset>
    <asp:Button runat="server" CommandName="Save" Text="Update Preferences" OnClick="Button_Click" />
</asp:Content>
