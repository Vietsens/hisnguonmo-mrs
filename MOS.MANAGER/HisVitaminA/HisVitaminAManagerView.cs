using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.MANAGER.HisVitaminA
{
    public partial class HisVitaminAManager : BusinessBase
    {
        public List<V_HIS_VITAMIN_A> GetView(HisVitaminAViewFilterQuery filter)
        {
            List<V_HIS_VITAMIN_A> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                List<V_HIS_VITAMIN_A> resultData = null;
                if (valid)
                {
                    resultData = new HisVitaminAGet(param).GetView(filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public V_HIS_VITAMIN_A GetViewById(long data)
        {
            V_HIS_VITAMIN_A result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                V_HIS_VITAMIN_A resultData = null;
                if (valid)
                {
                    resultData = new HisVitaminAGet(param).GetViewById(data);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }

        public V_HIS_VITAMIN_A GetViewById(long data, HisVitaminAViewFilterQuery filter)
        {
            V_HIS_VITAMIN_A result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(data);
                valid = valid && IsNotNull(filter);
                V_HIS_VITAMIN_A resultData = null;
                if (valid)
                {
                    resultData = new HisVitaminAGet(param).GetViewById(data, filter);
                }
                result = resultData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
