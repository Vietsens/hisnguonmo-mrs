using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisServiceUnit;

namespace MRS.MANAGER.Config
{
    public class HisServiceUnitCFG
    {
        //private const string SERVICE_UNIT_CODE__ = "MRS.HIS_RS.HIS_SERVICE_UNIT.SERVICE_UNIT_CODE.";


        //private static long serviceUnitId;
        //public static long SERVICE_UNIT_ID__
        //{
        //    get
        //    {
        //        if (serviceUnitId == 0)
        //        {
        //            serviceUnitId = GetId(SERVICE_UNIT_CODE__);
        //        }
        //        return serviceUnitId;
        //    }
        //    set
        //    {
        //        serviceUnitId = value;
        //    }
        //}

        private static List<HIS_SERVICE_UNIT> hisServiceUnits;
        public static List<HIS_SERVICE_UNIT> HisServiceUnits
        {
            get
            {
                if (hisServiceUnits == null || hisServiceUnits.Count == 0)
                {
                    HisServiceUnitFilterQuery filter = new HisServiceUnitFilterQuery();
                    hisServiceUnits = new HisServiceUnitManager().Get(filter);
                }
                return hisServiceUnits;
            }
        }

        //private static long GetId(string code)
        //{
        //    long result = 0;
        //    try
        //    {
        //        var config = Loader.dictionaryConfig[code];
        //        if (config == null) throw new ArgumentNullException(code);
        //        string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
        //        if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
        //        HisServiceUnitFilterQuery filter = new HisServiceUnitFilterQuery();
        //        //filter.SERVICE_RYPE_CODE = value;
        //        var data = new HisServiceUnitManager().Get(filter).FirstOrDefault(o => o.SERVICE_UNIT_CODE == value);
        //        if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
        //        result = data.ID;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //        result = 0;
        //    }
        //    return result;
        //}

        //private static List<long> GetListId(string code)
        //{
        //    List<long> result = new List<long>();
        //    try
        //    {
        //        var config = Loader.dictionaryConfig[code];
        //        if (config == null) throw new ArgumentNullException(code);
        //        string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
        //        var arr = value.Split(',');
        //        if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
        //        var data = HisServiceUnits.Where(o => arr.Contains(o.SERVICE_UNIT_CODE)).ToList();
        //        if (!(data != null && data.Count > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
        //        result = data.Select(o => o.ID).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Info("CODE:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => code), code));
        //        LogSystem.Error(ex);
        //    }
        //    return result;
        //}

        public static void Refresh()
        {
            try
            {
                hisServiceUnits = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
