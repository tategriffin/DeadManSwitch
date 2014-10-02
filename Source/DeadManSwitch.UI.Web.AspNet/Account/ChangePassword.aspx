<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet.Account.ChangePassword" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <nav>
        <ul id="menu">
            <li><a runat="server" href="~/Account/Preferences">Preferences</a></li>
            <li><a runat="server" href="~/Account/Profile">Profile</a></li>
            <li><a runat="server" href="~/Account/ChangePassword">Password</a></li>
        </ul>
    </nav>

    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <p class="validation-summary-errors">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <asp:Literal runat="server" ID="SuccessMessage" Visible="false" />

    <fieldset>
        <legend>Change Password Form</legend>
        <ol>
            <li>
                <asp:Label runat="server" AssociatedControlID="CurrentPassword">Current password</asp:Label>
                <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                    CssClass="field-validation-error" ErrorMessage="The current password field is required." />
            </li>
            <li>
                <asp:Label runat="server" AssociatedControlID="Password">New password</asp:Label>
                <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="field-validation-error" ErrorMessage="The new password field is required." />
            </li>
            <li>
                <asp:Label runat="server" AssociatedControlID="ConfirmPassword">Confirm new password</asp:Label>
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                        CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The confirm new password field is required." />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                        CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
            </li>
        </ol>
    </fieldset>
    <asp:Button runat="server" CommandName="ChangePassword" Text="Change Password" OnClick="Button_Click" />
</asp:Content>
