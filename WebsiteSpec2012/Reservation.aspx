<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reservation.aspx.cs" Inherits="Reservation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create reservation</title>
</head>
<body>
    <form id="form1" runat="server">
    
        <table width="400px">
             <tr>
                <td colspan="2" class="style1">
                    <h3>Make reservation</h3>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    
                    <asp:Label ID="LabelTool" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
                <td >
                    <asp:Label ID="Label4" runat="server" Text="Start"></asp:Label>
                    <asp:Calendar ID="CalendarDateStart" runat="server" Width="180px"></asp:Calendar>
                    <br />
                    <asp:Label ID="Label6" runat="server" Text="Hour to start"></asp:Label>
                    <br />
                    <asp:DropDownList ID="DropDownListStartHour" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
                <td >
                    <asp:Label ID="Label5" runat="server" Text="End"></asp:Label>
                    <asp:Calendar ID="CalendarDateEnd" runat="server" Width="180px"></asp:Calendar>
                    <br />
                    <asp:Label ID="Label7" runat="server" Text="Hour to end"></asp:Label>
                    <br />
                    <asp:DropDownList ID="DropDownListEndhour" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" >
                    
                    <br />
                    
                    <asp:Label ID="LabelHoursWanted" runat="server" Text="Hours wanted"></asp:Label>
                    <br />
                    <asp:TextBox ID="TextBoxHoursWanted" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonFindDate" runat="server" Text="Find next availible date" 
                        onclick="ButtonFindDate_Click" />
                    
                </td>
            </tr>
            <tr>
                <td colspan="2" >
                    
                    
                    <br />
                    <asp:Label ID="LabelLocation" runat="server" Text="Location" Width="150px"></asp:Label>
                    <br />
                    <asp:DropDownList ID="DropDownListLocation" runat="server" Width="150px">
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    
                    <asp:CheckBox ID="CheckBoxIsService" AutoPostBack="true" runat="server" 
                        oncheckedchanged="CheckBoxIsService_CheckedChanged" Text=" Service" />
                    
                    
                </td>
            </tr>
            <tr>
                <td colspan="2" >
                    <br />
                    <asp:Button ID="ButtonConfirm" runat="server" Text="Confirm" Width="75px" 
                        onclick="ButtonConfirm_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" Width="75px" 
                        onclick="ButtonCancel_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2" >
                    <br />
                    <asp:Label ID="LabelError" runat="server" Text="Label" ForeColor="Red" 
                        Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
    
    </form>
</body>
</html>
