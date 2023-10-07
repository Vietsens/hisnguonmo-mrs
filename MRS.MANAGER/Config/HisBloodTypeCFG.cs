using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBloodType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisBloodTypeCFG
    {
        private static List<HIS_BLOOD_TYPE> hisBloodTypes;
        public static List<HIS_BLOOD_TYPE> HisBloodTypes
        {
            get
            {
                if (hisBloodTypes == null || hisBloodTypes.Count == 0)
                {
                    hisBloodTypes = GetBloodType();
                }
                return hisBloodTypes;
            }
        }

        private static List<HIS_BLOOD_TYPE> GetBloodType()
        {
            List<HIS_BLOOD_TYPE> result = new List<HIS_BLOOD_TYPE>();
            try
            {
                result = new HisBloodTypeManager().Get(new HisBloodTypeFilterQuery());
                if (result == null) throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<HIS_BLOOD_TYPE>();
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                hisBloodTypes = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
