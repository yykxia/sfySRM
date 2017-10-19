<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPwd.aspx.cs" Inherits="SFYWebApi.EditPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
     <div><a style="font-size:12px" href="Default.aspx">获取数据</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="font-size:12px" href="DDCXRQ.aspx">预计发货日期</a><a style="font-size:12px" href="adminexit.aspx">退出系统</a> </div>
        <div id="content">
            <table border="0" cellspacing="1" cellpadding="3" class="table">
                <tr>
                    <th colspan="2" id="td_tag">
                        修改密码
                    </th>
                </tr>
                <tr>
                    <td width="15%" align="right" class="td1">
                        原始密码：
                    </td>
                    <td>
                        <input runat="server" type="password" class="input" id="txtrelpwd" /><span style="color:#ff0000;">*</span>
                    </td>
                </tr>
                <tr>
                    <td width="15%" align="right" class="td1">
                        新密码：
                    </td>
                    <td>
                        <input runat="server" type="password" id="txtnewpwd" class="input" /><span style="color:#ff0000;">6~16位*</span></td>
                </tr>
                <tr>
                    <td width="15%" align="right" class="td1">
                        重复密码：
                    </td>
                    <td>
                        <input type="password" runat="server" maxlength="10" class="input" id="txtchkpwd" /><span style="color:#ff0000;">*</span>
                           
                        <asp:Label ID="lblMeassge" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button runat="server" ID="btnCon" class="button_custom" Text="提交数据" OnClientClick="return Checkinfo();"
                            OnClick="btnCon_Click" />&nbsp;
                        <input type="button" id="btnCancel" class="button_custom" value="取消"  />
                    </td>
                </tr>
            </table>
        </div>
    </form>
     <script src="JS/tools.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Checkinfo(){
            var a=document.getElementById("txtrelpwd");
            var b=document.getElementById("txtnewpwd");
            var c=document.getElementById("txtchkpwd");
            if(trim(a.value)==""||trim(b.value)==""||trim(c.value)==""){
                alert("请完整输入");
                return false;
            }
            if(trim(b.value).length<6||trim(b.value).length>16){
                alert("新密码输入长度与要求不符");
                return false;
            }
            if(b.value!=c.value){
                alert("新密码两次输入不一致");
                return false;
            }
            
            return true;
        }
    </script>
</body>
</html>
