<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddressMap.aspx.cs" Inherits="SFYWebApi.AddressMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            Height="111px" Width="791px">
            <Columns>
                <asp:BoundField HeaderText="地址" DataField="Address" />
                <asp:BoundField HeaderText="省" DataField="sendProvince" />
                <asp:BoundField HeaderText="市" DataField="sendCity" />
                <asp:BoundField HeaderText="区" DataField="sendArea" />
            </Columns>
        </asp:GridView>
    
    </div>
    <asp:TextBox ID="TextBox1" runat="server" Height="22px" Width="128px"></asp:TextBox>
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Button" />
    </form>
</body>
</html>
