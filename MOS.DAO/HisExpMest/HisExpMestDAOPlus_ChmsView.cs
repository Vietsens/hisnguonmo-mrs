using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMest
{
    public partial class HisExpMestDAO : EntityBase
    {
        public List<V_HIS_EXP_MEST_CHMS> GetChmsView(HisExpMestSO search, CommonParam param)
        {
            List<V_HIS_EXP_MEST_CHMS> result = new List<V_HIS_EXP_MEST_CHMS>();

            try
            {
                result = GetWorker.GetChmsView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public V_HIS_EXP_MEST_CHMS GetChmsViewById(long id, HisExpMestSO search)
        {
            V_HIS_EXP_MEST_CHMS result = null;

            try
            {
                result = GetWorker.GetChmsViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public V_HIS_EXP_MEST_CHMS GetChmsViewByCode(string code, HisExpMestSO search)
        {
            V_HIS_EXP_MEST_CHMS result = null;

            try
            {
                result = GetWorker.GetChmsViewByCode(code, search);
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
