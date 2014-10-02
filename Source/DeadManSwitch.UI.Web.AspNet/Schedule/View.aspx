<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet.Schedule.View" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="NoSchedulesLabel" runat="server" Text ="You don't have any schedules." />
    <asp:Repeater ID="ScheduleRepeater" runat="server" OnItemDataBound="ScheduleRepeater_ItemDataBound">
        <HeaderTemplate>
            <h3><asp:Literal runat="server" Text="Schedules" /></h3>
        </HeaderTemplate>
        <ItemTemplate>
            <br />
            <asp:HyperLink runat="server" ID="DeleteHyperLink" CssClass="actionbtn">
                <asp:Image runat="server" ImageUrl="~/Images/delete.png" ImageAlign="AbsMiddle" BorderWidth="0px" AlternateText="Delete this schedule" />
            </asp:HyperLink>
            <asp:HyperLink runat="server" ID="EditHyperLink" CssClass="actionbtn">
                <asp:Image runat="server" ImageUrl="~/Images/edit.png" ImageAlign="AbsMiddle" BorderWidth="0px" AlternateText="Edit this schedule" />
            </asp:HyperLink>
            <asp:Literal runat="server" ID="ScheduleName" />
            <br />
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <asp:HyperLink runat="server" ID="AddHyperLink" CssClass="actionbtn">
        <asp:Image runat="server" ImageUrl="~/Images/add.png" ImageAlign="AbsMiddle" BorderWidth="0px" AlternateText="Create a new schedule" />
    </asp:HyperLink>
    &nbsp;
    <asp:HyperLink runat="server" ID="AddHyperLink2" Text="Create a new schedule" />
    <br />
    <br />
    <asp:Literal ID="NextCheckIn" runat="server" />
</asp:Content>
