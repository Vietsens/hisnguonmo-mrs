using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAllow;
using MOS.MANAGER.HisService;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    class HisServiceCFG
    {
        private const string SERVICE_CODE__OVER_EXPEND = "MOS.HIS_SERVICE.SERVICE_CODE.OVER_EXPEND"; //ma cua dich vu duoc cau hinh de luu gia tien gan cho BN trong truong hop so dich vu hao phi vuot qua gioi han hao phi cho phep
        private const string SERVICE_CODE__AUTO_FINISH_WHILE_TREATMENT_FINISH = "MOS.HIS_TREATMENT.AUTO_FINISH_SERVICE_REQ.SERVICE_CODE";// danh sach ma dich vu se tu dong ket thuc yeu cau khi ho so dieu tri duoc ket thuc

        private static long serviceIdOverExpend;
        public static long SERVICE_ID__OVER_EXPEND
        {
            get
            {
                if (serviceIdOverExpend == 0)
                {
                    serviceIdOverExpend = GetId(SERVICE_CODE__OVER_EXPEND);
                }
                return serviceIdOverExpend;
            }
            set
            {
                serviceIdOverExpend = value;
            }
        }

        private static List<long> autoFinishServiceIds = null;
        public static List<long> AUTO_FINISH__SERVICE_IDs
        {
            get
            {
                if (autoFinishServiceIds == null)
                {
                    autoFinishServiceIds = GetIds(SERVICE_CODE__AUTO_FINISH_WHILE_TREATMENT_FINISH);
                }
                return autoFinishServiceIds;
            }
            set
            {
                autoFinishServiceIds = value;
            }
        }

        private static List<V_HIS_SERVICE> dataView;
        public static List<V_HIS_SERVICE> DATA_VIEW
        {
            get
            {
                if (dataView == null)
                {
                    dataView = new HisServiceGet().GetView(new HisServiceViewFilterQuery());
                }
                return dataView;
            }
            set
            {
                dataView = value;
            }
        }

        private static long GetId(string code)
        {
            string value = ConfigUtil.GetStrConfig(code);
            if (!string.IsNullOrWhiteSpace(value))
            {
                V_HIS_SERVICE service = DATA_VIEW != null ? DATA_VIEW.Where(o => o.SERVICE_CODE == value).FirstOrDefault() : null;
                return service != null ? service.ID : -1;
            }
            return -1;
        }

        private static List<long> GetIds(string code)
        {
            List<long> result = new List<long>();
            try
            {
                List<string> value = ConfigUtil.GetStrConfigs(code);
                if (value != null && value.Count > 0)
                {
                    List<V_HIS_SERVICE> services = DATA_VIEW != null ? DATA_VIEW.Where(o => value.Contains(o.SERVICE_CODE)).ToList() : null;
                    if (services != null && services.Count > 0)
                        result = services.Select(s => s.ID).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<long>();
            }
            return result;
        }

        public static void Reload()
        {
            var data = new HisServiceGet().GetView(new HisServiceViewFilterQuery());
            dataView = data;
            var idOverExpend = GetId(SERVICE_CODE__OVER_EXPEND);
            serviceIdOverExpend = idOverExpend;
            var autoFinishId = GetIds(SERVICE_CODE__AUTO_FINISH_WHILE_TREATMENT_FINISH);
            autoFinishServiceIds = autoFinishId;
        }
    }
}
