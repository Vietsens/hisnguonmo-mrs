using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisGender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00490
{
    public class HisGenderCFG
    {
        private static List<HIS_GENDER> hisGender;
        public static List<HIS_GENDER> HisGender
        {
            get
            {
                if (hisGender == null || hisGender.Count == 0)
                {
                    hisGender = new List<HIS_GENDER>();
                    hisGender.AddRange(GetAll());
                }
                return hisGender;
            }
        
        }

        private static List<HIS_GENDER> GetAll()
        {
            List<HIS_GENDER> result = new List<HIS_GENDER>();
            try
            {
                HisGenderFilterQuery filter = new HisGenderFilterQuery();
                result = new HisGenderManager().Get(filter);
                if (result == null) result = new List<HIS_GENDER>();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<HIS_GENDER>();
            }
            return result;
        }
    }
}
