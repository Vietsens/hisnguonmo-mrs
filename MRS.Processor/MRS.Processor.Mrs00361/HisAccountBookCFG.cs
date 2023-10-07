using Inventec.Common.Logging; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisAccountBook; 
using SDA.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Text.RegularExpressions; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00361
{
    public class HisAccountBookCFG
    {
        private const string ACCOUNT_BOOK_CODE__KSKs = "MRS.HIS_ACCOUNT_BOOK.ACCOUNT_BOOK_CODE.KSKs"; 

        private static List<long> accountBookIdKsks; 
        public static List<long> ACCOUNT_BOOK_ID__KSKs
        {
            get
            {
                if (accountBookIdKsks==null||accountBookIdKsks.Count == 0)
                {
                    accountBookIdKsks = GetIds(ACCOUNT_BOOK_CODE__KSKs); 
                }
                return accountBookIdKsks; 
            }
            set
            {
                accountBookIdKsks = value; 
            }
        }

        private static List<string> GetCodes(string code)
        {
            List<string> result = new List<string>(); 
            try
            {
                var config = Loader.dictionaryConfig[code]; 
                if (config == null) throw new ArgumentNullException(code); 
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE; 
                string pattern = ","; 
                Regex myRegex = new Regex(pattern); 
                string[] Codes = myRegex.Split(value); 
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code); 

                if (!(Codes != null) || Codes.Length <= 0) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config)); 
                foreach (var item in Codes) ///
                {
                    result.Add(item); 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = null; 
            }
            return result; 
        }

        private static List<long> GetIds(string code)
        {
            List<long> accountBookIds = new List<long>(); 
            long result = -1; //de chi thuc hien load 1 lan
            try
            {
                List<string> codes = new List<string>(); 
                codes = GetCodes(ACCOUNT_BOOK_CODE__KSKs);
                var data = new HisAccountBookManager().GetByCodes(codes); 
                if (data == null) throw new ArgumentNullException(code); 
                accountBookIds.AddRange(data.Select(o => o.ID).ToList()); 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
            return accountBookIds; 
        }
    }
}
