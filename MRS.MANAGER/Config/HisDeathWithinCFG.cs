using MOS.MANAGER.HisDeathWithin;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisDeathWithinCFG
    {
        private const string MRS_DEATH_WITHIN_CODE__24HOURS = "MRS.HIS_RS.HIS_DEATH_WITHIN.DEATH_WITHIN_CODE.24HOURS";

        private static long DeathWithinId24Hours;
        public static long DEATH_WITHIN_ID__24HOURS
        {
            get
            {
                if (DeathWithinId24Hours == 0)
                {
                    DeathWithinId24Hours = GetId(MRS_DEATH_WITHIN_CODE__24HOURS);
                }
                return DeathWithinId24Hours;
            }
            set
            {
                DeathWithinId24Hours = value;
            }
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
                HisDeathWithinFilterQuery filter = new HisDeathWithinFilterQuery();
                //filter.ROOM_TYPE_CODE = value;//TODO
                var data = new HisDeathWithinManager().Get(filter).FirstOrDefault(o => o.DEATH_WITHIN_CODE == value);
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

        public static void Refresh()
        {
            try
            {
                DeathWithinId24Hours = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
