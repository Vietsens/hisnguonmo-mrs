using Inventec.Core;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisAccountBook
{
    public partial class HisAccountBookDAO : EntityBase
    {
        public List<V_HIS_ACCOUNT_BOOK> GetView(HisAccountBookSO search, CommonParam param)
        {
            List<V_HIS_ACCOUNT_BOOK> result = new List<V_HIS_ACCOUNT_BOOK>();

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

        public HIS_ACCOUNT_BOOK GetByCode(string code, HisAccountBookSO search)
        {
            HIS_ACCOUNT_BOOK result = null;

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
        
        public V_HIS_ACCOUNT_BOOK GetViewById(long id, HisAccountBookSO search)
        {
            V_HIS_ACCOUNT_BOOK result = null;

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

        public V_HIS_ACCOUNT_BOOK GetViewByCode(string code, HisAccountBookSO search)
        {
            V_HIS_ACCOUNT_BOOK result = null;

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

        public Dictionary<string, HIS_ACCOUNT_BOOK> GetDicByCode(HisAccountBookSO search, CommonParam param)
        {
            Dictionary<string, HIS_ACCOUNT_BOOK> result = new Dictionary<string, HIS_ACCOUNT_BOOK>();
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
