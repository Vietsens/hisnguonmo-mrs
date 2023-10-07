using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HcmPoorFundCFG
    {
        //d/s cac ma dich vu ma quy ho ngheo HCM chi tra cho ho Vuot can ngheo
        private const string VCN_ACCEPTED_SERVICE_CODE_CFG = "MOS.HCM_POOR_FUND.VCN_ACCEPTED_SERVICE_CODE";

        private static List<long> vcnAcceptServiceIds;
        public static List<long> VCN_ACCEPT_SERVICE_IDS
        {
            get
            {
                if (vcnAcceptServiceIds == null)
                {
                    vcnAcceptServiceIds = GetIds(VCN_ACCEPTED_SERVICE_CODE_CFG);
                }
                return vcnAcceptServiceIds;
            }
            set
            {
                vcnAcceptServiceIds = value;
            }
        }

        private static List<long> GetIds(string code)
        {
            List<long> result = null;
            try
            {
                result = new List<long>();
                List<string> data = ConfigUtil.GetStrConfigs(code);
                foreach (string t in data)
                {
                    string[] tmp = t.Split(':');
                    if (tmp != null && tmp.Length >= 2)
                    {
                        V_HIS_SERVICE service = HisServiceCFG.DATA_VIEW.Where(o => o.SERVICE_TYPE_CODE == tmp[0] && o.SERVICE_CODE == tmp[1]).FirstOrDefault();
                        if (service != null)
                        {
                            result.Add(service.ID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static void Reload()
        {
            var serviceIds = GetIds(VCN_ACCEPTED_SERVICE_CODE_CFG);
            vcnAcceptServiceIds = serviceIds;

        }
    }
}
