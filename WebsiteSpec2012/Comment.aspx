<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Comment.aspx.cs" Inherits="Comment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Make a comment</title>
</head>
<body>
    <form id="formComment" runat="server">
    
    <h3>Make a comment</h3>
        
        <asp:Label ID="LabelTool" runat="server" Text="Label" Font-Bold="True"></asp:Label>
        <br />
        <asp:TextBox ID="TextBoxComment" runat="server" Height="100px" 
            TextMode="MultiLine" Width="180px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="ButtonOk" runat="server" onclick="ButtonOk_Click" Text="Ok" 
            Width="75px" />
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="ButtonCancel" runat="server" onclick="ButtonCancel_Click" 
            Text="Cancel" Width="75px" />
        <br />
        <br />
        <asp:Label ID="LabelError" runat="server" ForeColor="Red" Text="Label" 
            Visible="False"></asp:Label>
    
    
    </form>
</body>
</html>
