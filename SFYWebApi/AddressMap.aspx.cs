using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SFYWebApi
{
    public partial class AddressMap : System.Web.UI.Page
    {

        DataSet addMap = new DataSet();
        wdgjWebService.wdgjWebService wdgjService = new SFYWebApi.wdgjWebService.wdgjWebService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                TextBox1.Text = DateTime.Now.ToShortDateString();
                TextBox2.Text = DateTime.Now.ToShortDateString();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //获取所有网店管家省市区信息
            //addMap = wdgjService.wdgj_AddressMap();
            //获取网店管家省市区实时信息
            //DataSet addMap = new DataSet();
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

            DateTime date1 = Convert.ToDateTime(TextBox1.Text);
            DateTime date2 = Convert.ToDateTime(TextBox2.Text);

            string sqlCmd = "select * from sfyorderTab where createdate>='" + date1 + "' and createdate<='" + date2 + "'";
            DataTable dt = new DataTable();
            SqlSel.GetSqlSel(ref dt, sqlCmd);
            string AddrStr = "", province = "", city = "", area = "";
            string[] addrArr = new string[3];
            dt.Columns.Add("sendProvince");
            dt.Columns.Add("sendCity");
            dt.Columns.Add("sendArea");
            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                AddrStr = dt.Rows[i]["Address"].ToString();
                addrArr = SplitAddressStr(AddrStr);
                dt.Rows[i]["sendProvince"] = addrArr[0];
                dt.Rows[i]["sendCity"] = addrArr[1];
                dt.Rows[i]["sendArea"] = addrArr[2];
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        /// <summary>
        /// 根据地址字符串获取省、市、区三段式信息
        /// </summary>
        /// <param name="AddStr">地址长字符串信息</param>
        /// <returns></returns>
        public string[] SplitAddressStr(string AddStr) 
        {
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
