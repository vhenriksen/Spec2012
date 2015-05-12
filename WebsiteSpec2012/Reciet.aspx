<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reciet.aspx.cs" Inherits="Reciet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="formReciet" runat="server">
    <h3>Reservation resume:</h3>
    <p>
        <asp:Label ID="LabelResume" runat="server" Text="Label"></asp:Label>
    </p>
    <p>
        <asp:Button ID="ButtonOk" runat="server" onclick="ButtonOk_Click" Text="Ok" 
            Width="75px" />
    </p>
    </form>
</body>
</html>
