<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet.Account.Profile" %>
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
        <legend>Profile Information Form</legend>
        <ol>
            <li>
                <asp:Label runat="server" AssociatedControlID="Email">Email</asp:Label>
                <asp:TextBox runat="server" ID="Email" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                    CssClass="field-validation-error" ErrorMessage="The email address field is required." />
            </li>
            <li>
                <asp:Label runat="server" AssociatedControlID="FirstName">First name</asp:Label>
                <asp:TextBox runat="server" ID="FirstName" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName"
                    CssClass="field-validation-error" ErrorMessage="The first name field is required." />
            </li>
            <li>
                <asp:Label runat="server" AssociatedControlID="LastName">Last name</asp:Label>
                <asp:TextBox runat="server" ID="LastName" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="LastName"
                    CssClass="field-validation-error" ErrorMessage="The Last name field is required." />
            </li>
        </ol>
    </fieldset>
    <asp:Button ID="Button1" runat="server" CommandName="Save" Text="Update Profile" OnClick="Button_Click" />

</asp:Content>
