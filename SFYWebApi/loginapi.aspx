<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="loginapi.aspx.cs" Inherits="SFYWebApi.loginapi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link type="text/css" href="CSS/master2.css" rel="Stylesheet" />
    <link type="text/css" href="CSS/login2.css" rel="Stylesheet" />

</head>
<body>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="login" runat="server">
  <tr>
    <td><table width="560" border="0" align="center" cellpadding="0" cellspacing="0">
      <tr>
        <td>
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td align="right">
                <img src="images/login-logo.png" alt="logo" />
                <span class="cssVerson"></span>
              </td>
              <td class="cssLine">&nbsp;</td>
              <td valign="top"><form runat="server" id="ftbl">
                <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                  <tr>
                    <td><table width="90%" border="0" align="center" cellpadding="0" cellspacing="0" class="cssTitle">
                      <tr>
                        <td><h1>登录</h1></td>
                      </tr>
                    </table>
                      <table width="100%" border="0" cellspacing="0" cellpadding="0" class="cssMain">
                        <tr>
                          <td valign="top" class="cssLabel"> 用户名：</td>
                          <td class="cssInput"><input name="userid" type="text" class="input" runat="server" id="userid" autocomplete="off" /></td>
                        </tr>
                        <tr>
                          <td valign="top" class="cssLabel">密码：</td>
                          <td class="cssInput"><input name="txtpwd" type="password" runat="server"  class="input" id="txtpwd" />
                           </td>
                        </tr>                      
                          </table></td>
                        </tr>
                        <tr>
                          <td style="padding-left:50px">
                             <%-- <input id="submit" class="cssBtn" name="submit" onclick="return submit_onclick()" type="button" value="登录" />--%>
                              <asp:Button ID="btnLogin" runat="server" Text="登 录" CssClass="cssBtn" 
                                  onclick="btnLogin_Click" />&nbsp;&nbsp;
                        <input type="reset" name="reset" id="reset" class="cssBtn" onclick="doReset();"  value="取消"/></td>
                          <td  style="height:38px">
                         
                       </td>
                        </tr>
                        <tr>
                          <td>&nbsp;</td>
                          <td> <span style="color:Red" id="msg_tip"></span></td>
                        </tr>
                      </table>
                 </form></td>
                    </tr>
                </table>
             </td>
            </tr>
          </table></td>
      </tr>
    </table>     
</body>
</html>
