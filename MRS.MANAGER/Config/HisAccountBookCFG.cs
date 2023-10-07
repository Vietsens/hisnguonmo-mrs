using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisAccountBook;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisAccountBookCFG
    {
        private const string ACCOUNT_BOOK_CODE__OUT_TREATS = "MRS.HIS_ACCOUNT_BOOK.ACCOUNT_BOOK_CODE.OUT_TREATS";

        private static List<long> accountBookIdRetiredMilitary;
        public static List<long> ACCOUNT_BOOK_ID__OUT_TREAT
        {
            get
            {
                if (accountBookIdRetiredMilitary == null || accountBookIdRetiredMilitary.Count == 0)
                {
                    accountBookIdRetiredMilitary = GetIds(ACCOUNT_BOOK_CODE__OUT_TREATS);
                }
                return accountBookIdRetiredMilitary;
            }
            set
            {
                accountBookIdRetiredMilitary = value;
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
            long result = -1;//de chi thuc hien load 1 lan
            try
            {
                List<string> codes = new List<string>();
                codes = GetCodes(ACCOUNT_BOOK_CODE__OUT_TREATS);

                var data = (new HisAccountBookManager().Get(new HisAccountBookFilterQuery()) ?? new List<HIS_ACCOUNT_BOOK>()).Where(o => codes.Contains(o.ACCOUNT_BOOK_CODE));
                if (data == null) throw new ArgumentNullException(code);
                accountBookIds.AddRange(data.Select(o => o.ID).ToList());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return accountBookIds;
        }

        public static void Refresh()
        {
            try
            {
                accountBookIdRetiredMilitary = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
