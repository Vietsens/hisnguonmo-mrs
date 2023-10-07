using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBloodAbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisBloodAboCFG
    {
        private static List<HIS_BLOOD_ABO> hisBloodAbos;
        public static List<HIS_BLOOD_ABO> HisBloodAbos
        {
            get
            {
                if (hisBloodAbos == null || hisBloodAbos.Count == 0)
                {
                    hisBloodAbos = GetBloodAbo();
                }
                return hisBloodAbos;
            }
        }

        private static List<HIS_BLOOD_ABO> GetBloodAbo()
        {
            List<HIS_BLOOD_ABO> result = new List<HIS_BLOOD_ABO>();
            try
            {
                result = new HisBloodAboManager().Get(new HisBloodAboFilterQuery());
                if (result == null) throw new NullReferenceException();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<HIS_BLOOD_ABO>();
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                hisBloodAbos = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
