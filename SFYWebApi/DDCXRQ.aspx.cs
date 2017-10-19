using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;


namespace SFYWebApi
{
    public partial class DDCXRQ : System.Web.UI.Page
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
        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = "";

            GridView1.DataSource = getdt();
            GridView1.DataBind();
        }
        private DataTable getdt()
        {
            DataTable dtb = new DataTable();
            string sqlCmd = "select t1.*,t2.YJFHRQ,t2.Remark from SFYOrderTab t1 left join yjfhtab t2 on t1.OrderId=t2.OrderID  ";
            string sqlwhr = "";
            if (CheckBox1.Checked == true)
            {
                sqlwhr += " where FKSate=1 ";
            }
            else
            {
                sqlwhr += " where (FKSate=0 or FKSate is null or  FKSate=1) ";
            }
            if (TextBox1.Text != "")
            {
                sqlwhr += " and CUST_PO_NUM like '%" + TextBox1.Text + "%' ";
            }
            if (txtNeedDate.Value != "")
            {
                if (DropDownList1.SelectedValue == "0")
                {
                    sqlwhr += " and t1.CREATION_DATE  between'" + txtNeedDate.Value + "' and '" + Text1.Value + "' ";
                }
                else
                {
                    sqlwhr += " and t1.NEED_BY_DATE  between'" + txtNeedDate.Value + "' and '" + Text1.Value + "' ";
                }
            }

            SqlSel.GetSqlSel(ref dtb, sqlCmd + sqlwhr);
            return dtb;
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string sqlcmd = ""; string sqltr = ""; 
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                string hdid = this.GridView1.Rows[i].Cells[13].Text;
                string lineid = this.GridView1.Rows[i].Cells[14].Text;
                string orderid = this.GridView1.Rows[i].Cells[15].Text;
                string fhrq = ((TextBox)(GridView1.Rows[i].Cells[0].Controls[0]).FindControl("TextBox3")).Text.Trim().ToString();
                string bzrmk = ((TextBox)(GridView1.Rows[i].Cells[0].Controls[0]).FindControl("txtBZRMK")).Text.Trim().ToString();   
                //TextBox txb = GridView1.Rows[i].Cells[8].FindControl("TextBox3") as TextBox;
                //string data = txb.Text.Trim();
                if (fhrq != "" || bzrmk!="")
                {
                    sqltr = "select YJFHRQ from yjfhtab where OrderID='" + orderid + "' and FKSate=0 ";//未同步到索菲亚
                    String YJFH = Convert.ToString(SqlSel.GetSqlScale(sqltr));
                    sqltr = "select YJFHRQ from yjfhtab where OrderID='" + orderid + "' and FKSate=1 ";//已同步到索菲亚
                    String YJFH2 = Convert.ToString(SqlSel.GetSqlScale(sqltr));
                    if (YJFH != "")//可更新
                    {
                        sqlcmd = "update YJFHTab set Header_ID='" + hdid + "',Line_ID='" + lineid + "',YJFHRQ='" + fhrq + "',Remark='" + bzrmk + "',FKSate=0 where OrderID='" + orderid + "'  ";
                        int execountsp = SqlSel.ExeSql(sqlcmd);
                        if (execountsp == 0)
                        {
                            break;
                        }
                    }
                    else if (YJFH2 != "")//直接跳出
                    {
                        break;
                    }
                    else
                    {
                        sqlcmd = "insert into YJFHTab(Header_ID,Line_ID,YJFHRQ,Remark,FKSate,OrderID) values('" + hdid + "','" + lineid + "','" + fhrq + "','" + bzrmk + "',0,'" + orderid + "')  ";
                        int execounts = SqlSel.ExeSql(sqlcmd);
                        if (execounts == 0)
                        {
                            break;
                        }
                    }
                }
            }
            Label1.Text = "保存成功。";
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            net.sogal.www.WebService aa = new SFYWebApi.net.sogal.www.WebService();
            DataTable newdt = new DataTable();
            string sqlCmd = "select * from YJFHTab where FKSate=0 ";
            SqlSel.GetSqlSel(ref newdt, sqlCmd);
            if (newdt.Rows.Count == 0)
            {
                Label2.Text = "所有订单预计发货日期已同步，请稍后再试。";
                return;
            }
            for (int ii = 0; ii < newdt.Rows.Count; ii++)
            {
                string YJID = newdt.Rows[ii]["YJID"].ToString();
                aa.AddPoRequisition(Int32.Parse(newdt.Rows[ii]["Header_ID"].ToString()), Int32.Parse(newdt.Rows[ii]["Line_ID"].ToString()), 0, "预计发货日期：" + newdt.Rows[ii]["YJFHRQ"].ToString() + "发货备注：" + newdt.Rows[ii]["Remark"].ToString());//传值
                string sqlstr = "update YJFHTab set [FKSate]=1 where [YJID]='" + YJID + "' ";
                int execounts = SqlSel.ExeSql(sqlstr);
                if (execounts == 0)
                {
                    break;
                }
            }
            Label2.Text = "预计发货日期 同步已完成！";
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            DataTable dt = getdt();
            Aspose.Cells.Workbook workBook = new Aspose.Cells.Workbook();
            Aspose.Cells.Worksheet sheet = workBook.Worksheets[0];
            if (dt.Rows.Count > 0)
            {
                sheet.Cells[0, 0].PutValue("PO单号");
                sheet.Cells[0, 1].PutValue("商品编码");
                sheet.Cells[0, 2].PutValue("商品名称");
                sheet.Cells[0, 3].PutValue("需求日期");
                sheet.Cells[0, 4].PutValue("下单日期");
                sheet.Cells[0, 5].PutValue("数量");
                sheet.Cells[0, 6].PutValue("单位");
                sheet.Cells[0, 7].PutValue("预计发货日期");
                sheet.Cells[0, 8].PutValue("发货备注");
                sheet.Cells[0, 9].PutValue("联系人");
                sheet.Cells[0, 10].PutValue("电话");
                sheet.Cells[0, 11].PutValue("地址");
                sheet.Cells[0, 12].PutValue("客服备注");
                int j = 0;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    j = i + 1;
                    sheet.Cells[j, 0].PutValue(dt.Rows[i]["CUST_PO_NUM"]);
                    sheet.Cells[j, 1].PutValue(dt.Rows[i]["ITEM_NUMBER"]);
                    sheet.Cells[j, 2].PutValue(dt.Rows[i]["ITEM_DESC"]);
                    sheet.Cells[j, 3].PutValue(dt.Rows[i]["NEED_BY_DATE"]);
                    sheet.Cells[j, 4].PutValue(dt.Rows[i]["CREATION_DATE"]);
                    sheet.Cells[j, 5].PutValue(dt.Rows[i]["QUANTITY"]);
                    sheet.Cells[j, 6].PutValue(dt.Rows[i]["UOM_CODE"]);
                    sheet.Cells[j, 7].PutValue(dt.Rows[i]["YJFHRQ"]);
                    sheet.Cells[j, 8].PutValue(dt.Rows[i]["Remark"]);
                    sheet.Cells[j, 9].PutValue(dt.Rows[i]["CONTACT_NAME"]);
                    sheet.Cells[j, 10].PutValue(dt.Rows[i]["PHONE_NUMBER"]);
                    sheet.Cells[j, 11].PutValue(dt.Rows[i]["ADDRESS"]);
                    sheet.Cells[j, 12].PutValue(dt.Rows[i]["DESCRIPTION"]);
                }

                workBook.Save(Server.MapPath("sfyorder" + DateTime.Now.ToString("yyMMddHHmmss") + ".xls"));
                DownLoadFile(Server.MapPath(""), "sfyorder" + DateTime.Now.ToString("yyMMddHHmmss") + ".xls");
            }
        }
        /// <summary>
        /// 下载服务器文件
        /// </summary>
        /// <param name="_FilePath">文件路径</param>
        /// <param name="_FileName">文件名</param>
        /// <returns>返回 bool型</returns>
        private bool DownLoadFile(string _FilePath, string _FileName)
        {
            try
            {
                System.IO.FileStream fs = System.IO.File.OpenRead(_FilePath + "\\" + _FileName);
                byte[] FileData = new byte[fs.Length];
                fs.Read(FileData, 0, (int)fs.Length);
                Response.Clear();
                Response.AddHeader("Content-Type", "application/ms-excel");
                string FileName = System.Web.HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(_FileName));
                Response.AddHeader("Content-Disposition", "inline;filename=" + System.Convert.ToChar(34) + FileName + System.Convert.ToChar(34));
                Response.AddHeader("Content-Length", fs.Length.ToString());
                Response.BinaryWrite(FileData);
                fs.Close();
                //删除服务器临时文件
                System.IO.File.Delete(_FilePath + "\\" + _FileName);
                Response.Flush();
                Response.End();

                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }

        }
    }
}
