<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="DeadManSwitch.UI.Web.AspNet.Schedule.Edit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Literal runat="server" ID="InvalidSchedule" Text="Either the schedule does not exist, or you are not allowed to change it." />

    <div runat="server" ID="EditScheduleSection">
        <p class="validation-summary-errors">
            <asp:Literal runat="server" ID="ErrorMessage" />
        </p>

        <asp:HiddenField ID="ScheduleId" runat="server" Value="-1" />
        <fieldset>
            <legend>Check In Schedule</legend>
            <ol>
                <li>
                    <asp:Label runat="server" AssociatedControlID="ScheduleName">Name</asp:Label>
                    <asp:TextBox runat="server" ID="ScheduleName" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ScheduleName"
                        CssClass="field-validation-error" ErrorMessage="The name field is required." />
                </li>
                <li>
                    <asp:CheckBox runat="server" ID="ScheduleEnabled" Text="Enabled" Checked="true"  CssClass="checkbox_nowrap" />
                </li>
                <li>
                    <asp:Label runat="server" AssociatedControlID="ScheduleDays">Days</asp:Label>
                    <div runat="server" ID="ScheduleDays">
                        <asp:CheckBox runat="server" ID="SundayCheckbox" Text="Su" CssClass="checkbox_nowrap" />
                        <asp:CheckBox runat="server" ID="MondayCheckbox" Text="M" CssClass="checkbox_nowrap" />
                        <asp:CheckBox runat="server" ID="TuesdayCheckbox" Text="Tu" CssClass="checkbox_nowrap" />
                        <asp:CheckBox runat="server" ID="WednesdayCheckbox" Text="W" CssClass="checkbox_nowrap" />
                        <asp:CheckBox runat="server" ID="ThursdayCheckbox" Text="Th" CssClass="checkbox_nowrap" />
                        <asp:CheckBox runat="server" ID="FridayCheckbox" Text="F" CssClass="checkbox_nowrap" />
                        <asp:CheckBox runat="server" ID="SaturdayCheckbox" Text="Sa" CssClass="checkbox_nowrap" />
                    </div>
                </li>
                <li>
                    <asp:Label runat="server" AssociatedControlID="CheckInStartHour">Between</asp:Label>
                    <div runat="server" ID="ScheduleEarlyCheckInTime" class="schedule_time_div">
                        <asp:DropDownList runat="server" ID="CheckInStartHour" />
                        :
                        <asp:DropDownList runat="server" ID="CheckInStartMinute" />
                        <asp:DropDownList runat="server" ID="CheckInStartAmPm" />
                    </div>
                </li>
                <li>
                    <asp:Label runat="server" AssociatedControlID="CheckInEndHour">And</asp:Label>
                    <div runat="server" ID="ScheduleTime" class="schedule_time_div">
                        <asp:DropDownList runat="server" ID="CheckInEndHour" />
                        :
                        <asp:DropDownList runat="server" ID="CheckInEndMinute" />
                        <asp:DropDownList runat="server" ID="CheckInEndAmPm" />
                    </div>
                </li>
                <li>
                    <asp:Label runat="server" AssociatedControlID="UserTimeZone">Time zone</asp:Label>
                    <asp:Label ID="UserTimeZone" runat="server" />
                </li>
            </ol>
        </fieldset>
        <asp:Button runat="server" CommandName="Save" Text="Done" OnClick="Button_Click" />
        <asp:Button runat="server" CommandName="Cancel" Text="Cancel" OnClick="Button_Click" CausesValidation="false" />
    </div>
</asp:Content>
