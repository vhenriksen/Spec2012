<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tools.aspx.cs" Inherits="Tools" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tools</title>
</head>
<body>
    <form id="formTools" runat="server">
    
    <h3 runat="server" ID="LoginName"></h3>
        <asp:Label ID="LabelTools" runat="server" Text="TOOLS" Font-Bold="True"></asp:Label>
        <asp:GridView ID="GridViewTools" runat="server" AllowPaging="True" 
            DataKeyNames="ID,Tool,Location" 
            onpageindexchanging="GridViewTools_PageIndexChanging" 
            onrowcommand="GridViewTools_RowCommand" PageSize="2">
            <Columns>
                <asp:ButtonField CommandName="AddComment" HeaderText="Comment" Text="Add" />
                <asp:ButtonField CommandName="createReservation" HeaderText="Reservation" 
                    Text="Create" />
            </Columns>
        </asp:GridView>
        <br />
        <asp:Button ID="ButtonLogout" runat="server" onclick="ButtonLogout_Click" 
            Text="Logout" />
    <br />
    <br />
    <asp:Label ID="LabelError" runat="server" ForeColor="Red" Text="Label" 
            Visible="False"></asp:Label>
    
    </form>
</body>
</html>
