using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisImpMest
{
    public partial class HisImpMestDAO : EntityBase
    {
        public List<V_HIS_IMP_MEST_MANU> GetManuView(HisImpMestSO search, CommonParam param)
        {
            List<V_HIS_IMP_MEST_MANU> result = new List<V_HIS_IMP_MEST_MANU>();
            try
            {
                result = GetWorker.GetManuView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_IMP_MEST_MANU GetManuViewById(long id, HisImpMestSO search)
        {
            V_HIS_IMP_MEST_MANU result = null;

            try
            {
                result = GetWorker.GetManuViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public V_HIS_IMP_MEST_MANU GetManuViewByCode(string code, HisImpMestSO search)
        {
            V_HIS_IMP_MEST_MANU result = null;

            try
            {
                result = GetWorker.GetManuViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

    }
}
