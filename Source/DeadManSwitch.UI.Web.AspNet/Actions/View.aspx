<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet.Actions.View" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="NoActionsLabel" runat="server" Text ="You don't have any escalation steps defined." />
    <asp:Repeater ID="ActionRepeater" runat="server" OnItemDataBound="ActionRepeater_ItemDataBound">
        <HeaderTemplate>
            <h3><asp:Literal runat="server" Text="Escalation Steps" /></h3>
        </HeaderTemplate>
        <ItemTemplate>
            <br />
<%--            <asp:HyperLink runat="server" ID="DeleteHyperLink" CssClass="actionbtn">
                <asp:Image runat="server" ImageUrl="~/Images/delete.png" ImageAlign="AbsMiddle" BorderWidth="0px" AlternateText="Delete this schedule" />
            </asp:HyperLink>
            <asp:HyperLink runat="server" ID="EditHyperLink" CssClass="actionbtn">
                <asp:Image runat="server" ImageUrl="~/Images/edit.png" ImageAlign="AbsMiddle" BorderWidth="0px" AlternateText="Edit this schedule" />
            </asp:HyperLink>--%>
            <asp:Label runat="server" ID="StepNumber" />.&nbsp;
            <asp:Literal runat="server" ID="StepDescription" />
            <br />
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <asp:HyperLink runat="server" ID="EditHyperLink" CssClass="actionbtn" NavigateUrl="~/Actions/Edit.aspx"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/edit.png" ImageAlign="Left" BorderWidth="0px" AlternateText="Add a step" /></asp:HyperLink>
    <asp:HyperLink runat="server" ID="EditHyperLink2" Text="Edit steps" NavigateUrl="~/Actions/Edit.aspx" />
</asp:Content>
