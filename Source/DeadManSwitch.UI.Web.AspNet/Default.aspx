<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><asp:Label ID="FeaturedMessage" runat="server" /></h1>
                <asp:Button ID="CheckInNow" runat="server" Text="Check In Now!" CommandName="CheckInNow" OnClick="Button_Click" CssClass="featuredcheckinnow" />
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">    
    <article>
        Periodic CheckIn is an electronic dead man's switch that will send out emails or text messages, to recipients you specify, if you don't check in.
        <ul>
            <li>text your spouse if you're stuck at work and will be home later than usual</li>
            <li>text your roommate to wake you up if you oversleep</li>
        </ul>

        <p>
            <br />
            <br />
            Periodic CheckIn should not be used for emergency, medical, or other similar situations.
        </p>
    </article>

    <aside>
        <h3>Contact Information</h3>
        <p>
            <span><a href="mailto:support@periodiccheckin.com">support@periodiccheckin.com</a></span>
        </p>
    </aside>

</asp:Content>
