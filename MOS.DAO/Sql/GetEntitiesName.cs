using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MOS.DAO.Sql
{
    public class GetEntitiesName
    {
        private const string entities = "Entities";

        public static string Get(string sql)
        {
            string result="";
            try
            {
                if (!String.IsNullOrWhiteSpace(sql))
                {
                    string fromTalbe = "";
                    String regex = @"\s+";//https://o7planning.org/vi/10795/huong-dan-su-dung-bieu-thuc-chinh-quy-trong-csharp
                    String[] _lstWord = Regex.Split(sql.ToLower(), regex);
                    _lstWord = _lstWord.Where(o => !String.IsNullOrWhiteSpace(o)).ToArray();
                    for (int i = 0; i < _lstWord.Count(); i++)
                    {
                        if (_lstWord[i] == "from")
                        {
                            fromTalbe = _lstWord[i + 1];
                            if (fromTalbe.StartsWith("("))
                                continue;
                            break;
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(fromTalbe))
                    {
                        var dbName = fromTalbe.Split('_').FirstOrDefault(o => o.Length == 3);
                        if (!string.IsNullOrWhiteSpace(dbName))
                        {
                            dbName = dbName.ToUpper();
                        }

                        if (dbName == "HIS")
                        {
                            result = "MOS";
                        }
                        else if (dbName == "SAR")
                        {
                            result = "";
                        }
                        else
                        {
                            result = dbName;
                        }
                    }
                }
                else
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            finally // đảm bảo luôn có từ khóa Entities
            {
                
                    result += entities;
            }
            return result;
        }
       
    }
}
