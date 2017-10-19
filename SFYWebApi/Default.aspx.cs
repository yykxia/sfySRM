using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SFYWebApi
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CheckAdminLogin())
                Response.Write(" <script  language='javascript'> window.top.location='loginapi.aspx'</script> ");
            if (!IsPostBack)
            {
                getcwtx();//验证当天获取数据是否和索菲亚同步
            }
        }
        //信息错误提醒，提醒前一天内错误信息
        protected void getcwtx()
        {
            net.sogal.www.WebService aa = new SFYWebApi.net.sogal.www.WebService();
            string strjson = aa.GetPoRequisition(DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")), DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")));//传入参数

            DataTable dtbl = new DataTable();
            dtbl = Json2Dtb(strjson);
            DataTable newdt = new DataTable();
            string sqlCmd = "select * from SFYOrderTab where CREATION_DATE between '" + DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")) + "' and '" + DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) + "'";
            SqlSel.GetSqlSel(ref newdt, sqlCmd);
            if (dtbl.Rows.Count > newdt.Rows.Count)//当索菲亚服务端大于本地
            {
                Label3.Text += "索菲亚数据和本地数据不对应！在" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + ",请重新获取！";
            }
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
            Label1.Text = "订单正在获取中，请稍后。";
            Label2.Text = "";
            if (txtNeedDate.Value.Length == 0)
            {
                Label1.Text = "请选择需求日期！";
                GridView1.DataSource = null;
                GridView1.DataBind();
                return;
            }
            net.sogal.www.WebService aa = new SFYWebApi.net.sogal.www.WebService();
            DateTime dt = DateTime.Parse(txtNeedDate.Value);
            DateTime dt2 = DateTime.Parse(Text1.Value);
            string strjson = aa.GetPoRequisition(dt, dt2);
            //Label4.Text = strjson;
            DataTable dtbl = new DataTable();
            dtbl = Json2Dtb(strjson);
            //编码对照表
            DataTable goodsTable = new DataTable();
            string sqlstr1= "select * from t_MapGoodsList";
            SqlSel.GetSqlSel(ref goodsTable, sqlstr1);

            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                string itemNum = dtbl.Rows[i]["ITEM_NUMBER"].ToString();
                //int quantity = Convert.ToInt32(dtbl.Rows[i]["QUANTITY"]);
                //if (dtbl.Rows[i]["ITEM_DESC"] != "" && dtbl.Rows[i]["ITEM_DESC"] != null)
                //{
                //    DataRow newRow;
                //    newRow = dtbl.NewRow();
                //    string orderid = dtbl.Rows[i]["HEADER_ID"].ToString() + "-" + dtbl.Rows[i]["LINE_ID"].ToString();


                //    if (dtbl.Rows[i]["ITEM_DESC"].ToString().Contains("["))
                //    {
                //        //判断物料描述中【】内计量单位与实际计量单位是否相符
                //        string descunitA = dtbl.Rows[i]["ITEM_DESC"].ToString().Split('/')[1];
                //        string descunit = descunitA.Substring(0, 1);
                //        string realunit = dtbl.Rows[i]["UOM_CODE"].ToString();
                //        if (descunit != realunit)
                //        {
                //            MessageBox.Show("订单号为" + orderid + "的备注计量单位与实际返回计量单位不符，请相关人员确认！", "请注意！");
                //        }
                //        string strcount = dtbl.Rows[i]["ITEM_DESC"].ToString().Split('[')[1];
                //        string innerstr = strcount.Substring(0, strcount.IndexOf("]"));
                //        int itemcount = Convert.ToInt32(innerstr.Substring(0, GetIndexOfChinese(innerstr) + 1));
                //        dtbl.Rows[i]["QUANTITY"] = itemcount * Convert.ToInt32(dtbl.Rows[i]["QUANTITY"]);
                //    }
                //    if (dtbl.Rows[i]["ITEM_DESC"].ToString().Contains("【"))
                //    {
                //        //判断物料描述中【】内计量单位与实际计量单位是否相符
                //        string descunitA = dtbl.Rows[i]["ITEM_DESC"].ToString().Split('/')[1];
                //        string descunit = descunitA.Substring(0, 1);
                //        string realunit = dtbl.Rows[i]["UOM_CODE"].ToString();
                //        if (descunit != realunit)
                //        {
                //            MessageBox.Show("订单号为" + orderid + "的备注计量单位与实际返回计量单位不符，请相关人员确认！", "请注意！");
                //        }
                //        string strcount = dtbl.Rows[i]["ITEM_DESC"].ToString().Split('【')[1];
                //        string innerstr = strcount.Substring(0, strcount.IndexOf("】"));
                //        int itemcount = Convert.ToInt32(innerstr.Substring(0, GetIndexOfChinese(innerstr) + 1));
                //        dtbl.Rows[i]["QUANTITY"] = itemcount * Convert.ToInt32(dtbl.Rows[i]["QUANTITY"]);
                //    }
                //    if (dtbl.Rows[i]["ITEM_DESC"].ToString().Contains("("))
                //    {
                //        string dpm = dtbl.Rows[i]["ITEM_DESC"].ToString().Split('(')[0];
                //        //获取()之间的字符 
                //        int IndexofA = dtbl.Rows[i]["ITEM_DESC"].ToString().IndexOf("("); //或者（
                //        int IndexofB = dtbl.Rows[i]["ITEM_DESC"].ToString().IndexOf(")"); //或者）
                //        string pinname = dtbl.Rows[i]["ITEM_DESC"].ToString().Substring(IndexofA + 1, IndexofB - IndexofA - 1);
                //        dtbl.Rows[i]["ITEM_DESC"] = dpm;
                //        dtbl.Rows[i]["ITEM_NUMBER"] = pinname;

                //    }
                //    if (dtbl.Rows[i]["ITEM_DESC"].ToString().Contains("（"))
                //    {
                //        string dtbln = dtbl.Rows[i]["ITEM_DESC"].ToString().Split('（')[0];
                //        //获取()之间的字符 
                //        int IndexofC = dtbl.Rows[i]["ITEM_DESC"].ToString().IndexOf("（"); //或者（
                //        int IndexofD = dtbl.Rows[i]["ITEM_DESC"].ToString().IndexOf("）"); //或者）
                //        string pname = dtbl.Rows[i]["ITEM_DESC"].ToString().Substring(IndexofC + 1, IndexofD - IndexofC - 1);
                //        dtbl.Rows[i]["ITEM_DESC"] = dtbln;
                //        dtbl.Rows[i]["ITEM_NUMBER"] = pname;
                //    }

                //    string pname = dtbl.Rows[i]["ITEM_DESC"].ToString().Substring(dtbln.Length).Replace("（", "").Replace("）", "");


                //    string otherstr = dtbl.Rows[i]["ITEM_DESC"].ToString().Split('(')[1];
                //    string pinname = otherstr.Split(')')[0];
                //}
                
                //匹配商品编码
                if (!string.IsNullOrEmpty(itemNum))
                {
                    int find = -1;
                    for (int k = 0; k < goodsTable.Rows.Count; k++)
                    {
                        string sfyNo = goodsTable.Rows[k]["targetNo"].ToString();
                        if (sfyNo == itemNum)
                        {
                            find = k;
                            break;
                        }
                    }
                    if (find >= 0)
                    {
                        dtbl.Rows[i]["ITEM_NUMBER"] = goodsTable.Rows[find]["goodsNo"];
                        //非最小计量数量则乘以相关数量
                        if (Convert.ToBoolean(goodsTable.Rows[find]["isMinUnitOrNo"]) == true)
                        {
                            dtbl.Rows[i]["QUANTITY"] = (Convert.ToInt32(dtbl.Rows[i]["QUANTITY"]) * (Convert.ToInt32(goodsTable.Rows[find]["UnitTimes"]))).ToString();
                        }
                        //定制款商品增加“索菲亚定制”备注，以便相关订单匹配定制仓库--2016年12月16日 19:17:36
                        if (Convert.ToBoolean(goodsTable.Rows[find]["isOrder"]) == true)
                        {
                            dtbl.Rows[i]["SPECIFICATION"] = dtbl.Rows[i]["SPECIFICATION"].ToString()+" 备注:索菲亚定制";
                        }


                    }
                }

            }

            GridView1.DataSource = dtbl;
            StringBuilder sbresault = new StringBuilder();
            sbresault.Append("");

            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataTable newdt = new DataTable();
                string orderid = dtbl.Rows[i]["HEADER_ID"].ToString() + "-" + dtbl.Rows[i]["LINE_ID"].ToString();
                string sqlCmd = "select * from SFYOrderTab where OrderId='" + orderid + "' ";
                SqlSel.GetSqlSel(ref newdt, sqlCmd);
                if (newdt.Rows.Count > 0)
                {
                    continue;
                    //sqlCmd = "select QUANTITY from SFYOrderTab where OrderId='" + orderid + "' and ITEM_NUMBER='" + dtbl.Rows[i]["ITEM_NUMBER"].ToString() + "'";
                    //int curcount = Convert.ToInt32(dtbl.Rows[i]["QUANTITY"]);
                    //int befcount = Convert.ToInt32(SqlSel.GetSqlScale(sqlCmd));
                    //if (curcount!=befcount)
                    //{
                    //    sbresault.Append(string.Format("订单号为{0}的订单,货品数量发生改变：原始数量为{2},现数量为（{1}），请通知相关人员！<br/>", orderid, curcount,befcount));
                    //    Label2.Text = sbresault.ToString();
                    //    continue;
                    //}
                    //else
                    //{
                    //    continue;
                    //}

                }
                else
                {
                    //联系人是否为空
                    if (dtbl.Rows[i]["CONTACT_NAME"].ToString() == "" || dtbl.Rows[i]["CONTACT_NAME"].ToString() == null)
                    {
                        sbresault.Append(string.Format("PO号为{0}的联系人为空，请查验！<br/>", dtbl.Rows[i]["CUST_PO_NUM"].ToString()));
                    }
                    //联系电话是否为空
                    if (dtbl.Rows[i]["PHONE_NUMBER"].ToString() == "" || dtbl.Rows[i]["PHONE_NUMBER"].ToString() == null)
                    {
                        sbresault.Append(string.Format("PO号为{0}的联系电话为空，请查验！<br/>", dtbl.Rows[i]["CUST_PO_NUM"].ToString()));
                    }
                    //判断订单地址是否存在疑问
                    if (dtbl.Rows[i]["Address"].ToString() == "" || dtbl.Rows[i]["Address"].ToString() == null)
                    {
                        sbresault.Append(string.Format("PO号为{0}的地址为空，请查验！<br/>", dtbl.Rows[i]["CUST_PO_NUM"].ToString()));
                    }
                    else
                    {
                        if (dtbl.Rows[i]["Address"].ToString().Length >= 8)
                        {
                            string origAdr = dtbl.Rows[i]["Address"].ToString();
                            DataTable prodtl = new DataTable();
                            string sqlstr = "select * from  tb_sys_capital";
                            SqlSel.GetSqlSel(ref prodtl, sqlstr);
                            int find = -1;
                            for (int j = 0; j < prodtl.Rows.Count; j++)
                            {
                                string provincename = prodtl.Rows[j]["CapitalName"].ToString();
                                if (origAdr.Substring(0, 8).Contains(provincename))
                                {
                                    find = j;
                                    break;
                                }
                            }
                            if (find < 0)
                            {
                                sbresault.Append(string.Format("PO号为{0}的地址可能不具体“{1}”，请查验！<br/>", dtbl.Rows[i]["CUST_PO_NUM"].ToString(), dtbl.Rows[i]["Address"].ToString()));
                            }

                        }
                        else
                        {
                            sbresault.Append(string.Format("PO号为{0}的地址可能不具体“{1}”，请查验！<br/>", dtbl.Rows[i]["CUST_PO_NUM"].ToString(), dtbl.Rows[i]["Address"].ToString()));
                        }
                    }

                    string guige = "";
                    int quantity = Convert.ToInt32(dtbl.Rows[i]["QUANTITY"]);
                    //定制款的实际数量transaction_quantity
                    int realQty;
                    try
                    {
                        realQty = Convert.ToInt32(dtbl.Rows[i]["transaction_quantity"]);
                    }
                    catch (Exception)
                    {
                        realQty = 0;
                    }
                    //如果定制数量大于0，则取定制数量
                    if (realQty > 0) 
                    {
                        quantity = realQty;
                    }
                    string crationname = dtbl.Rows[i]["CONTACT_NAME"].ToString();
                    //判断是否是定制款
                    if (isCustOrder(dtbl.Rows[i]["ITEM_NUMBER"].ToString()))
                    {
                        guige = "  规格：" + dtbl.Rows[i]["SPECIFICATION"].ToString();
                        //插入原始数据记录表origDataLog
                        //sqlCmd = "insert into origDataLog (origId,origText,sourceTab) values ('" + orderid + "','transaction_quantity:" + dtbl.Rows[i]["QUANTITY"].ToString() + "','SFYOrderTab')";
                        //int curExeCount = SqlSel.ExeSql(sqlCmd);
                        //if (curExeCount == 0)
                        //{
                        //    return;
                        //}
                        //else
                        //{
                        //    //订单表SFYOrderTab中存储定制款的实际数量
                        //    quantity = realQty;
                        //    crationname = crationname + guige;
                        //}
                        crationname = crationname + guige;
                    }
                    //索菲亚关于部分需配备礼盒的商品生成相应订单(支持一个产品对应多个赠品的情况)
                    string origGoods = dtbl.Rows[i]["ITEM_NUMBER"].ToString();
                    DataTable saledtl = new DataTable();
                    sqlCmd = "select (select goodsNo from goodslist where SaleAct.origgoodsid=goodslist.id) as origNo,(select goodsName from goodslist where SaleAct.origgoodsid=goodslist.id) as origName,";
                    sqlCmd += " (select goodsNo from goodslist where SaleAct.giftgoodsid=goodslist.id) as giftNo,(select goodsName from goodslist where SaleAct.giftgoodsid=goodslist.id) as giftName,* from SaleAct";
                    sqlCmd += " where enabled=1 and startdte<'" + DateTime.Now + "' and enddte>'" + DateTime.Now + "'";
                    SqlSel.GetSqlSel(ref saledtl, sqlCmd);
                    for (int bb = 0; bb < saledtl.Rows.Count; bb++)
                    {
                        string curGoodsNo = saledtl.Rows[bb]["origNo"].ToString();
                        if (origGoods == curGoodsNo)
                        {
                            sqlCmd = "select * from SFYOrderTab where parentid is not null and ITEM_NUMBER='" + saledtl.Rows[bb]["giftNo"].ToString() + "' and";
                            sqlCmd += " parentid='" + dtbl.Rows[i]["HEADER_ID"].ToString() + "-" + dtbl.Rows[i]["LINE_ID"].ToString() + "'";
                            DataTable curdtl = new DataTable();
                            SqlSel.GetSqlSel(ref curdtl, sqlCmd);
                            if (curdtl.Rows.Count > 0)
                            {
                                continue;
                            }
                            else
                            {
                                sqlCmd = "insert into SFYOrderTab (HEADER_ID,LINE_ID,PO_NUMBER,CUST_PO_NUM,ITEM_NUMBER,ITEM_DESC,NEED_BY_DATE,CREATION_DATE,QUANTITY,";
                                sqlCmd += " UOM_CODE,DESCRIPTION,CONTACT_NAME,PHONE_NUMBER,ADDRESS,ISNew,OrderId,OrderStatus,cipher,CreateDate,parentid) values (";
                                sqlCmd += " '" + dtbl.Rows[i]["HEADER_ID"].ToString() + "','" + dtbl.Rows[i]["LINE_ID"].ToString() + "','" + dtbl.Rows[i]["PO_NUMBER"].ToString() + "',";
                                sqlCmd += " '" + dtbl.Rows[i]["CUST_PO_NUM"].ToString() + "','" + saledtl.Rows[bb]["giftNo"].ToString() + "','" + saledtl.Rows[bb]["giftName"].ToString() + "',";
                                sqlCmd += " '" + dtbl.Rows[i]["NEED_BY_DATE"].ToString() + "','" + dtbl.Rows[i]["CREATION_DATE"].ToString() + "'," + quantity + ",'" + dtbl.Rows[i]["UOM_CODE"].ToString() + "',";
                                sqlCmd += " '" + dtbl.Rows[i]["CUST_PO_NUM"].ToString() + guige + "','" + crationname + "','" + dtbl.Rows[i]["PHONE_NUMBER"].ToString() + "',";
                                sqlCmd += " '" + dtbl.Rows[i]["ADDRESS"].ToString() + "',0,'SFY" + DateTime.Now.Ticks + "',";
                                sqlCmd += " 1,'MBH3Q2W876EG422','" + DateTime.Now + "','" + dtbl.Rows[i]["HEADER_ID"].ToString() + "-" + dtbl.Rows[i]["LINE_ID"].ToString() + "')";
                                int insertCounts = SqlSel.ExeSql(sqlCmd);
                                if (insertCounts == 0)
                                {
                                    break;
                                }
                            }
                            //break;
                        }
                    }
                    sqlCmd = "insert into SFYOrderTab (HEADER_ID,LINE_ID,PO_NUMBER,CUST_PO_NUM,ITEM_NUMBER,ITEM_DESC,NEED_BY_DATE,CREATION_DATE,QUANTITY,";
                    sqlCmd += " UOM_CODE,DESCRIPTION,CONTACT_NAME,PHONE_NUMBER,ADDRESS,ISNew,OrderId,OrderStatus,cipher,CreateDate) values (";
                    sqlCmd += " '" + dtbl.Rows[i]["HEADER_ID"].ToString() + "','" + dtbl.Rows[i]["LINE_ID"].ToString() + "','" + dtbl.Rows[i]["PO_NUMBER"].ToString() + "',";
                    sqlCmd += " '" + dtbl.Rows[i]["CUST_PO_NUM"].ToString() + "','" + dtbl.Rows[i]["ITEM_NUMBER"].ToString() + "','" + dtbl.Rows[i]["ITEM_DESC"].ToString() + "',";
                    sqlCmd += " '" + dtbl.Rows[i]["NEED_BY_DATE"].ToString() + "','" + dtbl.Rows[i]["CREATION_DATE"].ToString() + "'," + quantity + ",'" + dtbl.Rows[i]["UOM_CODE"].ToString() + "',";
                    sqlCmd += " '" + dtbl.Rows[i]["CUST_PO_NUM"].ToString() + guige + "','" + crationname + "','" + dtbl.Rows[i]["PHONE_NUMBER"].ToString() + "',";
                    sqlCmd += " '" + dtbl.Rows[i]["ADDRESS"].ToString().Replace("'","") + "',0,'" + dtbl.Rows[i]["HEADER_ID"].ToString() + "-" + dtbl.Rows[i]["LINE_ID"].ToString() + "',";
                    sqlCmd += " 1,'MBH3Q2W876EG422','" + DateTime.Now + "')";
                    int execounts = SqlSel.ExeSql(sqlCmd);
                    if (execounts == 0)
                    {
                        sbresault.Append(string.Format("客户{0}的信息存在异常，未能抓单成功，请校验!", dtbl.Rows[i]["CUST_PO_NUM"].ToString()));
                        continue;
                    }
                }
            }
            if (!string.IsNullOrEmpty(sbresault.ToString()))
            {
                Label2.Text = sbresault.ToString();
            }
            GridView1.DataBind();
            Label1.Text = "订单获取完成。";

        }

        ////写入数据至索菲亚接口
        protected void Button2_Click(object sender, EventArgs e)
        {
            net.sogal.www.WebService aa = new SFYWebApi.net.sogal.www.WebService();
            DataTable newdt = new DataTable();
            string sqlCmd = "select * from WDApiFH where FHSat=0 ";
            SqlSel.GetSqlSel(ref newdt, sqlCmd);
            if (newdt.Rows.Count == 0)
            {
                Label2.Text = "所有订单发货信息已同步，请稍后再试。";
                return;
            }
            for (int ii = 0; ii < newdt.Rows.Count; ii++)
            {
                string RQTId = newdt.Rows[ii]["RQTId"].ToString();
                aa.AddPoRequisition(Int32.Parse(newdt.Rows[ii]["Header_ID"].ToString()), Int32.Parse(newdt.Rows[ii]["Line_ID"].ToString()), Decimal.Parse(newdt.Rows[ii]["TQuantity"].ToString()), newdt.Rows[ii]["LOG_INFO"].ToString());//传值测试
                string sqlstr = "update WDApiFH set [FHSat]=1 where [RQTId]='" + RQTId + "' ";
                int execounts = SqlSel.ExeSql(sqlstr);
                if (execounts == 0)
                {
                    break;
                }
            }
            Label2.Text = "发货信息同步已完成！";
        }

        /// <summary>   
        /// Json转DataTable   
        /// </summary>   
        /// <param name="json"></param>   
        /// <returns></returns>   
        private DataTable Json2Dtb(string json)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ArrayList dic = jss.Deserialize<ArrayList>(json);
            DataTable dtb = new DataTable();
            if (dic.Count > 0)
            {
                foreach (Dictionary<string, object> drow in dic)
                {
                    if (dtb.Columns.Count == 0)
                    {
                        foreach (string key in drow.Keys)
                        {
                            dtb.Columns.Add(key, drow[key].GetType());
                        }
                    }
                    DataRow row = dtb.NewRow();
                    foreach (string key in drow.Keys)
                    {
                        row[key] = drow[key];
                    }
                    dtb.Rows.Add(row);
                }
            }

            return dtb;
        }
        private int GetIndexOfChinese(string CString)
        {
            int j;
            for (j = 0; j < CString.Length; j++) 
            {
                try
                {
                    Convert.ToInt32(CString.Substring(j, j + 1));
                }
                catch (Exception) 
                {
                    break;
                }                
            }
            return j;
        }

        //判断是否是定制订单
        private bool isCustOrder(string GoodsNo) 
        {
            DataTable goodsTable = new DataTable();
            string sqlstr1 = "select * from t_MapGoodsList where goodsNo='" + GoodsNo + "'";
            SqlSel.GetSqlSel(ref goodsTable, sqlstr1);
            if (goodsTable.Rows.Count > 0)
            {
                if (Convert.ToBoolean(goodsTable.Rows[0]["isOrder"]) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }
    }
}
