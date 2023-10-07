using Inventec.Common.Logging;
using MOS.MANAGER.HisHeinServiceType;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Proccessor.Mrs00562
{
    public class HisHeinServiceTypeCFG
    {
        public const string HEIN_SERVICE_TYPE__BLOOD__IN__VTYT = "0";
        public const string HEIN_SERVICE_TYPE__BLOOD__IN__THUOC = "1";
        public const string HEIN_SERVICE_TYPE__BLOOD__IN__DVKT = "2";

        public const string HEIN_SERVICE_TYPE__TRAN__IN__VTYT = "0";
        public const string HEIN_SERVICE_TYPE__TRAN__IN__THUOC = "1";
        public const string HEIN_SERVICE_TYPE__TRAN__IN__DVKT = "2";

        private const string HEIN_SERVICE_TYPE_CODE_BLOOD__SELECTEBHYT = "MRS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE_BLOOD__SELECTEBHYT";
        private const string HEIN_SERVICE_TYPE_CODE_TRAN__SELECTBHYT = "MRS.HIS_HEIN_SERVICE_TYPE.HEIN_SERVICE_TYPE_CODE_TRAN__SELECTEBHYT";

        private static string heinsServiceTypeBlood__SelectBhyt;
        public static string HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT
        {
            get
            {
                if (String.IsNullOrEmpty(heinsServiceTypeBlood__SelectBhyt))
                {
                    heinsServiceTypeBlood__SelectBhyt = GetCode(HEIN_SERVICE_TYPE_CODE_BLOOD__SELECTEBHYT);
                }
                return heinsServiceTypeBlood__SelectBhyt;
            }
        }

        private static string heinServiceTypeTran__SelectBhyt;
        public static string HEIN_SERVICE_TYPE_TRAN__SELECTBHYT
        {
            get
            {
                if (String.IsNullOrEmpty(heinServiceTypeTran__SelectBhyt))
                {
                    heinServiceTypeTran__SelectBhyt = GetCode(HEIN_SERVICE_TYPE_CODE_TRAN__SELECTBHYT);
                }
                return heinServiceTypeTran__SelectBhyt;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_HEIN_SERVICE_TYPE> heinServiceTypes;
        public static List<MOS.EFMODEL.DataModels.HIS_HEIN_SERVICE_TYPE> HEIN_SERVICE_TYPEs
        {
            get
            {
                if (heinServiceTypes == null)
                {
                    heinServiceTypes = GetAll();
                }
                return heinServiceTypes;
            }
            set
            {
                heinServiceTypes = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_HEIN_SERVICE_TYPE> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_HEIN_SERVICE_TYPE> result = null;
            try
            {
                HisHeinServiceTypeFilterQuery filter = new HisHeinServiceTypeFilterQuery();
                result = new HisHeinServiceTypeManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }


        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                HisHeinServiceTypeFilterQuery filter = new HisHeinServiceTypeFilterQuery();
                //filter.HEIN_SERVICE_TYPE_CODE = value;
                var data = new HisHeinServiceTypeManager().Get(filter).FirstOrDefault(o => o.HEIN_SERVICE_TYPE_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.ID;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        private static string GetCode(string code)
        {
            string result = null;
            try
            {
                if (!Loader.dictionaryConfig.ContainsKey(code))
                {
                    throw new ArgumentNullException(code);
                }
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                result = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private static bool GetBool(string code)
        {
            bool result = false;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                result = (value == "1");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

    }
}
