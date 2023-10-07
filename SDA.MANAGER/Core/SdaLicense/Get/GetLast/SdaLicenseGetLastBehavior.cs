using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Base;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace SDA.MANAGER.Core.SdaLicense.Get.GetLast
{
    class SdaLicenseGetLastBehavior : BeanObjectBase, ISdaLicenseGetLast
    {
        SdaLicenseFilterQuery filterQuery;
        private const long CHECK_TIME = 1000000; //1 ngay

        internal SdaLicenseGetLastBehavior(CommonParam param, SdaLicenseFilterQuery filter)
            : base(param)
        {
            filterQuery = filter;
        }

        SDA_LICENSE ISdaLicenseGetLast.Run()
        {
            SDA_LICENSE result = null;
            try
            {
                long? now = Inventec.Common.DateTime.Get.Now();
                if (Math.Abs(now.Value - filterQuery.Time) > CHECK_TIME)
                {
                    Logging("Thoi gian khong hop le. Thoi diem check la: " + now.Value + ". Hieu so la:" + Math.Abs(now.Value - filterQuery.Time) + "." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filterQuery), filterQuery), LogType.Warn);
                }
                else if (String.IsNullOrWhiteSpace(filterQuery.PUK) || String.IsNullOrWhiteSpace(filterQuery.SN_HDD) || String.IsNullOrWhiteSpace(filterQuery.APP_CODE))
                {
                    Logging("Khong du thong tin truy van." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filterQuery), filterQuery), LogType.Warn);
                }
                else
                {
                    filterQuery.ORDER_FIELD = "CREATE_TIME";
                    List<SDA_LICENSE> licenses = DAOWorker.SdaLicenseDAO.Get(filterQuery.Query(), param);
                    if (licenses != null && licenses.Count > 0)
                    {
                        result = licenses[0];
                    }
                    else
                    {
                        Logging("Khong lay duoc license" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filterQuery), filterQuery), LogType.Warn);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
    }
}
