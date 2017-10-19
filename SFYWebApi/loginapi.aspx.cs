using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SFYWebApi
{
    public partial class loginapi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DataTable newdt = new DataTable();
            string sqlCmd = "select * from APILogin where Acount='" + userid.Value + "'";
            SqlSel.GetSqlSel(ref newdt, sqlCmd);
            if (newdt.Rows.Count == 0)
            {
                Response.Write("<script>alert('您输入的用户名不存在！')</script>");
                return;
            }
            if (newdt.Rows[0]["Pwd"].ToString() == BPEycrypt.EncryptAdmin(txtpwd.Value))
            {
                HttpCookie cookieName = new HttpCookie("UserName");
                cookieName.Value = userid.Value;
                Response.AppendCookie(cookieName);
                HttpCookie cookieName2 = new HttpCookie("Pwd");
                cookieName2.Value = txtpwd.Value;
                Response.AppendCookie(cookieName2);
                string strsql = " update APILogin set IPAddress='" + IPHelp.ClientIP
               + "',LoginTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
               + "' where Acount='" + userid.Value + "' ";
                int execounts = SqlSel.ExeSql(strsql);
                Response.Redirect("Default.aspx");
            }
            else
            {
                Response.Write("<script>alert('您输入的密码有误！')</script>");
            }
        }
    }
}
