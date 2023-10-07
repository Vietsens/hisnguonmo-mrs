using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MOS.DAO.Sql
{
    public class Translate
    {
        public static string TranslateMessage(string message)
        {
            var url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=vi&dt=t&q={0}", message);
            var webClient = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
            var result = webClient.DownloadString(url);
            try
            {
                result = result.Substring(4, result.IndexOf(",", 4, StringComparison.Ordinal) - 4);
            }
            catch
            {
                result = message;
            }
            return result;
        }
    }
}
