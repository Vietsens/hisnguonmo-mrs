using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMest
{
    public partial class HisImpMestDAO : EntityBase
    {
        public List<V_HIS_IMP_MEST> GetView(HisImpMestSO search, CommonParam param)
        {
            List<V_HIS_IMP_MEST> result = new List<V_HIS_IMP_MEST>();

            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public HIS_IMP_MEST GetByCode(string code, HisImpMestSO search)
        {
            HIS_IMP_MEST result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
        
        public V_HIS_IMP_MEST GetViewById(long id, HisImpMestSO search)
        {
            V_HIS_IMP_MEST result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public V_HIS_IMP_MEST GetViewByCode(string code, HisImpMestSO search)
        {
            V_HIS_IMP_MEST result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, HIS_IMP_MEST> GetDicByCode(HisImpMestSO search, CommonParam param)
        {
            Dictionary<string, HIS_IMP_MEST> result = new Dictionary<string, HIS_IMP_MEST>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
