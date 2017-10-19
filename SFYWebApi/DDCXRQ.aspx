<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DDCXRQ.aspx.cs" Inherits="SFYWebApi.DDCXRQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="JS/DatePicker/WdatePicker.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <a style="font-size: 12px" href="EditPwd.aspx">修改密码</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a
                style="font-size: 12px" href="Default.aspx">获取数据</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a
                    style="font-size: 12px" href="adminexit.aspx">退出系统</a>
        </div>
        <div>
            PO单号：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            &nbsp;&nbsp;<asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem Value="0">下单日期</asp:ListItem>
                <asp:ListItem Value="1">需求日期</asp:ListItem>
            </asp:DropDownList>
            <input runat="server" type="text" class="input Wdate" id="txtNeedDate"
                size="12" style="width: 128px;" onclick="WdatePicker()" />至
            <input runat="server" type="text" class="input Wdate" id="Text1" size="12" style="width: 128px;"
                onclick="WdatePicker()" /><asp:CheckBox ID="CheckBox1" Text="已同步" Font-Size="12px" runat="server" />
            <asp:Button ID="Button1" runat="server" Text="查询数据" OnClick="Button1_Click" />&nbsp;&nbsp;
            <asp:Button ID="Button2" runat="server" Text="保存数据" OnClick="Button2_Click" />&nbsp;&nbsp;
            <asp:Button ID="Button3" runat="server" Text="同步预计日期" OnClick="Button3_Click" />&nbsp;&nbsp;
           <br /> <asp:Button ID="Button4" runat="server" Text="导出Excel" 
                onclick="Button4_Click" />
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
            <br />
            <asp:GridView ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                Font-Size="13px">
                <Columns>
                    <asp:TemplateField HeaderText="序号" InsertVisible="False">
                        <ItemTemplate>
                            <%#Container.DataItemIndex+1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CUST_PO_NUM" HeaderText="PO单号" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="ITEM_NUMBER" HeaderText="商品编码" />
                    <asp:BoundField DataField="ITEM_DESC" HeaderText="商品名称" />
                    <asp:BoundField DataField="NEED_BY_DATE" HeaderText="需求日期" />
                    <asp:BoundField DataField="CREATION_DATE" HeaderText="下单日期" />
                    <asp:BoundField DataField="QUANTITY" HeaderText="数量" />
                    <asp:BoundField DataField="UOM_CODE" HeaderText="单位" HeaderStyle-Width="40px" />
                    <asp:TemplateField HeaderText="预计发货日期">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%#Eval("YJFHRQ")%>' class="Wdate"
                                onclick="WdatePicker()"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField><asp:TemplateField HeaderText="发货备注">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBZRMK" Height="50px" Width="120px" Text='<%#Eval("Remark")%>'
                                    runat="server" TextMode="MultiLine"></asp:TextBox>  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CONTACT_NAME" HeaderText="联系人" />
                    <asp:BoundField DataField="PHONE_NUMBER" HeaderText="电话" />
                    <asp:BoundField DataField="ADDRESS" HeaderText="地址" HeaderStyle-Width="150px" />
                    <asp:BoundField DataField="HEADER_ID" HeaderStyle-Width="1px" HeaderText="HEADER_ID" />
                    <asp:BoundField DataField="LINE_ID"  HeaderStyle-Width="1px" HeaderText="LINE_ID" />
                    <asp:BoundField DataField="OrderID" HeaderStyle-Width="1px" HeaderText="OrderID" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
