using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using HTC.MANAGER.Manager;
using HTC.EFMODEL.DataModels;
using HTC.MANAGER.Core.ExpenseBO.HtcExpenseType.Get;

namespace MRS.MANAGER.Config
{
    public class HtcExpenseTypeCFG
    {
        private const string SDA_CONFIG__HTC_EXPENSE_TYPE_CODEs = "HTC.HTC_EXPENSE_TYPE.EXPENSE_TYPE_CODEs";
        private const string SDA_CONFIG__HTC_EXPENSE_TYPE_CODE_CPTT = "HTC.HTC_EXPENSE_TYPE.EXPENSE_TYPE_CODE_CPTT";
        private const string SDA_CONFIG__HTC_EXPENSE_TYPE_CODE_CPGT = "HTC.HTC_EXPENSE_TYPE.EXPENSE_TYPE_CODE_CPGT";
        private const string SDA_CONFIG__HTC_EXPENSE_TYPE_CODE_CPB = "HTC.HTC_EXPENSE_TYPE.EXPENSE_TYPE_CODE_CPB";
        private const string HTC_EXPENSE_TYPE_CODE__NS = "HTC.HTC_EXPENSE_TYPE.EXPENSE_TYPE_CODE__NS";

        private static List<long> expenseTypeIds;
        public static List<long> HTC_EXPENSE_TYPE_IDs
        {
            get
            {
                if (expenseTypeIds == null || expenseTypeIds.Count == 0)
                {
                    expenseTypeIds = GetIdByCode(SDA_CONFIG__HTC_EXPENSE_TYPE_CODEs);
                }
                return expenseTypeIds;
            }
            set
            {
                expenseTypeIds = value;
            }
        }

        private static string expenseTypeCodeCPTT;
        public static string HTC_EXPENSE_TYPE_CODE_CPTT
        {
            get
            {
                if (expenseTypeCodeCPTT == null)
                {
                    expenseTypeCodeCPTT = GetCode(SDA_CONFIG__HTC_EXPENSE_TYPE_CODE_CPTT);
                }
                return expenseTypeCodeCPTT;
            }
            set
            {
                expenseTypeCodeCPTT = value;
            }
        }

        private static string expenseTypeCodeCPGT;
        public static string HTC_EXPENSE_TYPE_CODE_CPGT
        {
            get
            {
                if (expenseTypeCodeCPGT == null)
                {
                    expenseTypeCodeCPGT = GetCode(SDA_CONFIG__HTC_EXPENSE_TYPE_CODE_CPGT);
                }
                return expenseTypeCodeCPGT;
            }
            set
            {
                expenseTypeCodeCPGT = value;
            }
        }

        private static string expenseTypeCodeCPB;
        public static string HTC_EXPENSE_TYPE_CODE_CPB
        {
            get
            {
                if (expenseTypeCodeCPB == null)
                {
                    expenseTypeCodeCPB = GetCode(SDA_CONFIG__HTC_EXPENSE_TYPE_CODE_CPB);
                }
                return expenseTypeCodeCPB;
            }
            set
            {
                expenseTypeCodeCPB = value;
            }
        }

        private static string expenseTypeCodeNS;
        public static string HTC_EXPENSE_TYPE_CODE_NS
        {
            get
            {
                if (String.IsNullOrEmpty(expenseTypeCodeNS))
                {
                    expenseTypeCodeNS = GetCode(HTC_EXPENSE_TYPE_CODE__NS);
                }
                return expenseTypeCodeNS;
            }
            set
            {
                expenseTypeCodeNS = value;
            }
        }

        public static List<long> GetIdByCode(string expenseTypeCodes)
        {
            List<long> result = null;
            try
            {
                var config = Loader.dictionaryConfig[expenseTypeCodes];
                if (config == null) throw new ArgumentNullException(expenseTypeCodes);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(expenseTypeCodes);
                List<string> CODEs = value.Split(new Char[] { ',' }).ToList();

                HtcExpenseTypeFilterQuery filter = new HtcExpenseTypeFilterQuery();
                //filter.GENDER_CODE = value;//TODO
                var data = new HTC.MANAGER.Manager.HtcExpenseTypeManager(new CommonParam()).Get<List<HTC_EXPENSE_TYPE>>(filter);

                result = data.Where(o => CODEs.Contains(o.EXPENSE_TYPE_CODE)).Select(p => p.ID).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static string GetCode(string expenseTypeCodes)
        {
            string result = null;
            try
            {
                var config = Loader.dictionaryConfig[expenseTypeCodes];
                if (config == null) throw new ArgumentNullException(expenseTypeCodes);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(expenseTypeCodes);
                result = value;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                expenseTypeIds = null;
                expenseTypeCodeCPTT = null;
                expenseTypeCodeCPGT = null;
                expenseTypeCodeCPB = null;
                expenseTypeCodeNS = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
