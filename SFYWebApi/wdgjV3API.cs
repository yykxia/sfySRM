using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Collections; 

namespace SFYWebApi
{
    public class wdgjV3API
    {
        #region Methods

        #region 订单接口

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="cipher"></param>
        /// <param name="TimeStamp1">起始时间（时间戳），0表示不限制起始时间</param>
        /// <param name="TimeStamp2">截止时间（时间戳），0表示不限制截止时间</param>
        /// <param name="OrderStatus">0：表示所有，1：表示已经付款，2：表示未付款，3：表示已经取消，4：已经发货</param>
        /// <returns></returns>
        //[HttpPost]
        public static string mOrderSearch(string cipher, string TimeStamp1, string TimeStamp2, int OrderStatus)
        {
            DateTime apibeginTime = new DateTime();
            if (TimeStamp1 != "")
            {
                 apibeginTime = DateTime.Parse(TimeStamp1.ToString());
            }
            DateTime apiendTime = DateTime.Parse(TimeStamp2.ToString());
            
            string ids="";
            //查询订单 

            //使用本地时间：
            object otmp = SqlSel.GetSqlScale(" select top 1 AddedTime from WDApi_logs order by AddedTime desc");
            string addtime = "";
            if (otmp != null)
            {
                addtime = otmp.ToString();
            }
            string beginTime = addtime;
            DateTime endTime = DateTime.Now;

            string descrip = string.Format("【wdgj_抓单】起始时间{0},结束时间{1}，订单状态{2}", beginTime, endTime, OrderStatus);

            //LogHelper.InsertApiLog(db, "订单查询", cipher, descrip, "wdgj_api");
            string IPAddress = IPHelp.ClientIP;
            string sqlCmd = "insert into WDApi_logs ([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description],[cipher]) values ('','" + DateTime.Now + "','wdgj_api','" + IPAddress + "','订单查询','" + descrip + "','" + cipher + "')";
            //执行插入日志
            int execounts = SqlSel.ExeSql(sqlCmd);

            string sql = "select OrderId,OrderStatus,cipher from SFYOrderTab where cipher='" + cipher + "' AND ISnew=0 ";
            //if (beginTime.Length != 0)
            //{
            //    sql += OrderStatus == 1 ? " and CREATION_DATE>='" + beginTime+"' " : " and  CREATION_DATE>='" + beginTime+"' ";
            //}
            //if (endTime.ToString().Length != 0)
            //{
            //    sql += OrderStatus == 1 ? " and CREATION_DATE<'" + endTime + "' " : " and  CREATION_DATE<'" + endTime + "' ";
            //}
            //if (OrderStatus != 0)
            //{
            //    sql += "  and OrderStatus= " + OrderStatus;
            //}
            DataTable dtb1 = new DataTable();
            SqlSel.GetSqlSel(ref dtb1, sql);

            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version='1.0' encoding='gb2312'?>");
            xml.Append("<OrderList>");
            for (int item = 0; item < dtb1.Rows.Count; item++)
            {
                string oid = dtb1.Rows[item]["OrderId"].ToString();
                //List.Add(string.Format("{0}[{1}]", oid, 1));
                ids += oid + ", ";
                xml.Append("<OrderNO>" + oid + "</OrderNO>");

            }

            //LogHelper.debug(string.Format("【网店管家测试】--订单总数：{0};", data.Count));

            //InsertLogsFile("订单总数：" + dtb1.Rows.Count + "订单编号：" + ids +"/r/n"+ string.Format("【wdgj_抓单】本地调用起始时间{5},结束时间{6}；API起始时间【{3}】{0},结束时间【{4}】{1}，订单总数{2}", apibeginTime, apiendTime, dtb1.Rows.Count, TimeStamp1, TimeStamp2, beginTime, endTime)+"/r/n");
            
            xml.Append("<Page>1</Page>");
            xml.Append("<Result>" + "1" + "</Result>");
            xml.Append("<OrderCount>" + dtb1.Rows.Count + "</OrderCount>");
            xml.Append("</OrderList>");
            return xml.ToString();
        }

        /// <summary>
        /// 获得时间戳
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetTimeSpanStr()
        {
            DateTime startDate = DateTime.Parse("1970-01-01 00:00:00");
            DateTime nowDate = DateTime.Now;
            TimeSpan ts = nowDate - startDate;

            return int.Parse(ts.TotalSeconds.ToString("#0"));
        }

        // 订单下载
        //[HttpPost]
        public static string mGetOrder(string cipher, string OrderNO)
        {

            //var data = new DbObject();

            StringBuilder sb = new StringBuilder();

            string IPAddress = IPHelp.ClientIP;
            string sqlCmd = "insert into WDApi_logs ([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description],[cipher]) values ('','" + DateTime.Now + "','wdgj_api','" + IPAddress + "','订单查询','" + string.Format("【wdgj_订单下载】时间{0},订单编号{1} ", DateTime.Now, OrderNO) + "','" + cipher + "')";
            //执行插入日志
            int execounts = SqlSel.ExeSql(sqlCmd);

            string sql = @"select OrderId,CREATION_DATE,CUST_PO_NUM,CONTACT_NAME,PO_NUMBER,ITEM_NUMBER,ADDRESS,description,ITEM_DESC,NEED_BY_DATE,QUANTITY,UOM_CODE
                            ,PHONE_NUMBER from SFYOrderTab where OrderId='" + OrderNO+"' ";
            string ponum="";
            string[] addrArr = new string[3];
            string province = "";//省
            string city = "";//市
            string area = "";//区
            DataTable dtb1 = new DataTable();
            SqlSel.GetSqlSel(ref dtb1, sql);
            if (dtb1 == null)
                return "不存在该订单编号";

            StringBuilder xml = new StringBuilder();
            //data.Add("Item", Items);
            if (dtb1.Rows.Count > 0)
            {
                string curdte = Convert.ToDateTime(dtb1.Rows[0]["CREATION_DATE"]).ToString("yyyy-MM-dd HH:mm:ss");
                string contact_name = dtb1.Rows[0]["contact_name"].ToString().TrimEnd();
                string needdte = Convert.ToDateTime(dtb1.Rows[0]["NEED_BY_DATE"]).ToString("yyyy-MM-dd");
                //DataTable prodtl =new DataTable();
                //string sqlstr="select * from  tb_sys_capital";
                //SqlSel.GetSqlSel(ref prodtl,sqlstr);
                string Address = dtb1.Rows[0]["Address"].ToString().TrimEnd();
                if (Address.Length >= 8)
                {
                    addrArr = SplitAddress(Address);
                    province = addrArr[0];
                    city = addrArr[1];
                    area = addrArr[2];
                    //int find = -1;
                    //for (int f = 0; f < prodtl.Rows.Count; f++)
                    //{
                    //    string provincename = prodtl.Rows[f]["CapitalName"].ToString();
                    //    if (Address.Substring(0, 8).Contains(provincename))
                    //    {
                    //        find = f;
                    //        province = prodtl.Rows[f]["CapitalName"].ToString();
                    //        break;
                    //    }
                    //}
                    //if (find < 0)
                    //{
                    //    return "地址格式有误！";
                    //}
                    
                }
                else
                {
                    return "地址格式有误！";
                }
                if (contact_name == "" || contact_name == null)
                {
                    contact_name = "-";
                }
                string TelPhone = dtb1.Rows[0]["PHONE_NUMBER"].ToString().TrimEnd();
                //if (TelPhone == ""|| TelPhone == null)
                //{
                //    TelPhone = "-";
                //}
                ponum=dtb1.Rows[0]["cust_po_num"].ToString();
                xml.Append("<Order>");
                xml.Append("<Result>1</Result>");
                xml.Append("<Cause></Cause>");
                xml.Append("<OrderNO>" + OrderNO + "</OrderNO>");
                xml.Append("<DateTime>" + curdte + "</DateTime>");
                xml.Append("<BuyerID><![CDATA[" + ponum + "]]></BuyerID>");
                xml.Append("<BuyerName><![CDATA[" + contact_name + "]]></BuyerName>");
                xml.Append("<Country><![CDATA[" + "中国" + "]]></Country>");
                xml.Append("<Province><![CDATA[" + province + "]]></Province>");
                xml.Append("<City><![CDATA[" + city + "]]></City>");
                xml.Append("<Town><![CDATA[" + area + "]]></Town>");
                xml.Append("<Adr><![CDATA[" + Address + "]]></Adr>");
                xml.Append("<Zip><![CDATA[" + "]]></Zip>");
                xml.Append("<Email><![CDATA[" + "]]></Email>");
                xml.Append("<Phone><![CDATA[" + TelPhone + "]]></Phone>");
                xml.Append("<Total>0</Total>");
                xml.Append("<Postage>0</Postage>");
                xml.Append("<PayAccount><![CDATA[支付宝]]></PayAccount>");//支付方式
                xml.Append("<PayID><![CDATA[" + "" + "]]></PayID>");//支付编号
                xml.Append("<logisticsName><![CDATA[" + "]]></logisticsName>");
                xml.Append("<chargetype><![CDATA[担保交易]]></chargetype>");//结算方式
                xml.Append("<CustomerRemark><![CDATA[需求日期：" + needdte + "]]></CustomerRemark>");
                xml.Append("<Remark><![CDATA[" + dtb1.Rows[0]["description"].ToString() + "]]></Remark>");
                xml.Append("<InvoiceTitle><![CDATA[" + "" + "]]></InvoiceTitle>");

                xml.Append("<Item>");
                xml.Append("<GoodsID><![CDATA[" + dtb1.Rows[0]["ITEM_NUMBER"].ToString() + "]]></GoodsID>");
                xml.Append("<GoodsName><![CDATA[" + dtb1.Rows[0]["ITEM_DESC"].ToString() + "]]></GoodsName>");
                xml.Append("<GoodsSpec><![CDATA[" + dtb1.Rows[0]["uom_code"].ToString() + "]]></GoodsSpec>");
                xml.Append("<Count>" + dtb1.Rows[0]["quantity"].ToString() + "</Count>");
                xml.Append("<Price>0</Price>");
                xml.Append("</Item>");

                xml.Append("</Order>");


            }
            sql = "update SFYOrderTab set isnew=1 where  OrderId='" + OrderNO + "'";
            SqlSel.ExeSql(sql);
            //插入日志文件
            //InsertLogsFile(string.Format("【网店管家_订单下载】时间{0},订单编号{1},用户名{2} ", DateTime.Now, OrderNO, ponum));
            return xml.ToString();
            //LogHelper.InsertLogToFile("wdgj_api", string.Format("【网店管家_订单下载】时间{0},订单编号{1},用户名{2} ", DateTime.Now, OrderNO, data["BuyerID"]));
        }

        #endregion

        #region 发货通知
        public static string mSndGoods(string OrderNO, string SndStyle, string BillID)
        {
            string orderlist = "";
            DataTable dtn = new DataTable();
            string sqldt = "select * from WLCode ";
            SqlSel.GetSqlSel(ref dtn, sqldt);
            for (int dd = 0; dd < dtn.Rows.Count; dd++)
            {
                string wlcode = dtn.Rows[dd]["EWL"].ToString();
                if (SndStyle.Contains(wlcode))
                {
                    orderlist = dtn.Rows[dd]["ZWL"].ToString();
                    break;
                }
            }
            //InsertLogsFile(string.Format("【wdgj_发货同步】订单号:{0},发货类型:{1},快递单号:{2} ", OrderNO, orderlist, BillID));
            string cipher = "MBH3Q2W876EG422";
            int i = OrderNO.Split(',').Length - 1;
            int m = 0;
            
            Encoding coding = Encoding.UTF8;
            //LogHelper.InsertLogToFile("wdgj_api", string.Format("【wdgj_发货同步】订单号:{0},发货时间{1}，同步时间{2}，发货类型:{3},用户:{4},快递单号:{5} ", OrderNO, SndDate, DateTime.Now, SndStyle, CustomerID, BillID));
            string IPAddress = IPHelp.ClientIP;

            //引用索菲亚接口
            //net.sogal.www.WebService aa = new SFYWebApi.net.sogal.www.WebService();
            foreach (var id in OrderNO.Split(','))
            {
                
                string sqlcmd = "select cipher,CREATION_DATE,OrderStatus,QUANTITY from SFYOrderTab where OrderId='" + id + "' ";
                DataTable dtb1 = new DataTable();
                SqlSel.GetSqlSel(ref dtb1, sqlcmd);
                if (dtb1 == null)
                {
                    return xml(0, OrderNO);
                }
                string dtbln = id.Split('-')[0];
                string dline = id.Substring(dtbln.Length+1);
                string qunty = dtb1.Rows[0]["QUANTITY"].ToString();
                string sqlCmd2 = "insert into WDApiFH ([Header_ID],[Line_ID],[OrderId],[TQuantity],[LOG_INFO],[FHSat],[Create_Date],[cipher]) values ('" + dtbln + "','" + dline + "','" + id + "','" + qunty + "','" + string.Format("订单号:{0}，发货类型:{1},快递单号:{2} ", id, orderlist, BillID) + "','0','" + DateTime.Now + "','" + cipher + "')";
                //执行发货同步
                int execounts2 = SqlSel.ExeSql(sqlCmd2);
                if (execounts2 == 1)
                {
                    m += 1;
                }
                //aa.AddPoRequisition(Int32.Parse(dtbln), Int32.Parse(dline), Decimal.Parse(qunty), string.Format("订单号:{0}，发货类型:{1},快递单号:{2} ", id, orderlist, BillID));//写入索菲亚接口
                //string sqlstr = "update WDApiFH set [FHSat]=1 where [OrderId]='" + id + "' ";
                //int execounts = SqlSel.ExeSql(sqlstr);
                //if (execounts == 0)
                //{
                //    break;
                //}
                //DataTable newdt = new DataTable();
                //string sqlCmd = "select * from WDApiFH where FHSat=0 and OrderId='" + id + "'";
                //SqlSel.GetSqlSel(ref newdt, sqlCmd);
                //string orderid = newdt.Rows[0]["OrderId"].ToString();
                //aa.AddPoRequisition(Int32.Parse(newdt.Rows[0]["Header_ID"].ToString()), Int32.Parse(newdt.Rows[0]["Line_ID"].ToString()), Decimal.Parse(newdt.Rows[0]["TQuantity"].ToString()), newdt.Rows[0]["LOG_INFO"].ToString());//传值测试
                //string sqlstr = "update WDApiFH set [FHSat]=1 where [OrderId]='" + orderid + "' ";
                //int execounts = SqlSel.ExeSql(sqlstr);
                //if (execounts == 0)
                //{
                //    break;
                //}
            }
               //写入索菲亚接口
            //net.sogal.www.WebService aa = new SFYWebApi.net.sogal.www.WebService();
            //DataTable newdt = new DataTable();
            //string sqlCmd = "select * from WDApiFH where FHSat=0 ";
            //SqlSel.GetSqlSel(ref newdt, sqlCmd);
            //for (int ii = 0; ii < newdt.Rows.Count; ii++)
            //{
            //    string RQTId = newdt.Rows[ii]["RQTId"].ToString();
            //    aa.AddPoRequisition(Int32.Parse(newdt.Rows[ii]["Header_ID"].ToString()), Int32.Parse(newdt.Rows[ii]["Line_ID"].ToString()), Decimal.Parse(newdt.Rows[ii]["TQuantity"].ToString()), newdt.Rows[ii]["LOG_INFO"].ToString());//传值测试
            //    string sqlstr = "update WDApiFH set [FHSat]=1 where [RQTId]='" + RQTId + "' ";
            //    int execounts = SqlSel.ExeSql(sqlstr);
            //    if (execounts == 0)
            //    {
            //        break;
            //    }
            //}
            string sqlCmdt = "insert into WDApi_logs ([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description],[cipher]) values ('','" + DateTime.Now + "','wdgj_api','" + IPAddress + "','发货同步','" + string.Format("【wdgj_发货同步】订单号:{0}，同步时间{1}，发货类型:{2},快递单号:{3} ", OrderNO.Replace(',', '，'), DateTime.Now, orderlist, BillID) + "','" + cipher + "')";
            //执行插入日志
            int execountst = SqlSel.ExeSql(sqlCmdt);
            if (m == i + 1)
            {

                return xml(1, "1");
            }
            else
            {
                return xml(0, OrderNO);
            }

        }

        public static string xml(int i, string reason)
        {
            if (i == 1)
            {
                StringBuilder xml = new StringBuilder();
                xml.Append("<?xml version='1.0' encoding='gb2312'?>");
                xml.Append("<rsp>");
                xml.Append("<result>1</result>");
                xml.Append("</rsp>");
                return xml.ToString();
            }
            else if (i == 0)
            {
                StringBuilder xml = new StringBuilder();

                xml.Append("<?xml version='1.0' encoding='gb2312'?>");
                xml.Append("<rsp>");
                xml.Append("<result>0</result>");
                xml.Append("<cause>" + reason + "</cause>");
                xml.Append("<goodsType></goodsType>");
                xml.Append("</rsp>");
                return xml.ToString();
            }
            else
            {
                return null;
            }
        }


        #endregion

        /// <summary>
        /// 插入日志文件txt
        /// </summary>
        /// <param name="content">插入内容</param>
        //private static void InsertLogsFile(string content)
        //{
        //    try
        //    {
        //        string filepth=System.Web.HttpContext.Current.Server.MapPath("APILOG/") + DateTime.Now.ToString("yyyyMMdd") + "wdgj_API.txt";
        //        if (!File.Exists(filepth))
        //        {
        //            FileStream fs1 = new FileStream(filepth, FileMode.Create, FileAccess.Write);//创建写入文件 
        //            StreamWriter sw = new StreamWriter(fs1);
        //            sw.BaseStream.Seek(0, SeekOrigin.End);
        //            sw.WriteLine("=====================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "=======================");
        //            sw.Write(content + "\r\n");
        //            sw.WriteLine("==========================================================================" + "\r\n");
        //            sw.Flush();
        //            sw.Close();
        //            fs1.Close();
        //        }
        //        else
        //        {
        //            FileStream fs = new FileStream(filepth, FileMode.Open, FileAccess.Write);
        //            StreamWriter sr = new StreamWriter(fs);
        //            sr.WriteLine("=====================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "=======================");
        //            sr.Write(content + "\r\n");
        //            sr.WriteLine("==========================================================================" + "\r\n");
        //            sr.Flush();
        //            sr.Close();
        //            fs.Close();
        //        }
               
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        /// <summary>
        /// 根据地址字符串获取省、市、区三段式信息
        /// </summary>
        /// <param name="AddStr">地址长字符串信息</param>
        /// <returns></returns>
        public static string[] SplitAddress(string AddStr)
        {
            //获取网店管家省市区实时信息
            DataSet addMap = new DataSet();
            if (System.Web.HttpContext.Current.Session["addMap"] != null)
            {
                addMap = (DataSet)System.Web.HttpContext.Current.Session["addMap"];
            }
            else 
            {
                wdgjWebService.wdgjWebService wdgjService = new SFYWebApi.wdgjWebService.wdgjWebService();
                addMap = wdgjService.wdgj_AddressMap();
                System.Web.HttpContext.Current.Session.Add("addMap", addMap);
            }

            string[] AddSplitInfo = new string[3];
            DataTable provDt = new DataTable();
            DataTable cityDt = new DataTable();
            DataTable areaDt = new DataTable();

            provDt = addMap.Tables["Province"];

            cityDt = addMap.Tables["City"];

            areaDt = addMap.Tables["Area"];

            //提取省、直辖市信息
            string ProvinceID = "", ProvinceName = "";
            for (int i = 0; i < provDt.Rows.Count; i++)
            {
                if (AddStr.StartsWith(provDt.Rows[i]["ProvinceName"].ToString()))
                {
                    ProvinceID = provDt.Rows[i]["ProvinceID"].ToString();
                    ProvinceName = provDt.Rows[i]["ProvinceName"].ToString();
                    break;
                }
            }
            //提取市、县信息
            string CityID = "", CityName = "";
            if (!string.IsNullOrEmpty(ProvinceID))
            {
                DataRow[] cityList = cityDt.Select("ProvinceID='" + ProvinceID + "'");
                for (int i = 0; i < cityList.Length; i++)
                {
                    if (AddStr.Contains(cityList[i]["CityName"].ToString()))
                    {
                        CityID = cityList[i]["CityID"].ToString();
                        CityName = cityList[i]["CityName"].ToString();
                        break;
                    }
                }
            }
            //提取地区信息
            string AreaName = "";
            if (!string.IsNullOrEmpty(CityID))
            {
                DataRow[] AreaList = areaDt.Select("CityID='" + CityID + "'");
                for (int i = 0; i < AreaList.Length; i++)
                {
                    if (AddStr.Contains(AreaList[i]["AreaName"].ToString()))
                    {
                        AreaName = AreaList[i]["AreaName"].ToString();
                        break;
                    }
                }
            }

            AddSplitInfo[0] = ProvinceName;
            AddSplitInfo[1] = CityName;
            AddSplitInfo[2] = AreaName;

            return AddSplitInfo;
        }
    }
}
#endregion