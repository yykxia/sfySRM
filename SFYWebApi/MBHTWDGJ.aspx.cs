using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Specialized;

namespace SFYWebApi
{
    public partial class MBHTWDGJ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DisplayData();
            }
        }

        public void DisplayData()
        {
            if (Request["uCode"] != null && Request["mType"] != null)
            {
                string uCode = Request["uCode"].ToString();
                object otmp = SqlSel.GetSqlScale(" select top 1 AddedTime from WDApi_logs order by AddedTime desc");
                string addtime = "";
                if (otmp != null)
                {
                    addtime = otmp.ToString();
                }
                string beginTime = addtime;
                DateTime endTime = DateTime.Now;
                if (1 == 1)
                {
                    if (Request["mType"] == "mOrderSearch" && uCode ==SqlSel.getcipher())
                    {
                        Response.Write(wdgjV3API.mOrderSearch(SqlSel.getcipher(),beginTime,endTime.ToString(),1));
                        Response.End();
                    }

                    else if (Request["mType"] == "mGetOrder" && uCode == SqlSel.getcipher())
                    {
                        string OrderNO = Request["OrderNO"].ToString();
                        byte[] buffer = Encoding.Default.GetBytes(wdgjV3API.mGetOrder(uCode,OrderNO));
                        string strDest = Encoding.GetEncoding("gb2312").GetString(buffer);
                        Response.Clear();

                        Response.ContentType = "text/xml";
                        Response.Write("<?xml version='1.0' encoding='UTF-8'?>" + strDest);

                        Response.End();

                    }
                    else if (Request["mType"] == "mSndGoods" && uCode == SqlSel.getcipher())
                    {
                        //string orderlist = "";
                        string OrderNO = Request["OrderNO"].ToString();
                        string SndStyle = Request["SndStyle"].ToString();
                        //string SndStyle = System.Text.Encoding.UTF8.GetBytes(sndsty).ToString();
                        string BillID = Request["BillID"].ToString();
                        //if (SndStyle.Contains("德邦"))
                        //{
                        //    orderlist = "debang";
                        //}
                        //else if (SndStyle.Contains("申通"))
                        //{
                        //    orderlist = "shentong";
                        //}
                        //else if (SndStyle.Contains("顺丰"))
                        //{
                        //    orderlist = "shunfeng";
                        //}
                        //else if (SndStyle.Contains("中通"))
                        //{
                        //    orderlist = "zhongtong";
                        //}
                        //else if (SndStyle.Contains("EMS"))
                        //{
                        //    orderlist = "ems";
                        //}
                        //else if (SndStyle.Contains("优速"))
                        //{
                        //    orderlist = "yousu";
                        //}
                        //else
                        //{
                        //    orderlist = "";
                        //}

                        string xml = wdgjV3API.mSndGoods(OrderNO, SndStyle, BillID);
                        Response.Write(xml);
                        Response.End();
                    }
                    else
                    {
                        Response.Write(wdgjV3API.xml(0, "canshu error"));
                        Response.End();
                    }
                }
                else
                {
                    Response.Write(wdgjV3API.xml(0, "uCode Error"));
                    Response.End();
                }
            }
            else
            {
                Response.Write(wdgjV3API.xml(0, "uCode Error or uCode is null"));
                Response.End();
            }
        }
    }
}
