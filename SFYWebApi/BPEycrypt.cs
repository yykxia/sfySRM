using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SFYWebApi
{
    public class BPEycrypt
    {
        /// <summary>
        /// 用户密码加密
        /// </summary>
        /// <param name="paramStr">待加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptAdmin(string paramStr)
        {
            return MD5Encrypt(MD5Encrypt(paramStr));
        }
        #region ========MD5加密========
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="paramstr"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string paramstr)
        {
            string privateKey = "loveyajuan";
            string tempStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(privateKey, "MD5");

            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(paramstr + tempStr, "MD5").ToLower();
        }
        #endregion
      
    }
}
