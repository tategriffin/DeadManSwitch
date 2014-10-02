<%@ Page Title="Create a new account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <p class="validation-summary-errors">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div id="RegistrationSection" runat="server">
    <fieldset>
        <legend>Registration Form</legend>
        <ol>
            <li>
                <asp:Label runat="server" AssociatedControlID="UserName">User name</asp:Label>
                <asp:TextBox runat="server" ID="UserName" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                    CssClass="field-validation-error" ErrorMessage="The user name field is required." />
            </li>
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
            <li>
                <asp:Label runat="server" AssociatedControlID="Password">Password</asp:Label>
                <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="field-validation-error" ErrorMessage="The password field is required." />
            </li>
            <li>
                <asp:Label runat="server" AssociatedControlID="ConfirmPassword">Confirm password</asp:Label>
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                        CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                        CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
            </li>
            <li>
                <asp:CheckBox runat="server" ID="RememberMe" />
                <asp:Label runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Remember me?</asp:Label>
            </li>
        </ol>
    </fieldset>
    <asp:Button runat="server" CommandName="Register" Text="Register" OnClick="Button_Click" />
    </div>
</asp:Content>
