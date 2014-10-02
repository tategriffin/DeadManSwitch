<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet.Actions.Edit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" ID="EditScheduleSection">
        <p class="validation-summary-errors">
            <asp:Literal runat="server" ID="ErrorMessage" />
        </p>

        <asp:Repeater ID="ActionRepeater" runat="server" OnItemDataBound="ActionRepeater_ItemDataBound">
            <HeaderTemplate>
                <h3><asp:Literal runat="server" Text="If I miss a check in" /></h3>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="StepId" />
                <asp:Label runat="server" ID="StepNumber" />.&nbsp;
                wait <asp:DropDownList ID="StepWaitMinutes" runat="server" /> minutes
                and then <asp:DropDownList ID="StepAction" runat="server" /> to
                <asp:TextBox ID="StepRecipient" runat="server" Width="200" />
                <br />
            </ItemTemplate>
        </asp:Repeater>
        <br />
        <asp:Button runat="server" CommandName="Save" Text="Done" OnClick="Button_Click" />
        <asp:Button runat="server" CommandName="Cancel" Text="Cancel" OnClick="Button_Click" CausesValidation="false" />
    </div>
</asp:Content>
