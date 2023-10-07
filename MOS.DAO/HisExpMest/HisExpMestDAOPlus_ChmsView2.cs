using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisExpMest
{
    public partial class HisExpMestDAO : EntityBase
    {
        public List<V_HIS_EXP_MEST_CHMS_2> GetChmsView2(HisExpMestSO search, CommonParam param)
        {
            List<V_HIS_EXP_MEST_CHMS_2> result = new List<V_HIS_EXP_MEST_CHMS_2>();

            try
            {
                result = GetWorker.GetChmsView2(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public V_HIS_EXP_MEST_CHMS_2 GetChmsView2ById(long id, HisExpMestSO search)
        {
            V_HIS_EXP_MEST_CHMS_2 result = null;

            try
            {
                result = GetWorker.GetChmsView2ById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public V_HIS_EXP_MEST_CHMS_2 GetChmsView2ByCode(string code, HisExpMestSO search)
        {
            V_HIS_EXP_MEST_CHMS_2 result = null;

            try
            {
                result = GetWorker.GetChmsView2ByCode(code, search);
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
