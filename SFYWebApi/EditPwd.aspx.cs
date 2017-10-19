using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SFYWebApi
{
    public partial class EditPwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckAdminLogin())
                Response.Write(" <script  language='javascript'> window.top.location='loginapi.aspx'</script> ");
        }
        /// <summary>
        /// 管理员是否登录了
        /// </summary>
        /// <returns></returns>
        public bool CheckAdminLogin()
        {
            if (Request.Cookies["UserName"] != null && Request.Cookies["Pwd"] != null)
                return true;
            else
                return false;
        }

        protected void btnCon_Click(object sender, EventArgs e)
        {
            string relpwd = this.txtrelpwd.Value.Trim();
            string newpwd = this.txtnewpwd.Value.Trim();
            string chkpwd = this.txtchkpwd.Value.Trim();

            if (string.IsNullOrEmpty(relpwd) || string.IsNullOrEmpty(newpwd) || string.IsNullOrEmpty(chkpwd))
            {
                Response.Write("<script>alert('输入不完整或网络错误！');window.location='EditPwd.aspx'</script>");
            }
            if (newpwd != chkpwd)
            {
                Response.Write("<script>alert('新密码两次输入不一致！');window.location='EditPwd.aspx'</script>");
            }
            if (newpwd.Length < 6 || newpwd.Length > 16)
            {
                Response.Write("<script>alert('新密码长度与要求不符！');window.location='EditPwd.aspx'</script>");
            }
            string username = Request.Cookies["UserName"].Value.ToString();
            string strsql = " update APILogin set Pwd='" +BPEycrypt.EncryptAdmin(newpwd)
                + "' where Acount='" + username + "' and Pwd='" + BPEycrypt.EncryptAdmin(relpwd) + "' ";
            int execounts = SqlSel.ExeSql(strsql);
            if (execounts > 0)
            {
                lblMeassge.Visible = true;
                lblMeassge.Text = "修改成功！";
            }
            else
            {
                lblMeassge.Visible = false;
                Response.Write("<script>alert('修改失败！');window.location='EditPwd.aspx'</script>");
            }
        }
    }
}
