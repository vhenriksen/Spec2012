<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Tool reservation login</title>
</head>
<body>
    <form id="formLogin" runat="server">
    <div>
    
        <asp:Label ID="LabelId" runat="server" Text="ID"></asp:Label>
        <br />
        <asp:TextBox ID="TextBoxId" runat="server"></asp:TextBox>
        <asp:RangeValidator ID="RangeValidatorIsIdInteger" runat="server" 
            ControlToValidate="TextBoxId" Display="Dynamic" 
            ErrorMessage="ID must be a number" ForeColor="Red" MaximumValue="2000000000" 
            MinimumValue="0" Type="Integer"></asp:RangeValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorIsIdMissing" runat="server" 
            ControlToValidate="TextBoxId" Display="Dynamic" ErrorMessage="ID is missing" 
            ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Label ID="LabelPassword" runat="server" Text="Password"></asp:Label>
        <br />
        <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidatorIsPasswordMissing" runat="server" 
            ControlToValidate="TextBoxPassword" Display="Dynamic" 
            ErrorMessage="Password is missing" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Button ID="ButtonLogin" runat="server" onclick="ButtonLogin_Click" 
            Text="Login" Width="128px" />
    
        <br />
        <br />
        <asp:Label ID="LabelError" runat="server" ForeColor="Red" Text="Label" 
            Visible="False"></asp:Label>
    
    </div>
    </form>
</body>
</html>
