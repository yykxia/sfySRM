<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SFYWebApi._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <script src="JS/DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div><a style="font-size:12px" href="EditPwd.aspx">修改密码</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="font-size:12px" href="DDCXRQ.aspx">预计发货日期</a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="font-size:12px" href="adminexit.aspx">退出系统</a> </div>
    <div>
    下单日期：<input runat="server" type="text" class="input Wdate" id="txtNeedDate" size="12" style="width: 128px;"
                            onclick="WdatePicker()" />至
                            <input runat="server" type="text" class="input Wdate" id="Text1" size="12" style="width: 128px;"
                            onclick="WdatePicker()" />
    <asp:Button ID="Button1" runat="server" Text="获取数据" onclick="Button1_Click" />
    <asp:Button ID="Button2" runat="server" Text="同步数据" Enabled=false OnClick="Button2_Click" />
    <asp:Label  ID="Label1" runat="server" Text=""></asp:Label>
    <br /><asp:Label ID="Label2" runat="server" Text=""></asp:Label>
        <asp:Label ID="Label3" runat=server Text=""></asp:Label>
        <br />
          <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="HEADER_ID" HeaderText="HEADER_ID" />
                                <asp:BoundField DataField="LINE_ID" HeaderText="LINE_ID" />
                                <asp:BoundField DataField="PO_NUMBER" HeaderText="PO_NUMBER" />
                                <asp:BoundField DataField="CUST_PO_NUM" HeaderText="CUST_PO_NUM" />
                                <asp:BoundField DataField="ITEM_NUMBER" HeaderText="ITEM_NUMBER" />
                                <asp:BoundField DataField="ITEM_DESC" HeaderText="ITEM_DESC" />
                                <asp:BoundField DataField="NEED_BY_DATE" HeaderText="NEED_BY_DATE" />
                                <asp:BoundField DataField="CREATION_DATE" HeaderText="CREATION_DATE" />
                                <asp:BoundField DataField="QUANTITY" HeaderText="QUANTITY" />
                                <asp:BoundField DataField="transaction_quantity" HeaderText="transaction_quantity" />
                                <asp:BoundField DataField="UOM_CODE" HeaderText="UOM_CODE" />
                                <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" />
                                <asp:BoundField DataField="CONTACT_NAME" HeaderText="CONTACT_NAME" />
                                <asp:BoundField DataField="PHONE_NUMBER" HeaderText="PHONE_NUMBER" />
                                <asp:BoundField DataField="ADDRESS" HeaderText="ADDRESS" />
                                <asp:BoundField DataField="SPECIFICATION" HeaderText="SPECIFICATION" />
                            </Columns>
                        </asp:GridView>
       <%-- <asp:Button ID="Button2" runat="server" Text="获取数据至索菲亚库" onclick="Button2_Click" />
        
        --%>
        <asp:Label ID="Label4" runat=server Text=""></asp:Label>
    </div>
    
    </form>
</body>
</html>
