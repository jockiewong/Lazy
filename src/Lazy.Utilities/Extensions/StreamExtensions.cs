using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.Utilities.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// 获取数据流的MD5值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetMD5Hash(this Stream stream)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(stream);
                stream.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5Hash() fail, error:" + ex.Message);
            }
        }
    }
}
