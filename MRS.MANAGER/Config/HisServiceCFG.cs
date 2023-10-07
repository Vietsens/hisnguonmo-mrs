using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisService;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisServiceCFG
    {
        private const string SERVICE_CODE__HIV = "MRS.HIS_SERVICE.SERVICE_CODE.HIV";

        private static List<string> serviceCodeHivs;
        public static List<string> SERVICE_CODE__HIVs
        {
            get
            {
                if (serviceCodeHivs == null)
                {
                    serviceCodeHivs = GetListCode(SERVICE_CODE__HIV);
                }
                return serviceCodeHivs;
            }

        }

        private const string SERVICE_CODE__HBSAG = "MRS.HIS_SERVICE.SERVICE_CODE.HBSAG";

        private static List<string> serviceCodeHbsAgs;
        public static List<string> SERVICE_CODE__HBSAGs
        {
            get
            {
                if (serviceCodeHbsAgs == null)
                {
                    serviceCodeHbsAgs = GetListCode(SERVICE_CODE__HBSAG);
                }
                return serviceCodeHbsAgs;
            }

        }

        private const string SERVICE_CODE__KSK = "DBCODE.HIS_RS.HIS_SERVICE.SERVICE_CODE.KSK";//KSK
        private const string SERVICE_CODE__KND = "MRS.HIS_SERVICE.SERVICE_CODE.KIDNEY_DIALYSISES";

        private static List<string> ListserviceCodeKND;
        public static List<string> getList_SERVICE_CODE__KND
        {
            get
            {
                if (ListserviceCodeKND == null)
                {
                    ListserviceCodeKND = GetListCode(SERVICE_CODE__KND);
                }
                return ListserviceCodeKND;
            }
            set
            {
                ListserviceCodeKND = value;
            }
        }

        private static List<string> ListserviceCodeKSK;
        public static List<string> getList_SERVICE_CODE__KSK
        {
            get
            {
                if (ListserviceCodeKSK == null)
                {
                    ListserviceCodeKSK = GetListCode(SERVICE_CODE__KSK);
                }
                return ListserviceCodeKSK;
            }
            set
            {
                ListserviceCodeKSK = value;
            }
        }

        private static string GetCode(string code)
        {
            string result = "";
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);

                result = value;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private static List<string> GetListCode(string code)
        {
            List<string> result = new List<string>();
            try
            {

                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                var arr = value.Split(',').ToList();

                result = arr;


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Info("CODE:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code));
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                serviceCodeHivs = null;
                serviceCodeHbsAgs = null;
                ListserviceCodeKND = null;
                ListserviceCodeKSK = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
