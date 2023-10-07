using MOS.DAO.Base;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MOS.DAO.HisAccidentVehicle
{
    partial class HisAccidentVehicleGet : EntityBase
    {
        public Dictionary<string, HIS_ACCIDENT_VEHICLE> GetDicByCode(HisAccidentVehicleSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCIDENT_VEHICLE> dic = new Dictionary<string, HIS_ACCIDENT_VEHICLE>();
            try
            {
                List<HIS_ACCIDENT_VEHICLE> listRecord = Get(search, param);
                if (listRecord != null)
                {
                    foreach (var item in listRecord)
                    {
                        if (!dic.ContainsKey(item.ACCIDENT_VEHICLE_CODE))
                        {
                            dic.Add(item.ACCIDENT_VEHICLE_CODE, item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => search), search) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => param), param), LogType.Error);
                LogSystem.Error(ex);
                dic.Clear();
            }
            return dic;
        }
    }
}
