<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckIn.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet.CheckIn" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <asp:Label ID="CheckInStatus" runat="server" /> Your next scheduled check in is <asp:Label ID="NextScheduledCheckIn" runat="server" />
    </p>

</asp:Content>
