using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SFYWebApi
{
    public partial class adminexit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Clear Cookies
                if (Request.Cookies["UserName"] != null)
                {
                    HttpCookie myCookie = Request.Cookies["UserName"];
                    myCookie.Expires = DateTime.Today.AddDays(-1);
                    Response.Cookies.Add(myCookie);

                }
                if (Request.Cookies["Pwd"] != null)
                {
                    HttpCookie myCookie = Request.Cookies["Pwd"];
                    myCookie.Expires = DateTime.Today.AddDays(-1);
                    Response.Cookies.Add(myCookie);

                }
                #endregion
                Response.Write(" <script  language='javascript'> window.top.location='loginapi.aspx'</script> ");
            }
        }
    }
}
